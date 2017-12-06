using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class CorporationView : MonoBehaviour {

    [SerializeField] InputField corpName = null;
    [SerializeField] Button createButton = null;
    [SerializeField] Dropdown stations = null;

    private Corporation _corp = null;

    private void Awake() {
        LocalDataManager.instance.OnCharacterChange += OnCharChange;

        stations.options.Clear();
        foreach (Station s in LocalDataManager.instance.GetStations()) {
            stations.options.Add(new Dropdown.OptionData(s.Name));
        }
        stations.value = 0;
        stations.RefreshShownValue();
    }

    private void OnDestroy() {
        LocalDataManager.instance.OnCharacterChange -= OnCharChange;
        LocalDataManager.instance.OnCorporationChange -= OnCorpInfo;
    }

    public void SetCorpID(int id) {

        if (id == -1) {
            //on doit créer la corporation
            SetModeCreation();
            TutorialManager.RaiseEvent("CorpoNeedCreate");                        
        } else {
            SetModeAffichage();
            LocalDataManager.instance.OnCorporationChange += OnCorpInfo;
            _corp = LocalDataManager.instance.GetCorporationInfo(id);
            OnCorpInfo(_corp);
            Window w = GetComponentInParent<Window>();
            stations.value = _corp.Station - 1;
            stations.RefreshShownValue();
            w.SetLoading(!_corp.Loaded);                            
        }
    }
      
    public void OnCreateClic() {
        int stationID = stations.value + 1;
        WideDataManager.wideDataManager.SendRequest(new CreateCorpRequest(LocalDataManager.instance.LocalCharacterID,
                                                                          stationID,
                                                                          corpName.text));

        Window w = GetComponentInParent<Window>();
        w.SetLoading(true);
        StartCoroutine(WaitCorp());
    }

    private IEnumerator WaitCorp() {
        while(_corp == null || !_corp.Loaded) {
            yield return null;
        }
        Window w = GetComponentInParent<Window>();
        w.SetLoading(false);
    }

    private void SetModeCreation() {
        CheckOkSensitivity();
        corpName.interactable = true;
        stations.interactable = true;
        createButton.gameObject.SetActive(true);
    }  

    private void SetModeAffichage() {
        corpName.interactable = false;
        stations.interactable = false;
        createButton.gameObject.SetActive(false);
    }

    public void CheckOkSensitivity() {
        if (corpName.text.Length > 0) {
            createButton.interactable = true;
        } else {
            createButton.interactable = false;
        }
    }

    private void OnCorpInfo(Corporation c) {
        if (null == _corp || null == c || _corp.ID != c.ID)
            return;

        if ( c.Loaded && c.Station !=-1) {
            corpName.text = c.Name;
            stations.value = c.Station - 1;
            stations.RefreshShownValue();
            Window w = GetComponentInParent<Window>();            
            w.SetLoading(!_corp.Loaded);
        }
    }

    private void OnCharChange(Character c) {
        SetCorpID(c.Corp);
    }
}
