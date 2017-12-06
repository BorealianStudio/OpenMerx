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
