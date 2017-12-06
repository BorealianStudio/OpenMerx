using UnityEngine;

public class ShipList : MonoBehaviour {

    MyShipList list = null;

    [SerializeField] Transform content = null;

    [SerializeField] ShipInfoLine linePrefab = null;

    private void Start() {
        list = LocalDataManager.instance.MyShipList;
        list.OnChange += OnChange;
        OnChange();
    }

    private void OnDestroy() {
        list.OnChange -= OnChange;
    }

    private void OnChange() {
        while(content.childCount > 0) {
            Transform t = content.GetChild(0);
            Destroy(t.gameObject);
            t.SetParent(null);
        }

        foreach (Ship s in list.Ships) {
            ShipInfoLine line = Instantiate(linePrefab,content);
            line.SetShip(s);            
        }
    }    
}
