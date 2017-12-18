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

public class Header : MonoBehaviour {
      
    [SerializeField] Text ICU = null;

    [SerializeField] Text Time = null;

    private Character _char = null;
    LocalDataManager dataManager = null;

    private void Start() {
        dataManager = LocalDataManager.instance;

        _char = dataManager.GetCharacterInfo(dataManager.LocalCharacterID);
        LocalDataManager.instance.OnCorporationChange += OnCorpChange;
        dataManager.OnTimeChange += OnTimeChange;
    }

    private void OnDestroy() {
        dataManager.OnTimeChange -= OnTimeChange;
        LocalDataManager.instance.OnCorporationChange -= OnCorpChange;
    }

    private void OnTimeChange() {
        Time.text = "frame : " + dataManager.Frame;
    }

    private void OnCorpChange(Corporation c) {
        if(null != _char && _char.Corp == c.ID && c.Loaded) { 
            ICU.text = c.ICU.ToString() + " ICU";
        }        
    }
}
