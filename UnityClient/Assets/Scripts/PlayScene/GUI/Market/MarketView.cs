// MIT License

// Copyright(c) 2017 Andre Plourde
// part of https://github.com/BorealianStudio/OpenMerx

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.


// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

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
