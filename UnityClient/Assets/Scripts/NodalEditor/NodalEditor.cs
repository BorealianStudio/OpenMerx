using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Newtonsoft.Json;

/// <summary>
/// c'est un editeur nodal. Il permet de creer des nodes, de les deplacer, des les connecter.
/// Cet editeur peut etre sauvé ou chargé a partie de donnes sauvées
/// </summary>
public class NodalEditor : MonoBehaviour, IPointerClickHandler {

    [SerializeField, Tooltip("Le gameObject qui contiendra les nodes du modele")]
    ModelZone modelZone = null;    
    [SerializeField, Tooltip("C'est le transform qui contiendra les prefab de node")]
    Transform prefabZone = null;
    [SerializeField, Tooltip("L'objet qui sera utilisé pour afficher les details du node selectionne")]
    NodeDetails detailZone = null;
    [SerializeField, Tooltip("Le prefab a utiliser pour instancier un Node")]
    Node nodePrefab = null;
    [SerializeField, Tooltip("Le prefab a utiliser pour instancer un lien entre nNode")]
    NodeLink nodeLinkPrefab = null;
    [SerializeField, Tooltip("Le prefab a utiliser pour instancier un prefab de Node")]
    NodeType nodeTypePrefab = null;
    [SerializeField, Tooltip("indique si l'utilisateur peut ou pas modifier le modele")]
    bool readOnly = false;


    Dictionary<int, Node> _nodes = new Dictionary<int, Node>(); //tous les node instancies

    //edition
    Node _currentFromNode = null;                               //le node source du lien en cours de creation
    ParamInfos _currentFromParam = null;                        //le param du _currentFromNode qu'on essais de connecter
    NodeLink _currentLink = null;                               //le lien en cours d'edition

    //drag
    public NodeType _dragedPrefab = null;                       //le Node en train d'etre deplace

    //la liste de tout les prefabs disponible pour le modele
    Dictionary<string, NodePrefabInfos> prefabs = new Dictionary<string, NodePrefabInfos>();
        
    //une structure de données serializable pour sauver et charger
    public class SaveStruct {
        public Dictionary<int, NodeInfos> nodes = new Dictionary<int, NodeInfos>();
        public List<LinkInfo> links = new List<LinkInfo>();
    }

    private void Awake() {

        //ici on crée les prefabs par defauts, a terme ca doit sortir
        NodePrefabInfos prefabStart = new NodePrefabInfos();
        prefabStart.nodeName = "Start";
        prefabStart.outputs.Add(new ParamInfos(ParamInfos.ParamType.ParamFlow, ParamInfos.ParamDirection.ParamOut, "StartOutput"));
        prefabStart.visible = false;
        prefabs.Add(prefabStart.nodeName, prefabStart);

        NodePrefabInfos prefabEnd = new NodePrefabInfos();
        prefabEnd.nodeName = "End";
        prefabEnd.inputs.Add(new ParamInfos(ParamInfos.ParamType.ParamFlow, ParamInfos.ParamDirection.ParamIn, "EndInput"));
        prefabs.Add(prefabEnd.nodeName, prefabEnd);

        NodePrefabInfos prefabStationSelector = new NodePrefabInfos();
        prefabStationSelector.nodeName = "Hangar selector";
        prefabStationSelector.outputs.Add(new ParamInfos(ParamInfos.ParamType.ParamHangar, ParamInfos.ParamDirection.ParamOut, "station"));
        prefabs.Add(prefabStationSelector.nodeName, prefabStationSelector);

        NodePrefabInfos prefabExplore = new NodePrefabInfos();
        prefabExplore.nodeName = "Explore";
        prefabExplore.inputs.Add(new ParamInfos(ParamInfos.ParamType.ParamFlow, ParamInfos.ParamDirection.ParamIn, "ExploreInput"));
        prefabExplore.outputs.Add(new ParamInfos(ParamInfos.ParamType.ParamFlow, ParamInfos.ParamDirection.ParamOut, "ExploreOutput"));
        prefabs.Add(prefabExplore.nodeName, prefabExplore);

        NodePrefabInfos prefabMoveTo = new NodePrefabInfos();
        prefabMoveTo.nodeName = "Move To";
        prefabMoveTo.inputs.Add(new ParamInfos(ParamInfos.ParamType.ParamFlow, ParamInfos.ParamDirection.ParamIn, "MoveInput"));
        prefabMoveTo.inputs.Add(new ParamInfos(ParamInfos.ParamType.ParamHangar, ParamInfos.ParamDirection.ParamIn, "Station"));
        prefabMoveTo.outputs.Add(new ParamInfos(ParamInfos.ParamType.ParamFlow, ParamInfos.ParamDirection.ParamOut, "MoveOutput"));
        prefabs.Add(prefabMoveTo.nodeName, prefabMoveTo);


        if (readOnly) {
            RectTransform r = prefabZone.GetComponent<RectTransform>();
            r.sizeDelta = Vector2.zero;
            r = GetComponentInChildren<ScrollRect>().GetComponentInChildren<RectTransform>();
            r.anchoredPosition = Vector2.zero;
            r.sizeDelta = Vector2.zero;
        } else {
            foreach (NodePrefabInfos i in prefabs.Values) {
                CreateNodePrefab(i);
            }
            CreateNode(Vector2.zero, prefabs["Start"]);
        }
        detailZone.gameObject.SetActive(false);
    }

