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

public class StationListView : MonoBehaviour {
    [SerializeField]
    Transform content = null;

    [SerializeField]
    StationListLineView linePrefab = null;

    [SerializeField]
    Toggle withHangartoggle = null;

    private Character _player = null;
    private Corporation _corp = null;

    private void Start() {
        withHangartoggle.onValueChanged.AddListener((b) => { UpdateVisu(); });

        _player = LocalDataManager.instance.GetCharacterInfo(LocalDataManager.instance.LocalCharacterID);
        LocalDataManager.instance.OnCharacterChange += OnCharChange;

        if (_player.Corp != -1) {
            _corp = LocalDataManager.instance.GetCorporationInfo(_player.Corp);
            LocalDataManager.instance.OnCorporationChange += OnCorpChange;
        }

        TutorialManager.RaiseEvent("StationListShow");

        UpdateVisu();
    }

    private void OnDestroy() {
        LocalDataManager.instance.OnCharacterChange -= OnCharChange;
        LocalDataManager.instance.OnCorporationChange -= OnCorpChange;
    }

    private void OnCharChange(Character c) {
        if (c.ID == _player.ID) {
            if (_player.Corp != -1 && _corp == null) {
                _corp = LocalDataManager.instance.GetCorporationInfo(_player.Corp);
            }

            UpdateVisu();
        }
    }

    private void OnCorpChange(Corporation c) {
        if(null != _corp && c.ID == _corp.ID) {
            UpdateVisu();
        }
    }

    private void UpdateVisu() {
        while(content.childCount > 0) {
            Destroy(content.GetChild(0).gameObject);
            content.GetChild(0).SetParent(null);
        }        
        
        foreach (Station s in LocalDataManager.instance.GetStations()) {
            if (!withHangartoggle.isOn || (_corp != null && s.Hangars.ContainsKey(_corp.ID))) {
                StationListLineView l = Instantiate<StationListLineView>(linePrefab);
                l.SetStation(s);
                l.transform.SetParent(content);
            }
        }
    }
    
}
