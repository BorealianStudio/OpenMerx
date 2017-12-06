using UnityEngine;
using UnityEngine.UI;

public class FlightPlanEditor : MonoBehaviour {

    [SerializeField, Tooltip("Bouton pour sauver le modele")]
    Button saveButton = null;
        
    [SerializeField, Tooltip("Container pour afficher le nom du modele en cours")]
    InputField modelName = null;

    [SerializeField, Tooltip("un lien vers le component NodalEditor pour charger et sauver")]
    NodalEditor nodalEditor = null;

    FlightPlan _plan = null;

    private void Start() {
        saveButton.onClick.AddListener(OnSaveClic);
    }
    
    public void SetPlan(FlightPlan plan) {
        _plan = plan;
        if (null != modelName)
            modelName.text = _plan.Name;
        nodalEditor.Load(plan.Data);
    }

    private void OnSaveClic() {
        int corpID = LocalDataManager.instance.LocalCorporation.ID;

        Destroy(GetComponentInParent<Window>().gameObject);

        WideDataManager.Request(new CreateFlightPlanRequest(_plan == null ? -1 : _plan.ID, corpID, nodalEditor.Save(), modelName.text));
    }

}