    private void Update() {
        if(null != _currentLink) {
            _currentLink.SetOutputPosition(Input.mousePosition);
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if(null != _currentLink) {
                Destroy(_currentLink.gameObject);
                _currentLink = null;
                _currentFromParam = null;
                _currentFromNode = null;
            }
        }
    }

    public RectTransform GetModelZone() {
        return modelZone.GetComponent<RectTransform>();
    }

    public void ToSave() {
        string s = Save();
        PlayerPrefs.SetString("test", s);
    }

    public void ToLoad() {
        string s = PlayerPrefs.GetString("test");
        Load(s);
    }

    public bool CanDragNode() {
        return !readOnly && _currentFromNode == null;
    }

    public string Save() {

        SaveStruct saveStruct = new SaveStruct();

        foreach(Node n in modelZone.GetComponentsInChildren<Node>()) {
            saveStruct.nodes.Add(n.NodeInfos.id,n.NodeInfos);
        }
        foreach(NodeLink l in modelZone.GetComponentsInChildren<NodeLink>()) {
            saveStruct.links.Add(l.infos);
        }

        string result = JsonConvert.SerializeObject(saveStruct);

        return result;
    }

    public void Load(string data) {
        //clean
        Clear();
                
        while (prefabZone.transform.childCount > 0) {
            Transform t = prefabZone.transform.GetChild(0);
            t.SetParent(null);
            Destroy(t.gameObject);
        }

        if (!readOnly) {
            //creer les prefab
            foreach (NodePrefabInfos i in prefabs.Values) {
                CreateNodePrefab(i);
            }
        }

        //creer les Nodes
        SaveStruct loadStruct = JsonConvert.DeserializeObject<SaveStruct>(data);
        foreach (NodeInfos n in loadStruct.nodes.Values) {
            Node node = Instantiate(nodePrefab);
            node.transform.SetParent(modelZone.transform);
            node.Editor = this;
            node.NodeInfos = n;
            node.transform.localScale = Vector2.one;
            node.NodePrefabInfos = prefabs[n.type];
            node.name = n.type;
            node.transform.localPosition = new Vector3(n.posX, n.posY);
            _nodes.Add(n.id, node);
        }

        //creer les liens entre les nodes
        foreach(LinkInfo l in loadStruct.links) { 
            Node targetNode = _nodes[l.ToID];
            Node fromNode = _nodes[l.FromID];

            NodeLink link = Instantiate(nodeLinkPrefab, modelZone.transform);
            link.Editor = this;
            link.SetColor(ColorFromType(l.LinkType));
            link.transform.SetAsFirstSibling();
            link.infos = l;

            fromNode.SetLinkToParam(link, fromNode, l.FromParam, targetNode, l.ToParam);
            targetNode.SetLinkToParam(link, fromNode, l.FromParam, targetNode, l.ToParam);
        }
    }

    public void Clear() {
        while (modelZone.transform.childCount > 0) {
            Transform t = modelZone.transform.GetChild(0);
            t.SetParent(null);
            Destroy(t.gameObject);
        }

        _nodes.Clear();
        SetActiveNode(null);
    }

