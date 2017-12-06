using UnityEngine;
using UnityEngine.UI;

public class ShipInfoLine : MonoBehaviour {
    [SerializeField,Tooltip("Text contenant le nom du vaisseau")]
    Text shipName = null;
    [SerializeField, Tooltip("Text contenant le Numero de la flotte en cours")]
    Text fleetNumber = null;
    [SerializeField, Tooltip("Text contenant le status actuel du vaisseua")]
    Text shipStatus = null;

    private Ship _ship;

    public void SetShip(Ship s) {
        _ship = s;
        LocalDataManager.instance.OnShipChange += ShipUpdate;
        UpdateView();
    }

    public void OnClic() {
        WindowSystem ws = FindObjectOfType<WindowSystem>();
        PrefabManager pm = FindObjectOfType<PrefabManager>();

        ShipView view = Instantiate(pm.prefabShipView);
        Window w = ws.NewWindow("ShipView",view.gameObject);
        
        view.SetShip(_ship);
        w.Show();
    }

    private void OnDestroy() {
        LocalDataManager.instance.OnShipChange -= ShipUpdate;
    }

    private void ShipUpdate(Ship s) {
        if (_ship != null && _ship.ID == s.ID) {
            UpdateView();
        }
    }

    private void UpdateView() {
        if(_ship != null && _ship.Loaded) {
            shipName.text = _ship.Name;
            shipStatus.text = _ship.Status;
            fleetNumber.text = _ship.Fleet.ToString();
        } else {
            //loading
        }
    }

}
