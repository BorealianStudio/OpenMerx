using UnityEngine;
using UnityEngine.UI;

public class NodeDetails : MonoBehaviour {


    NodalEditor _editor = null;
    Node _currentNode = null;
    [SerializeField] Button _deleteButton = null;
    [SerializeField, Tooltip("Transform ou instancier la vue custom en fonction du type de node")]
    Transform CustomZone = null;

    [SerializeField] HangarSelectionNodeView hangarSelectionNodeViewPrefab = null;
        
    private void Start() {
        _editor = GetComponentInParent<NodalEditor>();
    }

    public void SetNode(Node node) {
        _currentNode = node;
        _deleteButton.interactable = node.NodePrefabInfos.visible;
        if(node.NodeInfos.type == "Hangar selector") {
            HangarSelectionNodeView view = Instantiate(hangarSelectionNodeViewPrefab, CustomZone);
            view.SetNode(node);
        }
    }

    public void SetCanDelete(bool canDelete) {
        _deleteButton.interactable = canDelete;
    }

    public void DeleteClic() {
        if(null != _currentNode && _currentNode.NodePrefabInfos.visible) {
            _editor.RemoveNode(_currentNode);
            _currentNode = null;
        }
    }
}
