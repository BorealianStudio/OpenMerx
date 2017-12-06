using System.Collections.Generic;
using System.Collections;

using UnityEngine;

/// <summary>
/// Le local data manager est responsable de contenir les données necessaire au jeu et les 
/// mettre a jour de deux facon.
/// - Il simule l'evolution des objets
/// - Il se met a jour quand le WideDataManager lui donne une information plus precise
/// 
/// Il faut savoir que les données en LocalDataManager sont seulement les données que peut 
/// consulter le joueur local. Le WideDataManager selectionne les informations a passer.
/// 
/// </summary>
public class LocalDataManager : MonoBehaviour {
    
    public static LocalDataManager instance = null;
    private LinkedList<DataSystem> _systems = new LinkedList<DataSystem>();
    public ResourceDataManager ResourceDataManager { get; private set; }

    public delegate void RequestIDDone(Request r);
    public event RequestIDDone OnRequestDone = delegate { };

    #region events
    /// <summary> event quand un nouveau vaisseau est ajouté aux données </summary>
    public event EventHolder.ShipCB OnNewShip = delegate {};

    /// <summary> event quand un vaisseau est supprime des donnes locales </summary>
    public event EventHolder.ShipCB OnRemoveShip = delegate {};

    /// <summary> event leve quand un nouvel objet est cree dans le modele local </summary>
    public event EventHolder.DataObjectCB OnNewObject = delegate { };

    /// <summary> event leve a chaque changement de frame </summary>
    public event EventHolder.BasicCB OnTimeChange = delegate { };

    public EventHolder.ShipCB OnShipChange = delegate { };
    public EventHolder.StationCB OnStationChange = delegate { };
    public EventHolder.MailboxCB OnMailboxChange = delegate { };
    public EventHolder.HangarCB OnHangarChange = delegate { };
    public EventHolder.CharacterCB OnCharacterChange = delegate { };   
    public EventHolder.CorporationCB OnCorporationChange = delegate { };
    public EventHolder.ResourceStackCB OnResourceStackChange = delegate { };


    #endregion


    private List<EventHolder> eventHolder = new List<EventHolder>();
        
    public int LocalCharacterID { get; private set; }
    public Character LocalCharacter { get; private set; }
    public Corporation LocalCorporation { get; private set; }

    private Dictionary<int, DataObject> objects = new Dictionary<int, DataObject>();
    private int objIdIndex = 0;
        
    private Dictionary<int, Corporation> corps = new Dictionary<int, Corporation>();
    private Dictionary<int, Character> characters = new Dictionary<int, Character>();
    private Dictionary<int, Ship> ships = new Dictionary<int, Ship>();
    private Dictionary<int, Station> stations = new Dictionary<int, Station>();
    private Dictionary<int, Hangar> hangars = new Dictionary<int, Hangar>();
    private Dictionary<int, ResourceStack> stacks = new Dictionary<int, ResourceStack>();
    private Dictionary<int, FlightPlan> flightPlans = new Dictionary<int, FlightPlan>();
    private Dictionary<int, Fleet> fleets = new Dictionary<int, Fleet>();

    private MailBox mailBox = null;
    
    public bool Ready { get;  private set; }

    private float timeAcc = 0.0f;
    private float frameTime = 0.1f;
    private int frame = 0;  // frame since 1 janvier 2017?

    #region dataStructures
    public MyShipList MyShipList { get; private set; }

    #endregion

    public void Awake() {
        Application.runInBackground = true;

        _systems.AddLast(new ShipMovementSystem(this));

        ResourceDataManager = new ResourceDataManager();
    }

    private void Update() {
        timeAcc += Time.deltaTime;
        while (timeAcc > frameTime) {            
            timeAcc -= frameTime;
            foreach(DataSystem s in _systems) {
                s.Update();
            }
            frame++;
            OnTimeChange();
        }
    }

    public void SetLocalCharacterID(int id) {
        Ready = false;

        LocalCharacterID = id;

        WideDataManager.wideDataManager.OnRequestResult += OnRequestResult;
        WideDataManager.wideDataManager.OnServerUpdate += OnServerUpdate;

        StartCoroutine(Prepare(id));        
    }

    public int Frame { get { return frame; } }
    public MailBox Mailbox { get { return mailBox; } }

