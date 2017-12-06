using UnityEngine;
using UnityEngine.UI;

public class MarketResourceLine : MonoBehaviour {

    [SerializeField] Text _name = null;

    public delegate void OnButtonClic(MarketResourceLine line);
    public event OnButtonClic OnClic = delegate { };

    public ResourceInfos ResourceType { get; private set; }

    private void Start() {
        GetComponentInChildren<Button>().onClick.AddListener(() => { OnClic(this); });
    }

    public void SetResource(ResourceInfos type) {
        ResourceType = type;
        _name.text = type.Name;
    }
}
