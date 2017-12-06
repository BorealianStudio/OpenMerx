using UnityEngine;
using UnityEngine.UI;

public class WindowMessage : MonoBehaviour {

    [SerializeField] Button closeButton = null;
    [SerializeField] Text text = null; 

    public delegate void OnCloseAction();
    public event OnCloseAction OnClose = delegate { };

    private void Start() {
        if(null != closeButton) {
            closeButton.onClick.AddListener(() => OnClose());
        }
    }

    public void SetMessage(string message) {
        text.text = message;
    }
}
