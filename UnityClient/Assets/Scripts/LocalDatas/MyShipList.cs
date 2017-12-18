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
