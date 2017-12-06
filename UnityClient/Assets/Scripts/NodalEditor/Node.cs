using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(RectTransform))]
public class Node : MonoBehaviour, IDragHandler, IBeginDragHandler,  IEndDragHandler, IPointerClickHandler {

    RectTransform _rectTransform = null;

    [SerializeField] Text label = null;
    [SerializeField] GameObject paramButtonPrefab = null;
    [SerializeField] Transform leftParams = null;
    [SerializeField] Transform rightParams = null;

    public NodalEditor Editor { get; set; }
    private NodeInfos _nodeInfos = null;
    private NodePrefabInfos _nodePrefab = null;
    private bool _wasDraged = false;    

    private struct ParamLinkDetail {
        public NodeLink link;
        public Button button;
        public bool isFrom;// true si ce param est le debug du link, faux sinon
    }

    private Dictionary<string, Button> _buttons = new Dictionary<string, Button>();
    private Dictionary<string, ParamLinkDetail> _links = new Dictionary<string, ParamLinkDetail>();


    public NodeInfos NodeInfos {
        get {
            return _nodeInfos;
        }
        set {
            _nodeInfos = value;
            transform.localPosition = new Vector2(value.posX, value.posY);
        }
    }
    public NodePrefabInfos NodePrefabInfos {
        get {
            return _nodePrefab;
        }
        set {
            _nodePrefab = value;
            label.text = _nodePrefab.nodeName;

            _buttons.Clear();

            foreach (ParamInfos p in _nodePrefab.inputs) {
                GameObject o = Instantiate<GameObject>(paramButtonPrefab, leftParams);
                Button b = o.GetComponent<Button>();
                Image i = o.GetComponent<Image>();
                i.color = Editor.ColorFromType(p.Type);
                _buttons.Add(p.Name, b);
                b.onClick.AddListener(() => { Editor.ParamClic(this, p, _buttons[p.Name].transform.position); });
            }

            foreach (ParamInfos p in _nodePrefab.outputs) {
                GameObject o = Instantiate<GameObject>(paramButtonPrefab, rightParams);
                Button b = o.GetComponent<Button>();
                Image i = o.GetComponent<Image>();
                i.color = Editor.ColorFromType(p.Type);
                _buttons.Add(p.Name, b);
                b.onClick.AddListener(() => { Editor.ParamClic(this, p, _buttons[p.Name].transform.position); });
            }
        }

    }       

    private void Start() {
        _rectTransform = transform.parent.GetComponent<RectTransform>();
    }

    public Vector2 GetParamAttachPoint(ParamInfos p) {
        return _buttons[p.Name].transform.position;
    }

    public List<NodeLink> GetLinks() {
        List<NodeLink> result = new List<NodeLink>();

        foreach(ParamLinkDetail p in _links.Values) {
            result.Add(p.link);
        }

        return result;
    }
    public void RemoveLink(List<NodeLink> links) {

        List<string> toRemove = new List<string>();
        foreach (NodeLink l in links) {
            foreach (string s in _links.Keys) {
                if (_links[s].link == l) {
                    toRemove.Add(s);
                }
            }
        }
        foreach(string s in toRemove) {
            _links.Remove(s);
        }
    }
    public ParamInfos GetParamInfo(string name) {
        foreach(ParamInfos p in NodePrefabInfos.inputs) {
            if (p.Name == name)
                return p;
        }
        foreach (ParamInfos p in NodePrefabInfos.outputs) {
            if (p.Name == name)
                return p;
        }
        return null;
    }

    public bool IsParamFree(string paramName) {
        return !_links.ContainsKey(paramName);
    }

    public void ClearParam(string paramName) {

    }

    public void SetLinkToParam(NodeLink link, Node fromNode, string fromParamName, Node toNode, string toParamName) {

        string myParam = "";
        if (fromNode == this)
            myParam = fromParamName;
        else if (toNode == this)
            myParam = toParamName;
        else
            throw new Exception("SetLinkToParam d'un autre noeud?");

        if (_links.ContainsKey(myParam))
            _links.Remove(myParam);

        ParamLinkDetail d = new ParamLinkDetail();
        d.isFrom = fromNode == this;
        d.link = link;
        d.button = _buttons[myParam];
        _links.Add(myParam, d);

        if (d.isFrom) {
            link.SetInputPosition(d.button.transform.position);
        } else {
            link.SetOutputPosition(d.button.transform.position);
        }
    }

    void Clamp() {
       
        NodeInfos.posX = Mathf.Clamp(NodeInfos.posX, -_rectTransform.rect.width * 0.5f, _rectTransform.rect.width * 0.5f);
        NodeInfos.posY = Mathf.Clamp(NodeInfos.posY, -_rectTransform.rect.height * 0.5f, _rectTransform.rect.height * 0.5f);

        transform.localPosition = new Vector2(NodeInfos.posX, NodeInfos.posY);
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (!_wasDraged) {
            Editor.SetActiveNode(this);
        }
    }

    public void OnBeginDrag(PointerEventData eventData) {
        _wasDraged = false;
    }

    public void OnDrag(PointerEventData eventData) {

        if (!Editor.CanDragNode())
            return;

        _wasDraged = true;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Editor.GetModelZone(), Input.mousePosition, null, out localPoint);
        NodeInfos.posX = localPoint.x;
        NodeInfos.posY = localPoint.y;

        foreach (ParamLinkDetail p in _links.Values) {
            if (p.isFrom) {
                p.link.SetInputPosition(p.button.transform.position);
            } else {
                p.link.SetOutputPosition(p.button.transform.position);
            }
        }

        transform.position = Input.mousePosition;

        Clamp();
    }

    public void OnEndDrag(PointerEventData eventData) {
        _wasDraged = false;
    }
}