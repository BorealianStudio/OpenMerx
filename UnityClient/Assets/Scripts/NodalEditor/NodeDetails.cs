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
