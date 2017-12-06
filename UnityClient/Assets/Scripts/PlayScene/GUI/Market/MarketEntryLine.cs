using UnityEngine;
using UnityEngine.UI;

public class MarketEntryLine : MonoBehaviour {

    [SerializeField] Text qte = null;
    [SerializeField] Text price = null;

    private MarketData _data = null;

    public void SetData(MarketData data) {
        _data = data;

        qte.text = _data.Qte.ToString();
        price.text = _data.Price.ToString();
    }
}
