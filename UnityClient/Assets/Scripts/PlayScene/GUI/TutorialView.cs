using UnityEngine;
using UnityEngine.UI;

public class TutorialView : MonoBehaviour {

    public delegate void ButtonClic();
    public event ButtonClic OnButton1Clic = delegate { };
    public event ButtonClic OnButton2Clic = delegate { };

    [SerializeField]
    Text TextZone = null;
    
    [SerializeField]
    Button button1 = null;

    [SerializeField]
    Button button2 = null;

    private void Awake() {
        button1.onClick.AddListener(() => { OnButton1Clic(); });
        button2.onClick.AddListener(() => { OnButton2Clic(); });
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void SetButton1View(bool isVisible) {
        button1.gameObject.SetActive(isVisible);
    }

    public void SetTextButton1(string text) {
        button1.GetComponentInChildren<Text>().text = text;
    }

    public void SetButton2View(bool isVisible) {
        button2.gameObject.SetActive(isVisible);
    }

    public void SetTextButton2(string text) {
        button2.GetComponentInChildren<Text>().text = text;
    }

    public void SetText(string text) {
        TextZone.text = text;
    }
}
