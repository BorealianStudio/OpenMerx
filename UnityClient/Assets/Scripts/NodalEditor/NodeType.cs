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
