using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class FleetPlanEditorView : MonoBehaviour {

    [SerializeField, Tooltip("Liste des modele de plans du joueur")]
    Dropdown plans = null;

    [SerializeField, Tooltip("Button pour nettoyer l'editeur")]
    Button clearButton = null;

    [SerializeField, Tooltip("Bouton pour lancer la flotte")]
    Button launchButton = null;

    NodalEditor _editor = null;

    List<FlightPlan> _plans = null;

    List<int> _shipIDs = null;
    int _hangarID = 0;

    private void Start() {
        _editor = GetComponentInChildren<NodalEditor>();
        clearButton.onClick.AddListener(_editor.Init);
        launchButton.onClick.AddListener(OnLauch);
        plans.onValueChanged.AddListener(PlanSelect);

        plans.options.Clear();
        _plans = LocalDataManager.instance.GetFlightPlans(LocalDataManager.instance.LocalCorporation);
        plans.options.Add(new Dropdown.OptionData("Custom"));
        foreach(FlightPlan f in _plans) { 
            plans.options.Add(new Dropdown.OptionData(f.Name));
        }

        plans.value = 0;
        plans.RefreshShownValue();
    }

    public void SetData(List<int> ships, int hangarID) {
        _shipIDs = ships;
        _hangarID = hangarID;
    }

    private void PlanSelect(int value) {
        if (value == 0) {            
            _editor.Init();
        } else {
            _editor.Load(_plans[value - 1].Data);
        }
    }

    private void OnLauch() {
        WideDataManager.Request(new FleetCreateRequest(_hangarID, _shipIDs, _editor.Save()));
        Destroy(GetComponentInParent<Window>().gameObject);
    }
}
   