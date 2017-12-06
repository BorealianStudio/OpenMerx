using System.Collections.Generic;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class MarketOrder : MonoBehaviour {
    [SerializeField] Text resourceName = null;
    [SerializeField] Button validateButton = null;
    [SerializeField] GameObject waiting = null;
    [SerializeField] InputField qte = null;
    [SerializeField] InputField price = null;
    [SerializeField] Dropdown Hangars = null;

    private long _reqID = 0;
    private ResourceInfos _resType = null;
    private List<Hangar> _hangars = new List<Hangar>();

    private int _max = 0;

    private void Awake() {
        Hangars.onValueChanged.AddListener(OnHangarSelectChange);
        UpdateVisu();

        qte.onValidateInput = OnQteValidate;
    }

    private void Start() {
        validateButton.onClick.AddListener(OnValidateClic);
    }

    public void SetPrice(int iprice) {
        price.text = iprice.ToString();
    }

    public void SetQte(int iQte) {
        qte.text = iQte.ToString();
    }

    public void SetResource(int type) {
        _resType = LocalDataManager.instance.ResourceDataManager.GetResourceInfos(type);
        UpdateVisu();
    }

    private void UpdateVisu() {
        StartCoroutine(UpdateVisuRoutine());
    }

    private IEnumerator UpdateVisuRoutine() {
        waiting.SetActive(true);

        Hangars.options.Clear();
        Hangars.RefreshShownValue();

        _hangars.Clear();

        if (_resType == null) {
            waiting.SetActive(false);
            yield break;
        } else {
            resourceName.text = _resType.Name;

            int localCorp = LocalDataManager.instance.LocalCharacter.Corp;

            foreach(Station s in LocalDataManager.instance.GetStations()) {
                if (s.Hangars.ContainsKey(localCorp)) {
                    Hangar h = LocalDataManager.instance.GetHangarInfo(s.Hangars[localCorp]);
                    while (!h.Loaded) {
                        yield return null;
                    }
                    foreach(int stackID in h.StacksIDs) {
                        ResourceStack r = LocalDataManager.instance.GetResourceStackInfo(stackID);
                        while (!r.Loaded) {
                            yield return null;
                        }
                        if(r.Type == _resType.Type) {
                            Hangars.options.Add(new Dropdown.OptionData(s.Name));
                            _hangars.Add(h);
                            break;
                        }
                    }
                }
            }
        }

        Hangars.RefreshShownValue();
        OnHangarSelectChange(Hangars.value);
        waiting.SetActive(false);
        yield break;
    }

    private void OnHangarSelectChange(int value) {

        if (value >= Hangars.options.Count) {
            _max = 0;
            SetQte(0);
            return;
        }

        Hangar h = _hangars[value];

        _max = 0;
        foreach (int stackID in h.StacksIDs) {
            ResourceStack r = LocalDataManager.instance.GetResourceStackInfo(stackID);
            if (r.Type == _resType.Type) {
                _max += r.Qte;
            }
        }
        SetQte(_max);
    }

    private void OnValidateClic() {
        waiting.SetActive(true);
        LocalDataManager.instance.OnRequestDone += OnRequest;
        Hangar h = _hangars[Hangars.value];
        _reqID = WideDataManager.Request(new MarketPlaceSellOrderRequest(_resType.Type, int.Parse(qte.text), int.Parse(price.text), h.ID));
        
    }

    private void OnRequest(Request r) {
        if (r.RequestID == _reqID) {
            _reqID = -1;
            LocalDataManager.instance.OnRequestDone -= OnRequest;
            waiting.SetActive(false);
            MarketPlaceSellOrderRequest m = r as MarketPlaceSellOrderRequest;
            if (m.Result == 0) {
                Window w = GetComponentInParent<Window>();
                Destroy(w.gameObject);
            } else {
                WindowSystem ws = FindObjectOfType<WindowSystem>();
                switch (m.Result) {
                    case -1: ws.ShowMessage("Not enough resource in hangar"); break;
                }                
            }
        }
    }

    private char OnQteValidate(string text, int charIndex, char addedChar) {
        int tmp = 0;

        string newString = text + addedChar;
        if (int.TryParse(newString, out tmp)) {
            if(tmp >= 0 && tmp <= _max)
                return addedChar;
        }

        return '\0';
    }
}
