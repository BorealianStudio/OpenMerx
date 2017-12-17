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
/// C'est un component qui gere le modele nodale. C'est le parent de tout les nodes du modele
/// </summary>
public class ModelZone : MonoBehaviour, IDropHandler, IScrollHandler {
    
    [SerializeField] NodalEditor editor = null; 

    public void OnDrop(PointerEventData eventData) {

        if (null != editor._dragedPrefab && editor.CanDragNode()) {
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(editor.GetModelZone(), Input.mousePosition, null, out localPos);

            editor.CreateNode(localPos, editor._dragedPrefab);
        }
    }

    public void OnScroll(PointerEventData eventData) {
        //gestion du zoom sur la zone du modele
        if (eventData.scrollDelta.y > 0) {
            transform.localScale = new Vector3(transform.localScale.x + 0.1f,
                                                transform.localScale.y + 0.1f,
                                                transform.localScale.z + 0.1f);
        } else if (transform.localScale.x > 0.1f) {
            transform.localScale = new Vector3(transform.localScale.x - 0.1f,
                                                transform.localScale.y - 0.1f,
                                                transform.localScale.z - 0.1f);
        }
    }
}
