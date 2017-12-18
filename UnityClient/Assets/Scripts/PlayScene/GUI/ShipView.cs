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

using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class ShipView : MonoBehaviour {

    [SerializeField] Button infoButton = null;
    [SerializeField] Button planningButton = null;
    [SerializeField, Tooltip("Button pour afficher l'onglet de logs du vaisseau")]
    Button logsButton = null;

    [SerializeField] Transform infoZone = null;
    [SerializeField] Transform planningZone = null;
    [SerializeField, Tooltip("Zone a afficher quand on affiche les logs")]
    Transform logsZone = null;

    [SerializeField, Tooltip("Le gameObject a activé quand le vaisseau n'est pas dans une flotte")]
    GameObject noFleetWarning = null;

    private Ship _ship = null;

    private void Start() {

        infoButton.onClick.AddListener(() => { ShowInfos(); });
        planningButton.onClick.AddListener(() => { ShowPlanning(); });
        logsButton.onClick.AddListener(ShowLogs);
        ShowInfos();

        LocalDataManager.instance.OnShipChange += OnShipChange;

        TutorialManager.RaiseEvent("ShipView");
    }

    private void OnDestroy() {
        LocalDataManager.instance.OnShipChange -= OnShipChange;
    }

    public void SetShip(Ship ship) {
        Window window = GetComponentInParent<Window>();

        _ship = ship;

        OnShipChange(_ship);

        window.SetLoading(true);
        StartCoroutine(WaitLoad());
    }

    private IEnumerator WaitLoad() {
                
        //ici on doit s'assurer que le viasseau est charge 
        while (!_ship.Loaded) {
            yield return null;
        }

        if (_ship.Fleet > 0) {            
            Fleet f = LocalDataManager.instance.GetFleetInfo(_ship.Fleet);

            NodalEditor editor = planningZone.GetComponentInChildren<NodalEditor>();
            editor.Load(f.Data);
        }

        Window window = GetComponentInParent<Window>();
        window.SetLoading(false);
    }

    private void OnShipChange(Ship s) {
        if (null != _ship && _ship.ID == s.ID) {
            noFleetWarning.SetActive(_ship.Fleet < 1);
            logsZone.GetComponentInChildren<Text>().text = s.Logs;
        }
    }

    private void ShowInfos() {
        infoZone.gameObject.SetActive(true);
        planningZone.gameObject.SetActive(false);
        logsZone.gameObject.SetActive(false);
    }

    private void ShowPlanning() {
        infoZone.gameObject.SetActive(false);
        planningZone.gameObject.SetActive(true);
        logsZone.gameObject.SetActive(false);
    }

    private void ShowLogs() {
        infoZone.gameObject.SetActive(false);
        planningZone.gameObject.SetActive(false);
        logsZone.gameObject.SetActive(true);
    }
}
