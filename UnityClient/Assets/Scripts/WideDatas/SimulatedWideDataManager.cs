using System;
using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json;

using UnityEngine;

public class SimulatedWideDataManager : WideDataManager {

    SerializeContainer _container = null;

    public Routes _routes = new Routes();

    public SimulatedShipManager _shipManager = null;
    public SimulatedHangarManager _hangarManager = null;
    public SimulatedPOIsManager _POIsManager = null;
    public SimulatedMarketManager _marketManager = null;

    public float _loadTime = 1.0f;
    private long _requestID = 0;
    private float _timeAcc = 0.0f;
    private float _frameLength = 0.1f;

    private void Awake() {
        _container = new SerializeContainer();
        _shipManager = new SimulatedShipManager(this, _container);
        _hangarManager = new SimulatedHangarManager(this, _container);
        _POIsManager = new SimulatedPOIsManager(this, _container);
        _marketManager = new SimulatedMarketManager(this, _container);

        //creer un peu de données mais ne rien envoyer au model puor le moment    
        Character Andre = new Character(1);
        Andre.Name = "Andre";
        Andre.Loaded = true;
        Andre.Corp = -1;
        _container._characters.Add(1, Andre);
        MailBox m1 = new MailBox(1);
        m1.Loaded = true;
        _container._mailBoxs.Add(1, m1);

        Station albanel = new Station(1);
        albanel.Name = "Albanel";
        albanel.Loaded = true;
        _container._stations.Add(1, albanel);                

        Station normandin = new Station(2);
        normandin.Name = "Normandin";
        normandin.Loaded = true;
        _container._stations.Add(2, normandin);

        Station dolbeau = new Station(3);
        dolbeau.Name = "Dolbeau";
        dolbeau.Loaded = true;
        _container._stations.Add(3, dolbeau);

        Station saintMethod = new Station(4);
        saintMethod.Name = "Saint-Methode";
        saintMethod.Loaded = true;
        _container._stations.Add(4, saintMethod);

        Station mistassini = new Station(5);
        mistassini.Name = "Mistassini";
        mistassini.Loaded = true;
        _container._stations.Add(5, mistassini);

        Station saintFelicien = new Station(6);
        saintFelicien.Name = "Saint-Félicien";
        saintFelicien.Loaded = true;
        _container._stations.Add(saintFelicien.ID, saintFelicien);

        Station chambord = new Station(7);
        chambord.Name = "Chambord";
        chambord.Loaded = true;
        _container._stations.Add(chambord.ID, chambord);

        _routes.AddRoute(1, 2, 800.0f);
        _routes.AddRoute(1, 3, 800.0f);
        _routes.AddRoute(2, 4, 800.0f);
        _routes.AddRoute(3, 4, 800.0f);
        _routes.AddRoute(3, 5, 200.0f);
        _routes.AddRoute(4, 6, 1200.0f);
        _routes.AddRoute(6, 7, 1200.0f);

    }

    public class SerializeContainer {

        public Dictionary<int, Corporation> _corps = null;
        public Dictionary<int, Character> _characters = null;
        public Dictionary<int, Station> _stations = null;
        public Dictionary<int, Hangar> _hangars = null;
        public Dictionary<int, Ship> _ships = null;
        public Dictionary<int, MailBox> _mailBoxs = null;
        public Dictionary<int, ResourceStack> _resourceStacks = null;
        public Dictionary<int, PointOfInterest> _POIs = null;
        public Dictionary<int, Bookmark> _bookmarks = null;
        public List<MarketData> _market = null;
        public Dictionary<int, FlightPlan> _flightPlans = null;
        public Dictionary<int, Fleet> _fleets = null;

        public int _shipIDs = 1;
        public int _hangarIDs = 1;
        public int _currentFrame = 1;
        public int _mailIDs = 1;
        public int _resourceStackIDs = 1;
        public int _POIsIDs = 1;
        public int _bookmarkIDs = 1;
        public int _flightPlanIDs = 1;
        public int _fleetIDs = 1;

        public SerializeContainer() {
            _corps = new Dictionary<int, Corporation>();
            _characters = new Dictionary<int, Character>();
            _stations = new Dictionary<int, Station>();
            _hangars = new Dictionary<int, Hangar>();
            _ships = new Dictionary<int, Ship>();
            _mailBoxs = new Dictionary<int, MailBox>();
            _resourceStacks = new Dictionary<int, ResourceStack>();
            _POIs = new Dictionary<int, PointOfInterest>();
            _bookmarks = new Dictionary<int, Bookmark>();
            _market = new List<MarketData>();
            _flightPlans = new Dictionary<int, FlightPlan>();
            _fleets = new Dictionary<int, Fleet>();
    }   
    };

