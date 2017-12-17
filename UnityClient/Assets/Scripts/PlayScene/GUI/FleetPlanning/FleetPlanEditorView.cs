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

public class FleetPlanEditorView : MonoBehaviour {

    [SerializeField, Tooltip("Liste des modele de plans du joueur")]
    Dropdown plans = null;

    [SerializeField, Tooltip("Button pour nettoyer l'editeur")]
    Button clearButton = null;

    [SerializeField, Tooltip("Bouton pour lancer la flotte")]
    Button launchButton = null;

    NodalEditor _editor = null;

    List<FlightPlan> _plans = null;

    List<int> _shipIDs = null;
    int _hangarID = 0;

    private void Start() {
        _editor = GetComponentInChildren<NodalEditor>();
        clearButton.onClick.AddListener(_editor.Init);
        launchButton.onClick.AddListener(OnLauch);
        plans.onValueChanged.AddListener(PlanSelect);

        plans.options.Clear();
        _plans = LocalDataManager.instance.GetFlightPlans(LocalDataManager.instance.LocalCorporation);
        plans.options.Add(new Dropdown.OptionData("Custom"));
        foreach(FlightPlan f in _plans) { 
            plans.options.Add(new Dropdown.OptionData(f.Name));
        }

        plans.value = 0;
        plans.RefreshShownValue();
    }

    public void SetData(List<int> ships, int hangarID) {
        _shipIDs = ships;
        _hangarID = hangarID;
    }

    private void PlanSelect(int value) {
        if (value == 0) {            
            _editor.Init();
        } else {
            _editor.Load(_plans[value - 1].Data);
        }
    }

    private void OnLauch() {
        WideDataManager.Request(new FleetCreateRequest(_hangarID, _shipIDs, _editor.Save()));
        Destroy(GetComponentInParent<Window>().gameObject);
    }
}
   