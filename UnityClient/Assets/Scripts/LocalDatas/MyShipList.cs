using System.Collections.Generic;

/// <summary>
/// c'est la liste de tout les vaisseaux du joueur local
/// </summary>
public class MyShipList  {

    private LocalDataManager _manager = null;
    private Dictionary<int, Ship> _ships = new Dictionary<int, Ship>();
    private int _corpID = -1;

    #region events
    public event EventHolder.BasicCB OnChange = delegate { };
    #endregion

    public MyShipList(LocalDataManager manager) {
        manager.OnShipChange += OnNewShip;
        SetManager(manager);
    }

    public List<Ship> Ships {
        get { return new List<Ship>(_ships.Values); } }

    public void SetLocalCorp(int corpID) {
        _corpID = corpID;
        WideDataManager.wideDataManager.SendRequest(new LoadMyShipsRequest(corpID));
    }

    private void SetManager(LocalDataManager manager) {
        if (null != _manager) {
            _manager.OnNewShip -= OnNewShip;
            _manager.OnRemoveShip-= OnRemoveShip;
        }
        _manager = manager;
        if (null != _manager) {
            _manager.OnNewShip += OnNewShip;
            _manager.OnRemoveShip += OnRemoveShip;
        }
    }

    private void OnNewShip(Ship s) {

        if(_ships.ContainsKey(s.ID) && s.Corp != _corpID) {
            _ships.Remove(s.ID);
            OnChange();
        }

        if(s.Corp != -1 &&  s.Corp == _corpID && !_ships.ContainsKey(s.ID)) {
            _ships.Add(s.ID, s);
            OnChange();
        }
    }

    private void OnRemoveShip(Ship s) {
        if(_ships.ContainsKey(s.ID)) {
            _ships.Remove(s.ID);
            OnChange();
        }
    }    
}