    private IEnumerator Prepare(int id) {

        MyShipList = new MyShipList(this);

        //charger la current Frame
        long frameReqID = WideDataManager.Request(new GetCurrentFrameRequest());
        bool waitForFrame = true;
        RequestIDDone frameMethCB = delegate (Request r) {
            if (r.RequestID == frameReqID) {
                GetCurrentFrameRequest g = r as GetCurrentFrameRequest;
                waitForFrame = false;
                frame = g.Frame;
            }
        };

        OnRequestDone += frameMethCB;        
        while (waitForFrame) {
            yield return null;
        }        
        OnRequestDone -= frameMethCB;

        //charger le character du joeur
        WideDataManager.wideDataManager.SendRequest(new LoadCharacterRequest(id));
        while (!characters.ContainsKey(id) || !characters[id].Loaded) {
            yield return null;
        }

        //charger egalement sa mailbox
        while(mailBox == null || !mailBox.Loaded) {
            yield return null;
        }

        //charger la corportation du joueur si elle existe
        float waiting = 3.0f;
        if(characters.ContainsKey(id) && characters[id].Corp != -1) {
            while(!corps.ContainsKey(characters[id].Corp) || !corps[characters[id].Corp].Loaded) {
                if (waiting > 2.0f) {
                    WideDataManager.wideDataManager.SendRequest(new LoadCorporationRequest(characters[id].Corp));
                    waiting = 0.0f;
                }
                waiting += Time.deltaTime;
                yield return null;
            }
        }       

        //charger la liste des stations de l'univers
        long reqID = WideDataManager.wideDataManager.SendRequest(new LoadStationsRequest());

        bool stationLoadOK = false;
        RequestIDDone methCB = delegate (Request r) { if (r.RequestID == reqID) stationLoadOK = true; };

        OnRequestDone += methCB;
        while (!stationLoadOK) {
            yield return null;
        }
        OnRequestDone -= methCB;

/*
        reqID = WideDataManager.wideDataManager.SendRequest(new LoadRoutesRequest());

        bool routesOK = false;
        methCB = delegate (Request r) {
            if (r.RequestID == reqID) {
                LoadRoutesRequest lrr = r as LoadRoutesRequest;
                routes = lrr.Routes;
                routesOK = true;
            }
        };

        OnRequestDone += methCB;
        while (!routesOK) {
            yield return null;
        }
        OnRequestDone -= methCB;
*/
        Ready = true;

        TutorialManager.RaiseEvent("gameStart");
    }   

    private void OnRequestResult(Request r) {

        //mise a jour des objets modifies
        CBHolder holder = new CBHolder();
        holder.charCB = CharacterUpdate;
        holder.corpCB = CorporationUpdate;
        holder.hangarCB= HangarUpdate;
        holder.resourceStackCB = ResourceStackUpdate;
        holder.shipCB = ShipUpdate;
        holder.stationCB = StationUpdate;
        holder.mailboxCB = MailboxUpdate;
        holder.flighPlanCB = FlightPlanUpdate;
        holder.fleetCB = FleetUpdate;

        r.Update(holder);

        // propagation de tout les events levés
        SendEvents();

        OnRequestDone(r);
    }

    private void SendEvents() {
        foreach (EventHolder e in eventHolder) {
            e.Exectute();
        }
        eventHolder.Clear();
    }

    public Corporation GetCorporationInfo(int id) {
        Corporation result = null;
        if (!corps.ContainsKey(id)) {
            Corporation c = new Corporation(id);
            AddObject(c);
            corps.Add(id, c);
            
        }
        result = corps[id];

        if (!result.Loaded)
            WideDataManager.wideDataManager.SendRequest(new LoadCorporationRequest(id));

        return result;
    }

    public Character GetCharacterInfo(int characterID) {
        Character result = null;
        if (!characters.ContainsKey(characterID)) {
            Character c = new Character(characterID);
            AddObject(c);
            characters.Add(characterID, c);
            
        }        
        result = characters[characterID];
        if (!result.Loaded)
            WideDataManager.wideDataManager.SendRequest(new LoadCharacterRequest(characterID));
        return result;
    }

    public Fleet GetFleetInfo(int fleetID) {
        Fleet result = null;
        if (!fleets.ContainsKey(fleetID)) {
            result = new Fleet(fleetID);
            AddObject(result);
            fleets.Add(fleetID, result);

        }
        result = fleets[fleetID];

        return result;
    }

    public Hangar GetHangarInfo(int hangarID) {

        Hangar result = null;
        if (!hangars.ContainsKey(hangarID)) {
            Hangar h = new Hangar(hangarID);
            hangars.Add(hangarID, new Hangar(hangarID));
            AddObject(h);
        }
        result = hangars[hangarID];
        if (!result.Loaded)
            WideDataManager.wideDataManager.SendRequest(new LoadHangarRequest(hangarID));
        return result;

    }

    public Hangar GetHangarInfo(int stationID, int corpID) {
        if (!stations.ContainsKey(stationID) || !stations[stationID].Loaded )
            return null;

        Station station = stations[stationID];
        if (!station.Hangars.ContainsKey(corpID))
            return null;

        int hangarID = station.Hangars[corpID];

        return GetHangarInfo(hangarID);
    }

    public ResourceStack GetResourceStackInfo(int stackID) {
        ResourceStack result = null;
        if (!stacks.ContainsKey(stackID)) {
            result = new ResourceStack(stackID);
            stacks.Add(stackID, result);
            AddObject(result);
        }
        result = stacks[stackID];

        if (!result.Loaded) {
            WideDataManager.Request(new LoadResourceStackRequest(stackID));
        }
        return result;
    }

