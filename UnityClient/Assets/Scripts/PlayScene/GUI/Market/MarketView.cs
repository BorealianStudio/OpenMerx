using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class MarketView : MonoBehaviour {
    [SerializeField] Transform resourceList = null;
    [SerializeField] MarketResourceLine resourceLinePrefab = null;
    [SerializeField] Transform dataZone = null;
    [SerializeField] Button createOrderButton = null;

    [SerializeField] Transform dataList = null;
    [SerializeField] MarketEntryLine marketEntryLinePrefab = null;

    private List<ResourceInfos> products = new List<ResourceInfos>();
    private long _reqID = 0;
    private int _currentResourceID = 0;

    private void Awake() {
        foreach (ResourceInfos r in LocalDataManager.instance.ResourceDataManager.GetAllResources()) {
            products.Add(r);
        }

        createOrderButton.onClick.AddListener(OnCreateSellOrderClic);
    }

    void Start () {
        foreach (ResourceInfos r in products) {
            MarketResourceLine l = Instantiate(resourceLinePrefab, resourceList);
            l.SetResource(r);
            l.OnClic += OnResourceClic;
        }        
	}
       
    void OnResourceClic(MarketResourceLine line) {
        _currentResourceID = line.ResourceType.Type;
        LocalDataManager.instance.OnRequestDone += OnRequestDone;
        _reqID = WideDataManager.Request(new MarketDataRequest(line.ResourceType.Type));
        dataZone.gameObject.SetActive(false);
    }

    void OnCreateSellOrderClic() {
        PrefabManager pm = FindObjectOfType<PrefabManager>();
        WindowSystem ws = FindObjectOfType<WindowSystem>();
        MarketOrder order = Instantiate(pm.prefabMarketOrderView);
        Window w = ws.NewWindow("marketOrder", order.gameObject);
        order.SetResource(_currentResourceID);
        w.Title = "Selling...";
        w.Show();
    }

    private void OnRequestDone(Request r) {
        if(r.RequestID == _reqID) {
            LocalDataManager.instance.OnRequestDone -= OnRequestDone;
            while(dataList.childCount > 0) {
                GameObject o = dataList.GetChild(0).gameObject;
                o.transform.SetParent(null);
                Destroy(o);
            }

            MarketDataRequest request = r as MarketDataRequest;
            foreach(MarketData m in request.Datas) {
                MarketEntryLine line = Instantiate(marketEntryLinePrefab, dataList);
                line.SetData(m);
            }

            dataZone.gameObject.SetActive(true);
        }
    }
}
