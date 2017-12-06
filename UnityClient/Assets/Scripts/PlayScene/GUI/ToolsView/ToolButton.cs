using UnityEngine;
using UnityEngine.UI;

public class ToolButton : MonoBehaviour {

    public Button Button { get; private set; }

    [SerializeField] Text buttonTitle = null;

    private void Awake() {
        Button = GetComponent<Button>();
    }    

    public void SetBlinking(bool willBlink) {
    }    

    public void SetTitle(string title) {
        buttonTitle.text = title;
    }
}
