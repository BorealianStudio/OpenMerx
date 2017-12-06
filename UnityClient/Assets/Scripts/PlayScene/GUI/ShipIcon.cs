using UnityEngine;

public class ShipIcon : MonoBehaviour {

    private Ship _ship = null;

    public void SetShip(Ship ship) {
        _ship = ship;
    }

    private void OnDestroy() {
        SetShip(null);
    }

    public void OnClic() {
        if(null != _ship) {
            WindowSystem ws = FindObjectOfType<WindowSystem>();
            PrefabManager pm = FindObjectOfType<PrefabManager>();

            ShipView v = Instantiate(pm.prefabShipView);           
            Window w = ws.NewWindow("ShipInfo", v.gameObject);
            v.SetShip(_ship);
            w.Show();
        }
    }
}