    public void Init() {
        Clear();
        CreateNode(Vector2.zero, prefabs["Start"]);
    }

    private void CreateNodePrefab(NodePrefabInfos infos) {
        if (infos.visible) {
            NodeType nodeType = Instantiate<NodeType>(nodeTypePrefab, prefabZone);
            nodeType.GetComponentInChildren<Text>().text = infos.nodeName;
            nodeType.infos = infos;
            nodeType.name = infos.nodeName;
        }
    }

    public void CreateNode(Vector2 position, NodeType nodeType) {
        CreateNode(position, nodeType.infos);
    }

    public void CreateNode(Vector2 position, NodePrefabInfos infos) {
        int index = 0;
        while (_nodes.ContainsKey(index)) {
            index++;
        }
        Node n = Instantiate(nodePrefab);
        n.Editor = this;
        n.transform.SetParent(modelZone.transform);
        n.transform.localPosition = position;
        n.transform.localScale = Vector2.one;

        NodeInfos i = new NodeInfos();

        i.id = index;
        i.posX = position.x;
        i.posY = position.y;
        n.NodeInfos = i;
        n.NodePrefabInfos = infos;
        n.NodeInfos.type = infos.nodeName;

        _nodes.Add(i.id, n);
    }

    public void RemoveNode(Node node) {
        if (null == node)
            return;

        List<NodeLink> toDelete = node.GetLinks();

        foreach(Node n in _nodes.Values) {
            n.RemoveLink(toDelete);
        }

        foreach(NodeLink n in toDelete) {
            Destroy(n);
            n.transform.SetParent(null);
        }

        if (_nodes.ContainsKey(node.NodeInfos.id)) {
            SetActiveNode(null);
            int index = node.NodeInfos.id;
            node.transform.SetParent(null);
            Destroy(node.gameObject);
            _nodes.Remove(index);
        }
    }

    public void SetActiveNode(Node node) {
        if (null == node) {
            detailZone.gameObject.SetActive(false);            
        } else {
            detailZone.gameObject.SetActive(true);
            detailZone.SetNode(node);
        }
        detailZone.SetCanDelete(!readOnly);
    }

    public void ParamClic(Node node, ParamInfos infos, Vector2 attachPoint) {
        switch (infos.Direction) {
            case ParamInfos.ParamDirection.ParamOut:
                if (_currentFromNode == null && node.IsParamFree(infos.Name)) {
                    _currentFromNode = node;
                    _currentFromParam = infos;
                    _currentLink = Instantiate(nodeLinkPrefab, modelZone.transform);
                    _currentLink.Editor = this;
                    _currentLink.transform.SetAsFirstSibling();
                    _currentLink.SetInputPosition(attachPoint);
                    _currentLink.SetOutputPosition(Input.mousePosition);
                    _currentLink.SetColor(ColorFromType(infos.Type));
                }
                break;
            case ParamInfos.ParamDirection.ParamIn:
                if (_currentLink != null && node != _currentFromNode &&
                    node.IsParamFree(infos.Name) && 
                    _currentFromParam.Type == infos.Type) {

                    _currentFromNode.SetLinkToParam(_currentLink, _currentFromNode, _currentFromParam.Name, node, infos.Name);
                    node.SetLinkToParam(_currentLink, _currentFromNode, _currentFromParam.Name, node, infos.Name);
    
                    LinkInfo linkInfo = new LinkInfo(_currentFromNode.NodeInfos.id, _currentFromParam.Name,
                                                    node.NodeInfos.id, infos.Name, infos.Type);
                    _currentLink.infos = linkInfo;
                    _currentFromNode = null;
                    _currentLink = null;
                    _currentFromParam = null;
                }
                break;
        }
    }

    public Color ColorFromType(ParamInfos.ParamType type) {
        switch (type) {
            case ParamInfos.ParamType.ParamFlow: return Color.white;
            case ParamInfos.ParamType.ParamShip: return Color.red;
            case ParamInfos.ParamType.ParamHangar: return Color.green;
        }
        return Color.white;
    }

    public void OnPointerClick(PointerEventData eventData) {
        SetActiveNode(null);
    }
}