    private void Update() {
        _timeAcc += Time.deltaTime;
        while(_timeAcc > _frameLength) {
            //do one frame
            _shipManager.UpdateOneFrame();
            _POIsManager.UpdateOneFrame();
            _marketManager.UpdateOneFrame();

            _container._currentFrame++;
            _timeAcc -= _frameLength;
        }
    }

    public void LoadFromString(string data) {

        _container = JsonConvert.DeserializeObject<SerializeContainer>(data);
        _shipManager = new SimulatedShipManager(this, _container);
        _hangarManager = new SimulatedHangarManager(this, _container);
        _POIsManager = new SimulatedPOIsManager(this, _container);
        _marketManager = new SimulatedMarketManager(this, _container);
    }

    public string SaveToString() {

        string str = JsonConvert.SerializeObject(_container);

        return str;
    }

    public int Frame { get { return _container._currentFrame; } }

    public override long SendRequest(Request r) {
        r.RequestID = _requestID++;
        string type = r.GetType().ToString();
        switch (type) {
            case "BuyHangarRequest": StartCoroutine(BuyHangarRequest(r)); break;                
            case "CreateCorpRequest": StartCoroutine(CreateCorpRequest(r)); break;
            case "CreateFlightPlanRequest":StartCoroutine(CreateFlightPlanRequest(r)); break;
            case "DeleteMailRequest": StartCoroutine(DeleteMailRequest(r)); break;
            case "DeleteFlightPlanRequest": StartCoroutine(DeleteFlightPlanRequest(r)); break;
            case "FleetCreateRequest": StartCoroutine(FleetCreateRequest(r)); break;            
            case "GetCurrentFrameRequest": StartCoroutine(GetCurrentFrameRequest(r)); break;
            case "GetFlightPlanRequest": StartCoroutine(GetFlightPlanRequest(r)); break;            
            case "LoadCharacterRequest": StartCoroutine(LoadCharacterRequest(r)); break;                 
            case "LoadCorporationRequest": StartCoroutine(LoadCorporationRequest(r)); break;
            case "LoadResourceStackRequest": StartCoroutine(LoadResourceStackRequest(r)); break;            
            case "LoadStationsRequest": StartCoroutine(LoadStationsRequest(r)); break;
            case "LoadRoutesRequest": StartCoroutine(LoadRoutesRequest(r)); break;                
            case "LoadMyShipsRequest": StartCoroutine(LoadMyShipsRequest(r)); break;
            case "LoadHangarRequest": StartCoroutine(LoadHangarRequest(r)); break;
            case "LoadShipRequest": StartCoroutine(LoadShipRequest(r)); break;            
            case "MarketDataRequest": StartCoroutine(MarketDataRequest(r)); break;
            case "MarketPlaceSellOrderRequest": StartCoroutine(MarketPlaceSellOrderRequest(r)); break;
            case "MarketPlaceBuyOrderRequest": StartCoroutine(MarketPlaceBuyOrderRequest(r)); break;            
            case "SendMailRequest": StartCoroutine(SendMailRequest(r)); break;                
            default:
            throw new Exception("Unknown Request " + type);
        }

        return r.RequestID;
    }

    public void SendServerUpdate(ServerUpdate s) {
        ServerUpdate(s);
    }

    private IEnumerator CreateCorpRequest(Request r) {
        yield return new WaitForSeconds(_loadTime);

        CreateCorpRequest c = r as CreateCorpRequest;

        Corporation c1 = new Corporation(_container._corps.Count + 1);
        c1.Loaded = true;
        c1.Name = c.Name;
        c1.Owner = c.Char;
        c1.Station = c.Station;
        c1.ICU = 1000;
        c1.FlightPlans = new List<int>();

        _container._corps.Add(c1.ID, c1);
        _container._characters[c.Char].Corp = c1.ID;

        Hangar h = CreateHangar(c.Station, c1.ID);
        Ship s = CreateShip(h.ID);

        c.Corporation = c1;
        c.Character = _container._characters[c.Char];
        c.Hangar = h;
        c.Ship = s;


        FinishRequest(r);
    }

    private IEnumerator CreateFlightPlanRequest(Request r) {
        CreateFlightPlanRequest c = r as CreateFlightPlanRequest;

        c.Corp = _container._corps[c.CorpID];

        FlightPlan f = null;
        if (c.PlanID == -1) {
            f = new FlightPlan(_container._flightPlanIDs++);
            f.Loaded = true;
            _container._flightPlans.Add(f.ID, f);
            c.Corp.FlightPlans.Add(f.ID);
        } else {
            f = _container._flightPlans[c.PlanID];
        }

        f.Data = c.Data;
        f.Name = c.Name;       

        FinishRequest(c);
        yield break;
    }

