using UnityEngine;
using UnityEngine.UI;

public class NodeDetails : MonoBehaviour {


    NodalEditor _editor = null;
    Node _currentNode = null;
    NodeLink _currentLink = null;

    [SerializeField] Button _deleteButton = null;
    [SerializeField, Tooltip("Transform ou instancier la vue custom en fonction du type de node")]
    Transform CustomZone = null;
    [SerializeField, Tooltip("Trahsform ou on affiche l'info d'un link")]
    Transform linkInfos = null;

    [SerializeField] HangarSelectionNodeView hangarSelectionNodeViewPrefab = null;
        
    private void Start() {
        _editor = GetComponentInParent<NodalEditor>();        
    }

    public void SetNode(Node node) {
        _currentLink = null;
        _currentNode = node;

        linkInfos.gameObject.SetActive(false);
        CustomZone.gameObject.SetActive(true);

        _deleteButton.interactable = node.NodePrefabInfos.visible;

        Clear();

        if(node.NodeInfos.type == "Hangar selector") {
            HangarSelectionNodeView view = Instantiate(hangarSelectionNodeViewPrefab, CustomZone);
            view.SetNode(node);
        }
    }

    public void SetLink(NodeLink link) {        
        _currentNode = null;
        _currentLink = link;

        linkInfos.gameObject.SetActive(true);
        CustomZone.gameObject.SetActive(false);

        linkInfos.Find("EditSource").GetComponent<Button>().interactable = false;

        _deleteButton.interactable = true;
    }

    private void Clear() {
        while(CustomZone.childCount > 0) {
            Transform f = CustomZone.GetChild(0);
            f.SetParent(null);
            Destroy(f.gameObject);
        }
    }

    public void SetCanDelete(bool canDelete) {
        _deleteButton.interactable = canDelete;
    }

    public void EditTargetClic() {
        _editor.EditTargetLink(_currentLink);
    }

    public void EditSourceClic() {
        Debug.Log("source");
    }

    public void DeleteClic() {
        if(null != _currentNode && _currentNode.NodePrefabInfos.visible) {
            _editor.RemoveNode(_currentNode);
            _currentNode = null;
        } else if(null != _currentLink) {
            _editor.RemoveLink(_currentLink);
            _currentNode = null;            
        }
    }
}
