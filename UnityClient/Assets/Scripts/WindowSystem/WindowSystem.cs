using System.Collections.Generic;

using UnityEngine;

public class WindowSystem : MonoBehaviour {

    [SerializeField]
    private Window windowPrefab = null;

    [SerializeField]
    private Transform ModaleZone = null;

    [System.Serializable]
    private struct WindowPosition {

        public WindowPosition(float x, float y) {
            X = x; Y = y;
        }

        public float X { get; set; }
        public float Y { get; set; }
    }

    private Dictionary<string, WindowPosition> positions = new Dictionary<string, WindowPosition>();

    private void Start() {
        if (PlayerPrefs.HasKey("positions")) {
            string s = PlayerPrefs.GetString("positions");
            positions = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, WindowPosition>>(s);
        }
    }

    private void OnDestroy() {
        string s = Newtonsoft.Json.JsonConvert.SerializeObject(positions);
        PlayerPrefs.SetString("positions", s);
    }

    public void ShowMessage(string message) {
        ModaleZone.gameObject.SetActive(true);

        WindowMessage messageView = ModaleZone.GetComponentInChildren<WindowMessage>();
        messageView.SetMessage(message);
        messageView.OnClose += OnMessageClose;
    }

    public Window NewWindow(string name, GameObject data) {
        Window newWindow = Instantiate<Window>(windowPrefab);
        newWindow.Hide();
        newWindow.transform.SetParent(transform);
        newWindow.SetPosition(Vector2.zero);
        data.transform.SetParent(newWindow.Content);
        RectTransform r = data.GetComponent<RectTransform>();
        r.anchorMin = Vector2.zero;
        r.anchorMax = Vector2.one;
        r.offsetMin = Vector2.zero;
        r.offsetMax = Vector2.zero;

        newWindow.OnClose += OnWindowClose;
        newWindow.WindowType = name;

        if (positions.ContainsKey(name)) {
            r = newWindow.GetComponent<RectTransform>();
            r.anchoredPosition = new Vector2(positions[name].X, positions[name].Y);
        }

        ClamWindow(newWindow);

        return newWindow;
    }

    public void BringToFront(Window window) {
        window.transform.SetAsLastSibling();
    }

    private void OnMessageClose() {
        WindowMessage messageView = ModaleZone.GetComponentInChildren<WindowMessage>();
        messageView.OnClose -= OnMessageClose;
        ModaleZone.gameObject.SetActive(false);
    }

    private void OnWindowClose(Window w) {
        RectTransform r = w.GetComponent<RectTransform>();

        if (!positions.ContainsKey(w.WindowType))
            positions.Add(w.WindowType, new WindowPosition());
        positions[w.WindowType] = new WindowPosition(r.anchoredPosition.x, r.anchoredPosition.y);
    }

    public void ClamWindow(Window w) {
        if (null == w)
            return;

        Rect rect = GetComponent<RectTransform>().rect;
        Rect myRect = w.GetComponent<RectTransform>().rect;

        //clamp bottom
        float minY = -rect.height * 0.5f;
        if (w.transform.localPosition.y - myRect.height < minY) {
            w.transform.localPosition = new Vector2(w.transform.localPosition.x, myRect.height + minY);
        }

        //clamp Left
        if (w.transform.localPosition.x  < -rect.width * 0.5f) {
            w.transform.localPosition = new Vector2(-rect.width * 0.5f, w.transform.localPosition.y);
        }

        //clamp right
        if (w.transform.localPosition.x + myRect.width > rect.width * 0.5f) {
            w.transform.localPosition = new Vector2(rect.width * 0.5f - myRect.width, w.transform.localPosition.y);
        }

        //clamp top
        if (w.transform.localPosition.y > rect.height * 0.5f) {
            w.transform.localPosition = new Vector2(w.transform.localPosition.x, rect.height * 0.5f);
        }

    }
}