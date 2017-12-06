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
