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

using UnityEngine;
using UnityEngine.UI;

public class HangarSelectionNodeView : MonoBehaviour {

    [SerializeField, Tooltip("Dropdown contenant les hangars disponibles")]
    Dropdown hangars = null;
    
    private Node _node = null;

    private List<Hangar> _hangarList = new List<Hangar>();
     
    private void Awake() {        
        hangars.options.Clear();

        int localCorp = LocalDataManager.instance.LocalCorporation.ID;
        foreach (Station s in LocalDataManager.instance.GetStations()) {
            if (s.Hangars.ContainsKey(localCorp)) {
                _hangarList.Add(LocalDataManager.instance.GetHangarInfo(s.Hangars[localCorp]));
                hangars.options.Add(new Dropdown.OptionData(s.Name));
            }
        }
        hangars.value = 0;
        hangars.RefreshShownValue();

        hangars.onValueChanged.AddListener(OnHangarChange);
    }

    public void SetNode(Node node) {
        _node = node;
        int hangarID = node.NodeInfos.nodeParams.GetInt("HangarID", -1);
        if(hangarID > 0) {
            for(int i = 0; i < _hangarList.Count; i++) {
                if(_hangarList[i].ID == hangarID) {
                    hangars.value = i;
                    hangars.RefreshShownValue();
                    break;
                }
            }
        } else {
            OnHangarChange(0);
        }
    }

    private void OnHangarChange(int value) {

        int hangarID = _hangarList[value].ID;
        _node.NodeInfos.nodeParams.Set("HangarID", hangarID);
        
    }
}
