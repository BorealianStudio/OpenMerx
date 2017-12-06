using UnityEngine;
using UnityEngine.EventSystems;

public class WindowResizeHandler : MonoBehaviour, IDragHandler {
    private RectTransform _rectTrans = null;
    private WindowContent _contentInfos = null;
    private Window _window = null;

    private void Awake() {
        _rectTrans = GetComponentInParent<Window>().GetComponent<RectTransform>();        
    }

    private void Start() {
        _contentInfos = _rectTrans.GetComponentInChildren<WindowContent>();
        _window = GetComponentInParent<Window>();
    }

    public void OnDrag(PointerEventData data) {

        float currentWidth = _rectTrans.sizeDelta.x;
        float newWidth = currentWidth + data.delta.x;
        if (null != _contentInfos && newWidth < (float)_contentInfos.minWidthPixel) {
            newWidth = (float)_contentInfos.minWidthPixel;
        }
        float currentHeight = _rectTrans.sizeDelta.y;
        float newHeight = currentHeight - data.delta.y;
        if (null != _contentInfos && newHeight < (float)_contentInfos.minHeightPixel) {
            newHeight = (float)_contentInfos.minHeightPixel;
        }
        newWidth = Mathf.Max(newWidth, Mathf.Max(100,_window.MinWidth));
        newHeight = Mathf.Max(newHeight, Mathf.Max(100, _window.MinHeight));

        _rectTrans.sizeDelta = new Vector2(newWidth,
                                           newHeight);
    }
}