    public Ship CreateShip(int hangarID) {

        if (!_container._hangars.ContainsKey(hangarID))
            return null;

        Hangar h = _container._hangars[hangarID];
        Corporation c = _container._corps[h.Corp];
        Character owner = _container._characters[c.Owner];
        

        if (c.ICU < 50)
            return null;

        Ship ship = new Ship(_container._shipIDs++);
        ship.Loaded = true;
        ship.Name = "Battlestar " + ship.ID;
        ship.Corp = h.Corp;
        ship.Status = "Idle";
        ship.Hangar = h.ID;

        _container._ships.Add(ship.ID, ship);

        h.Ships.Add(ship.ID);

        c.ICU -= 50;

        return ship;
    }

    public Hangar CreateHangar(int stationID, int corpID) {

        Hangar h = new Hangar(_container._hangarIDs++);
        h.Loaded = true;
        h.Station = stationID;
        h.Corp = corpID;
        h.StacksIDs = new List<int>();
        _container._hangars.Add(h.ID, h);
        _container._stations[stationID].Hangars.Add(corpID, h.ID);

        return h;
    }

    private IEnumerator LoadCharacterRequest(Request r) {
        yield return new WaitForSeconds(_loadTime);

        LoadCharacterRequest l = r as LoadCharacterRequest;

        if (_container._characters.ContainsKey(l.CharacterID)) {
            l.Character = _container._characters[l.CharacterID];
            l.MailBox = _container._mailBoxs[l.CharacterID];
        } else {
            l.Character = null;
        }

        FinishRequest(r);        
    }

    private IEnumerator LoadCorporationRequest(Request r) {
        yield return new WaitForSeconds(_loadTime);

        LoadCorporationRequest l = r as LoadCorporationRequest;
        if (_container._corps.ContainsKey(l.CorpID)) {
            l.Corp = _container._corps[l.CorpID];
            FinishRequest(l);
        }
    }

    private IEnumerator LoadStationsRequest(Request r) {
        yield return new WaitForSeconds(_loadTime);

        LoadStationsRequest l = r as LoadStationsRequest;
        l.Stations = new List<Station>(_container._stations.Values);

        FinishRequest(r);
    }

    private IEnumerator GetCurrentFrameRequest(Request r) {
        yield return new WaitForSeconds(0.01f);

        GetCurrentFrameRequest l = r as GetCurrentFrameRequest;
        l.Frame = _container._currentFrame;

        FinishRequest(l);
    }

    private IEnumerator GetFlightPlanRequest(Request r) {
        yield return new WaitForSeconds(0.01f);

        GetFlightPlanRequest g = r as GetFlightPlanRequest;
        g.Plans = new List<FlightPlan>();
        foreach(int i in g.PlanIDs) {
            g.Plans.Add(_container._flightPlans[i]);
        }
        FinishRequest(g);
    }    

    private IEnumerator LoadRoutesRequest(Request r) {
        yield return new WaitForSeconds(_loadTime);

        LoadRoutesRequest l = r as LoadRoutesRequest;

        l.Routes = _routes;

        FinishRequest(l);
    }

    private IEnumerator LoadMyShipsRequest(Request r) {
        yield return new WaitForSeconds(_loadTime);

        LoadMyShipsRequest l = r as LoadMyShipsRequest;

        List<Ship> myShips = new List<Ship>();

        foreach(Ship s in _container._ships.Values) {
            if(s.Corp == l.Corp) {
                myShips.Add(s);
            }
        }
        l.Ships = myShips;

        FinishRequest(l);
    }

    private IEnumerator LoadHangarRequest(Request r) {
        yield return new WaitForSeconds(_loadTime);

        LoadHangarRequest l = r as LoadHangarRequest;
        
        if (_container._hangars.ContainsKey(l.HangarID)) {
            l.Hangar = _container._hangars[l.HangarID];
            l.Stacks = new List<ResourceStack>();
            foreach(int i in l.Hangar.StacksIDs) {
                l.Stacks.Add(_container._resourceStacks[i]);
            }
            FinishRequest(l);
        }        
    }

    private IEnumerator LoadResourceStackRequest(Request r) {
        yield return new WaitForSeconds(_loadTime);
        
        LoadResourceStackRequest s = r as LoadResourceStackRequest;
        if (_container._resourceStacks.ContainsKey(s.StackID)) {
            s.Stack = _container._resourceStacks[s.StackID];

            FinishRequest(s);
        }
    }

