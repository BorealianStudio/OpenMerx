using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class FlightPlanLineInfos : MonoBehaviour, IPointerClickHandler {

    [SerializeField, Tooltip("text qui affiche le nom du plan de vol")]
    Text planName = null;

    [SerializeField, Tooltip("button pour detruire ce plan de vol")]
    Button deleteButton = null;

    FlightPlan _plan = null;

    private void Awake() {
        deleteButton.onClick.AddListener(OnDeleteRequest);
    }

    public void SetFlightPlan(FlightPlan f) {
        planName.text = f.Name;
        _plan = f;
    }

    private void OnDeleteRequest() {
        WideDataManager.Request(new DeleteFlightPlanRequest(_plan.ID, LocalDataManager.instance.LocalCorporation.ID));
    }

    public void OnPointerClick(PointerEventData eventData) {
        Destroy(GetComponentInParent<Window>().gameObject);
        PrefabManager pm = FindObjectOfType<PrefabManager>();
        WindowSystem ws = FindObjectOfType<WindowSystem>();
        FlightPlanEditor editor = Instantiate(pm.prefabFlightPlanEditorView);
        Window w = ws.NewWindow("FlightPlanEditor", editor.gameObject);
        w.SetWidth(1200);
        w.SetHeight(700);
        editor.SetPlan(_plan);
        w.Title = "Flight plan editor";
        w.Show();
    }
}
