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