    private IEnumerator LoadShipRequest(Request r) {
        yield return new WaitForSeconds(_loadTime);

        LoadShipRequest l = r as LoadShipRequest;
        if (_container._ships.ContainsKey(l.ShipID)) {
            l.Ship = _container._ships[l.ShipID];
            FinishRequest(l);
        }
    }   

    private IEnumerator BuyHangarRequest(Request r) {
        yield return new WaitForSeconds(_loadTime);
        
        BuyHangarRequest b = r as BuyHangarRequest;

        Station s = _container._stations[b.StationID];
        if (s.Hangars.ContainsKey(b.CorpID))
            yield break;

        Hangar h = CreateHangar(b.StationID, b.CorpID);

        b.Hangar = h;
        b.Station = s;

        FinishRequest(b);
    }

    private IEnumerator SendMailRequest(Request r) {
        yield return new WaitForSeconds(_loadTime);

        SendMailRequest s = r as SendMailRequest;

        Mail m = new Mail(_container._mailIDs++);
        m.Title = s.Subject;
        m.Content = s.Message;

        s.MailBox = _container._mailBoxs[s.DestID];
        s.MailBox.AddMail(m);

        FinishRequest(s);

    }

    private IEnumerator DeleteFlightPlanRequest(Request r) {
        yield return new WaitForSeconds(_loadTime);

        DeleteFlightPlanRequest d = r as DeleteFlightPlanRequest;

        d.Corporation = _container._corps[d.CorpID];
        d.Corporation.FlightPlans.Remove(d.PlanID);
        _container._flightPlans.Remove(d.PlanID);

        FinishRequest(d);
    }

    private IEnumerator FleetCreateRequest(Request r) {
        yield return new WaitForSeconds(_loadTime);

        FleetCreateRequest f = r as FleetCreateRequest;
        //tester que les vaisseaux sont au hangar
        //tester que le hangar est au joueur
        //tester que les vaisseaux sont disponibles 

        Fleet fleet = new Fleet(_container._fleetIDs++);
        _container._fleets.Add(fleet.ID, fleet);
        fleet.Data = f.Data;
        fleet.CurrentNode = 0;
        fleet.TaskStartFrame = _container._currentFrame;
        fleet.ShipIDs = f.ShipIDs;
        fleet.LastHangar = f.FromHangarID;
        Hangar hangar = _container._hangars[f.FromHangarID];
        fleet.LastStation = hangar.Station;

        //assigner les vaisseaux a la flotte
        f.Ships = new List<Ship>();
        foreach(int s in f.ShipIDs) {
            Ship ship = _container._ships[s];
            ship.Fleet = fleet.ID;
            f.Ships.Add(ship);
        }
        f.Fleet = fleet;

        FinishRequest(f);
    }

    private IEnumerator DeleteMailRequest(Request r) {
        yield return new WaitForSeconds(_loadTime);

        DeleteMailRequest d = r as DeleteMailRequest;

        d.Mailbox = _container._mailBoxs[d.MailboxID];
        d.Mailbox.RemoveMail(d.MailID);

        FinishRequest(d);
    }

    private IEnumerator MarketDataRequest(Request r) {
        yield return new WaitForSeconds(_loadTime);

        MarketDataRequest m = r as MarketDataRequest;

        m.Datas = _marketManager.GetBuyOrders(m.ResourceType);

        FinishRequest(m);
    }

    private IEnumerator MarketPlaceSellOrderRequest(Request r) {
        yield return new WaitForSeconds(_loadTime);

        MarketPlaceSellOrderRequest m = r as MarketPlaceSellOrderRequest;

        m.Hangar = _container._hangars[m.HangarID];
        
        if (_hangarManager.RemoveResourceToHangar(m.HangarID, m.ResourceType, m.Qte)) {
            //on a reussi a enlever les ressources on fait la requete
            _marketManager.CreateSellOrder(m.ResourceType, m.Qte, m.Price);
            Corporation corp = _container._corps[m.Hangar.Corp];
            corp.ICU += m.Qte * m.Price;
            m.Corp = corp;
        } else {
            //pas suffisement de ressources
            m.Result = -1;
        }
        
        FinishRequest(m);
    }
    
    private IEnumerator MarketPlaceBuyOrderRequest(Request r) {
        yield return new WaitForSeconds(_loadTime);

        MarketPlaceBuyOrderRequest m = r as MarketPlaceBuyOrderRequest;

        m.Hangar = _container._hangars[m.HangarID];
        m.Corp = _container._corps[m.Hangar.Corp];

        CreateShip(m.Hangar.ID);

        FinishRequest(m);
    }

}
