using System.Collections.Generic;

public class ShipMovementSystem : DataSystem {

    private Dictionary<int, Ship> _ships = new Dictionary<int, Ship>();
    private LocalDataManager _manager = null;

    public ShipMovementSystem(LocalDataManager manager) {
         _manager = manager;
        _manager.OnNewObject += OnNewObject;
    }
    
    private void OnNewObject(DataObject obj) {
        Ship ship = obj as Ship;
        if(null != ship) {
            if (!_ships.ContainsKey(ship.DataID)) {
                _ships.Add(ship.DataID, ship);
            }
        }
    }

    public override void Update() {
    }
}
