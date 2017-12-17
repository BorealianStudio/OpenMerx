// MIT License

// Copyright(c) 2017 Andre Plourde
// part of https://github.com/BorealianStudio/OpenMerx

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.


// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

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