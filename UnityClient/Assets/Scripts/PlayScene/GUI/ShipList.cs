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
