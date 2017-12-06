using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class HangarView : MonoBehaviour {

    [SerializeField] Transform resourcePart = null;
    [SerializeField] Transform shipPart = null;
    [SerializeField] Transform marketPart = null;
    [SerializeField] Button resourceTab = null;
    [SerializeField] Button shipTab = null;
    [SerializeField] Button marketTab = null;

    [SerializeField] ShipIcon shipIconPrefab = null;
    [SerializeField] GridLayoutGroup shipGrid = null;

    [SerializeField] ResourceIcon resourceIconPrefab = null;
    [SerializeField] GridLayoutGroup resourcesGrid = null;

    Hangar _hangar = null;

    private void Start() {
        LocalDataManager.instance.OnHangarChange += OnhangarChange;
        resourceTab.onClick.AddListener(() => ShowResources());
        shipTab.onClick.AddListener(() => ShowShips());
        marketTab.onClick.AddListener(() => ShowMarket());
        ShowResources();
    }

    private void OnDestroy() {
        LocalDataManager.instance.OnHangarChange -= OnhangarChange;
    }

    public void SetHangar(Hangar hangar) {
        _hangar = hangar;
        StartCoroutine(WaitForHangar());
    }
    
    public void OnBuyShipClic() {
        WideDataManager.Request(new MarketPlaceBuyOrderRequest(10, 1, 100, _hangar.ID));
    }

    public void OnStartFleet() {
        int hangarID = _hangar.ID;

        List<int> shipIDs = new List<int>();
        foreach (int i in _hangar.Ships) {
            Ship s = LocalDataManager.instance.GetShipInfo(i);
            if(s.Fleet <= 0) {
                shipIDs.Add(i);
            }
        }

        PrefabManager pm = FindObjectOfType<PrefabManager>();
        WindowSystem ws = FindObjectOfType<WindowSystem>();

        FleetPlanEditorView editor = Instantiate(pm.prefabFleetPlanEditorView);
        editor.SetData(shipIDs, hangarID);

        Window w = ws.NewWindow("editFleetPlan", editor.gameObject);
        w.Show();
    }

    private IEnumerator WaitForHangar() {
        Window w = GetComponentInParent<Window>();
        w.SetLoading(true);
        float time = 3.0f;
        while (!_hangar.Loaded) {
            if (time > 2.0f) {
                WideDataManager.wideDataManager.SendRequest(new LoadHangarRequest(_hangar.ID));
                time = 0.0f;
            }
            time += Time.deltaTime;
            yield return null;
        }
        foreach(int i in _hangar.StacksIDs) {
            ResourceStack r = LocalDataManager.instance.GetResourceStackInfo(i);
            while (!r.Loaded) {
                yield return null;
            }
        }
        long? id = -1;
        bool loop = true;
        WideDataManager.RequestCB frameMethCB = delegate (Request r) {
            if (r.RequestID == id) {
                loop = false;
            }
        };
        WideDataManager.wideDataManager.OnRequestResult += frameMethCB;
        id = WideDataManager.Request(new GetFlightPlanRequest(LocalDataManager.instance.LocalCorporation.FlightPlans));
        while (loop) {
            yield return null;
        }
        WideDataManager.wideDataManager.OnRequestResult -= frameMethCB;

        UpdateShips();
        UpdateResources();

        w.SetLoading(false);
        yield break;
    }

    private void OnhangarChange(Hangar h) {
        if(null != _hangar && h.Station == _hangar.Station && h.Corp == _hangar.Corp) {
            UpdateShips();
            UpdateResources();
        }
    }

    private void UpdateShips() {
        while(shipGrid.transform.childCount > 0) {
            Destroy(shipGrid.transform.GetChild(0).gameObject);
            shipGrid.transform.GetChild(0).SetParent(null);
        }

        //recuperer tous les vaisseaux dans ce hangar
        foreach (int i in _hangar.Ships) {
            ShipIcon icon = Instantiate(shipIconPrefab);
            Ship s = LocalDataManager.instance.GetShipInfo(i);
            icon.SetShip(s);
            icon.transform.SetParent(shipGrid.transform);
        }        
    }

    private void UpdateResources() {
        while (resourcesGrid.transform.childCount > 0) {
            Destroy(resourcesGrid.transform.GetChild(0).gameObject);
            resourcesGrid.transform.GetChild(0).SetParent(null);
        }

        foreach(int i in _hangar.StacksIDs) {
            ResourceIcon icon = Instantiate(resourceIconPrefab,resourcesGrid.transform);
            ResourceStack r = LocalDataManager.instance.GetResourceStackInfo(i);
            icon.SetStack(r);            
        }
    }

    private void ShowResources() {
        resourcePart.gameObject.SetActive(true);
        shipPart.gameObject.SetActive(false);
        marketPart.gameObject.SetActive(false);
    }

    private void ShowShips() {
        resourcePart.gameObject.SetActive(false);
        shipPart.gameObject.SetActive(true);
        marketPart.gameObject.SetActive(false);
    }

    private void ShowMarket() {
        resourcePart.gameObject.SetActive(false);
        shipPart.gameObject.SetActive(false);
        marketPart.gameObject.SetActive(true);
    }
}
