using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class StationListLineView : MonoBehaviour {

    [SerializeField] Text Name = null;
    [SerializeField] Image hasHangar = null;
    [SerializeField] Transform detailNoHangar = null;
    [SerializeField] Transform detailWithHangar = null;

    private Station _station = null;

    private void Start() {
        LocalDataManager.instance.OnStationChange += OnStationChange;
    }

    public void SetStation(Station s) {
        _station = s;        
        UpdateVisu();
    }

    private void OnDestroy() {
        LocalDataManager.instance.OnStationChange -= OnStationChange;
    }

    public void OnClic() {
        TutorialManager.RaiseEvent("stationLineShowDetailNoHangar");
        
        RectTransform r = GetComponent<RectTransform>();
        if (r.sizeDelta.y > 100.0f) {
            r.sizeDelta = new Vector2(r.sizeDelta.x, 100);
        } else {
            r.sizeDelta = new Vector2(r.sizeDelta.x, 200);
        }
    }

    public void OnBuyHangarClic() {
        int corpID = LocalDataManager.instance.LocalCharacter.Corp;
        if (corpID < 1)
            return;

        WideDataManager.Request(new BuyHangarRequest(_station.ID,corpID));
    }

    private void UpdateVisu() {
        if (_station.Loaded) {
            Name.text = _station.Name;
            Character c = LocalDataManager.instance.GetCharacterInfo(LocalDataManager.instance.LocalCharacterID);
            bool testHangar = _station.Hangars.ContainsKey(c.Corp);
            hasHangar.gameObject.SetActive(testHangar);
            if (testHangar) {
                LocalDataManager.instance.GetHangarInfo(_station.Hangars[c.Corp]);
                detailWithHangar.gameObject.SetActive(true);
                detailNoHangar.gameObject.SetActive(false);
            } else {
                detailWithHangar.gameObject.SetActive(false);
                detailNoHangar.gameObject.SetActive(true);
            }
        }
    }
    
    private void OnStationChange(Station s) {
        if(null != _station && s.ID == _station.ID)
            UpdateVisu();
    }

    public void OnHangarClic() {
        StartCoroutine(OnHangarClicWait());
    }

    private IEnumerator OnHangarClicWait() {
        Hangar h = LocalDataManager.instance.GetHangarInfo(_station.ID, LocalDataManager.instance.LocalCharacter.Corp);
        while (!h.Loaded) {
            yield return null;
        }
        WindowSystem ws = FindObjectOfType<WindowSystem>();
        PrefabManager pf = FindObjectOfType<PrefabManager>();

        HangarView view = Instantiate(pf.prefabHangarView);
        Window w = ws.NewWindow("HangarView", view.gameObject);
        Station s = LocalDataManager.instance.GetStations()[h.Station - 1];
        view.SetHangar(h);
        w.Title = s.Name;
        w.Show();
        yield break;
    }
}
