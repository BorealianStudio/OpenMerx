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

using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class MyFlightPlansView : MonoBehaviour {
    [SerializeField, Tooltip("Le prefab a utilser pour instancier une ligne")]
    FlightPlanLineInfos linePrefab = null;
    [SerializeField, Tooltip("Le transform parent des lignes")]
    Transform content = null;
    [SerializeField, Tooltip("Button qui lance la creation d'un plan de vol")]
    Button createNewButton = null;

    Corporation _corp = null;

    private void Start() {        
        createNewButton.onClick.AddListener(() => {
            Destroy(GetComponentInParent<Window>().gameObject);
            PrefabManager pm = FindObjectOfType<PrefabManager>();
            WindowSystem ws = FindObjectOfType<WindowSystem>();
            FlightPlanEditor editor = Instantiate(pm.prefabFlightPlanEditorView);
            Window w = ws.NewWindow("FlightPlanEditor", editor.gameObject);
            w.SetWidth(1200);
            w.SetHeight(700);
            w.Title = "Flight plan editor";
            w.Show();
        });

        LocalDataManager.instance.OnCorporationChange += OnCorporationChange;
    }

    private void OnDestroy() {
        LocalDataManager.instance.OnCorporationChange -= OnCorporationChange;
    }

    private void  OnCorporationChange(Corporation c) {
        if(null != _corp && c.ID == _corp.ID) {
            UpdateList();
        }
    }

    public void SetCorporation(Corporation corp) {
        _corp = corp;
        StartCoroutine(WaitForPlans());        
    }

    private IEnumerator WaitForPlans() {

        bool loop = true;
        long? id = 0;
        WideDataManager.RequestCB frameMethCB = delegate (Request r) {
            if(r.RequestID == id) {
                loop = false;
            }
        };

        WideDataManager.wideDataManager.OnRequestResult += frameMethCB;                
        id = WideDataManager.Request(new GetFlightPlanRequest(_corp.FlightPlans));
        while (loop) {
            yield return null;
        }
        WideDataManager.wideDataManager.OnRequestResult -= frameMethCB;

        UpdateList();
    }

    private void UpdateList() {
        if (null == _corp)
            return;

        while(content.childCount > 0) {
            Transform t = content.GetChild(0);
            t.SetParent(null);
            Destroy(t.gameObject);
        }

        foreach (FlightPlan f in LocalDataManager.instance.GetFlightPlans(_corp)) {
            FlightPlanLineInfos line = Instantiate(linePrefab, content);
            line.SetFlightPlan(f);
        }
    }
}
