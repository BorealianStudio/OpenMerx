using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowHeader : MonoBehaviour, IBeginDragHandler, IDragHandler {
    private RectTransform _RectTrans = null;
    private WindowSystem _windowSystem = null;


    private void Awake() {
        _RectTrans = GetComponentInParent<Window>().GetComponent<RectTransform>();
        _windowSystem = FindObjectOfType<WindowSystem>();
    }

    public void OnDrag(PointerEventData data) {
        _RectTrans.anchoredPosition += data.delta;
        Window w = GetComponentInParent<Window>();
        _windowSystem.BringToFront(w);
    }

    public void OnBeginDrag(PointerEventData eventData) {
        Window w = GetComponentInParent<Window>();
        _windowSystem.BringToFront(w);
    }
}