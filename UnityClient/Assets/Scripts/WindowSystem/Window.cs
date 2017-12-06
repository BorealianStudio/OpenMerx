using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Window : MonoBehaviour, IPointerClickHandler{

    #region events

    public delegate void WindowAction(Window w);

    public event WindowAction OnClose = delegate { };

    #endregion events

    [SerializeField, Tooltip("Bouont a utiliser pour fermer la fenetre")]
    private Button closeButton = null;

    [SerializeField, Tooltip("La zone qui contiendra les données")]
    private Transform content = null;

    [SerializeField, Tooltip("Le titre de la fenetre")]
    private Text title = null;

    [SerializeField, Tooltip("Le gameobject qui represente le chargement en cours")]
    private CanvasGroup loading = null;

    public int MinWidth { get; set; }
    public int MinHeight { get; set; }

    private RectTransform mRectTrans = null;
    private WindowSystem windowSystem = null;

    public Transform Content {
        get { return content; }
    }

    public string WindowType { get; set; }

    private void Awake() {
        if (null != closeButton) {
            closeButton.onClick.AddListener(CloseWindow);
        }
        mRectTrans = GetComponent<RectTransform>();
        mRectTrans.anchorMin = new Vector2(0.0f, 1.0f);
        mRectTrans.anchorMax = new Vector2(0.0f, 1.0f);
    }

    private void Start() {
        windowSystem = GetComponentInParent<WindowSystem>();
    }

    public void Hide() {
        CanvasGroup c = GetComponent<CanvasGroup>();
        c.alpha = 0.0f;
        c.interactable = false;
        c.blocksRaycasts = false;

    }

    public void Show() {
        CanvasGroup c = GetComponent<CanvasGroup>();
        c.alpha = 1.0f;
        c.interactable = true;
        c.blocksRaycasts = true;
    }

    public void SetPosition(Vector2 position) {
        mRectTrans.anchoredPosition = position;
    }

    public string Title {
        get { return (title == null ? "" : title.text); }
        set {
            if (title != null) {
                title.text = value;
            }
        }
    }

    private void CloseWindow() {
        OnClose(this);
        GameObject.Destroy(gameObject);
    }

    public void OnPointerClick(PointerEventData data) {
        if (null != windowSystem)
            windowSystem.BringToFront(this);
    }

    public void SetWidth(int width) {
        RectTransform r = GetComponent<RectTransform>();
        r.sizeDelta = new Vector2(width,r.sizeDelta.y);
    }

    public void SetHeight(int height) {
        RectTransform r = GetComponent<RectTransform>();
        r.sizeDelta = new Vector2(r.sizeDelta.x,height);
    }

    public void Clamp() {
        windowSystem.ClamWindow(this);
    }

    public void SetLoading(bool isLoading) {
        if(null != loading) {
            if (isLoading) {
                loading.gameObject.SetActive(true);
                loading.alpha = 1.0f;
                loading.interactable = true;
                loading.blocksRaycasts = true;
            } else {
                loading.gameObject.SetActive(false);
                loading.alpha = 0.0f;
                loading.interactable = false;
                loading.blocksRaycasts = false;
            }

        }
    }
}