    public List<FlightPlan> GetFlightPlans(Corporation c) {
        List<FlightPlan> result = new List<FlightPlan>();
        foreach(int i in c.FlightPlans) {
            result.Add(flightPlans[i]);
        }

        return result;
    }

    public Ship GetShipInfo(int shipID) {
        Ship result = null;
        if (!ships.ContainsKey(shipID)) {
            Ship s = new Ship(shipID);
            AddObject(s);
            ships.Add(shipID, s);
        }
        result = ships[shipID];
        if (!result.Loaded)
            WideDataManager.Request(new LoadShipRequest(shipID));

        return result;
    }

    public List<Station> GetStations() {
        return new List<Station>(stations.Values);
    }

    // ********************************************************************
    //                              UPDATES
    // ********************************************************************

    private void FleetUpdate(Fleet f) {
        if (null == f)
            return;
        if (!fleets.ContainsKey(f.ID)) {
            Fleet fleet = new Fleet(f.ID);
            AddObject(fleet);
            fleets.Add(fleet.ID, fleet);
        }
        eventHolder.AddRange(fleets[f.ID].Update(f));
    }

    private void ShipUpdate(Ship s) {
        if (null == s)
            return;
        if (!ships.ContainsKey(s.ID)) {
            Ship ship = new Ship(s.ID);
            AddObject(ship);
            ships.Add(ship.ID, ship);
            ships[s.ID].Update(s);
            OnNewShip(ship);
            return;
        }
        eventHolder.AddRange(ships[s.ID].Update(s));
    }

    private void CorporationUpdate(Corporation cb) {
        if (null == cb)
            return;
        if (!corps.ContainsKey(cb.ID)) {
            Corporation corp = new Corporation(cb.ID);
            AddObject(corp);
            corps.Add(corp.ID, corp);
        }
        eventHolder.AddRange(corps[cb.ID].Update(cb));

        if (LocalCorporation == null &&  cb.Owner == LocalCharacterID) {
            LocalCorporation = corps[cb.ID];
            MyShipList.SetLocalCorp(cb.ID);
        }
            
    }

    private void CharacterUpdate(Character c) {
        if (null == c)
            return;
        if (!characters.ContainsKey(c.ID)) {
            Character character = new Character(c.ID);
            AddObject(character);
            characters.Add(character.ID, character);
        }
        eventHolder.AddRange(characters[c.ID].Update(c));

        if (null == LocalCharacter && c.ID == LocalCharacterID)
            LocalCharacter = characters[c.ID];
        
    }

    private void StationUpdate(Station s) {
        if (null == s)
            return;
        if (!stations.ContainsKey(s.ID)) {
            Station station = new Station(s.ID);
            AddObject(station);
            stations.Add(s.ID, s);
        }
        eventHolder.AddRange(stations[s.ID].Update(s));
    }

    private void FlightPlanUpdate(FlightPlan f) {
        if (null == f)
            return;
        if (!flightPlans.ContainsKey(f.ID)) {
            FlightPlan fp = new FlightPlan(f.ID);
            AddObject(fp);
            flightPlans.Add(fp.ID, fp);
        }        
        eventHolder.AddRange(flightPlans[f.ID].Update(f));
    }

    private void HangarUpdate(Hangar h) {
        if (null == h)
            return;
        if (!hangars.ContainsKey(h.ID))
            hangars.Add(h.ID, h);
        eventHolder.AddRange(hangars[h.ID].Update(h));
    }

    private void ResourceStackUpdate(ResourceStack r) {
        if (null == r)
            return;
        if (!stacks.ContainsKey(r.ID))
            stacks.Add(r.ID, r);
        eventHolder.AddRange(stacks[r.ID].Update(r));
    }

    private void MailboxUpdate(MailBox m) {
        if (null == m)
            return;
        if (m.ID == LocalCharacter.ID) {
            if (null == mailBox)
                mailBox = new MailBox(m.ID);
            eventHolder.AddRange(mailBox.Update(m));
        }        
    }

    private void AddObject(DataObject obj) {
        obj.DataID = objIdIndex++;
        objects.Add(obj.DataID, obj);
        OnNewObject(obj);
    }

    // -------------------------------------------------------------
    //                              SERVER UPDATES
    // -------------------------------------------------------------
    private void OnServerUpdate(ServerUpdate update) {
        foreach(Ship s in update.ships) {
            ShipUpdate(s);
        }
        foreach(Hangar h in update.hangars) {
            HangarUpdate(h);
        }
        foreach(Corporation c in update.corps) {
            CorporationUpdate(c);
        }
        foreach(ResourceStack r in update.stacks) {
            ResourceStackUpdate(r);
        }

        SendEvents();
    }
}
