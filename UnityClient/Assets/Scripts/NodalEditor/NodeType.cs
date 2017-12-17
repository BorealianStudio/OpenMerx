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

/// <summary>
/// c'est un prefab qui est utilisé pour instancier des node dans le modele
/// </summary>
public class NodeType : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    Vector2 _from;              //position ou il etait au debut du drag
    NodalEditor _editor = null; //l'editeur nodal auquel il appartient
    public NodePrefabInfos infos = null;

    private void Start() {
        _editor = GetComponentInParent<NodalEditor>();
    }

    public void OnDrag(PointerEventData eventData) {
        if (!_editor.CanDragNode())
            return;

        transform.position = Input.mousePosition;
    }

    public void OnBeginDrag(PointerEventData eventData) {

        _editor._dragedPrefab = this;
        _from = transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData) {
        _editor._dragedPrefab = null;
        transform.position = _from;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
