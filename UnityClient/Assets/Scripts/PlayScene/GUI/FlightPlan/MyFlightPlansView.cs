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
