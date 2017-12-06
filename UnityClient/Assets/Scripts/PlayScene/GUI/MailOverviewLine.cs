using UnityEngine;
using UnityEngine.UI;

public class MailOverviewLine : MonoBehaviour {

    public delegate void OnClicAction(Mail m);
    public event OnClicAction OnButtonClic = delegate { };

    [SerializeField] Text title = null;

    public Mail Mail { get; private set; }

    private void Start() {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => OnButtonClic(Mail));
    }
    public void SetMail(Mail m) {
        Mail = m;
        title.text = m.Title;
    }    
}
