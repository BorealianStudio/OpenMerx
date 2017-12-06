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
