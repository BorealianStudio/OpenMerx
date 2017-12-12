using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(RectTransform))]
public class Node : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler {

    RectTransform _rectTransform = null;

    [SerializeField] Text label = null;

    [SerializeField, Tooltip("Prefab pour une ligne de parametre")]
    NodeParamLine paramLinePrefab = null;

    public NodalEditor Editor { get; set; }
    private NodeInfos _nodeInfos = null;
    private NodePrefabInfos _nodePrefab = null;

    private bool _wasDraged = false;
    private Vector3 _dragDelta = Vector2.zero;

    private Dictionary<string, Button> _buttons = new Dictionary<string, Button>();
    private List<NodeLink> _links = new List<NodeLink>();


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
                NodeParamLine line = Instantiate(paramLinePrefab, this.transform);
                Button b = line.SetData(p);
                Image i = b.GetComponent<Image>();
                i.color = Editor.ColorFromType(p.Type);
                _buttons.Add(p.Name, b);
                b.onClick.AddListener(() => { Editor.ParamClic(this, p, _buttons[p.Name].transform.position); });
            }

            foreach (ParamInfos p in _nodePrefab.outputs) {
                NodeParamLine line = Instantiate(paramLinePrefab, this.transform);
                Button b = line.SetData(p);
                Image i = b.GetComponent<Image>();
                i.color = Editor.ColorFromType(p.Type);
                _buttons.Add(p.Name, b);
                b.onClick.AddListener(() => { Editor.ParamClic(this, p, _buttons[p.Name].transform.position); });
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }

    }       

    private void Start() {
        _rectTransform = transform.parent.GetComponent<RectTransform>();

        UpdateLinks();
    }

    public List<NodeLink> GetLinks() {
        return _links;
    }

    public void RemoveLink(NodeLink link) {
        _links.Remove(link);
    }

    public void RemoveLinks(List<NodeLink> links) {
        foreach (NodeLink l in links) {
            RemoveLink(l);
        }
    }

    public bool IsParamFree(string paramName) {
        int nbLink = 0;
        foreach(NodeLink l in _links) {
            if(l.infos.FromID == NodeInfos.id && l.infos.FromParam == paramName) {
                nbLink++;
            }
            if(l.infos.ToID == NodeInfos.id && l.infos.ToParam == paramName) {
                nbLink++;
            }
        }
        foreach(ParamInfos p in _nodePrefab.inputs) {
            if(p.Name == paramName) {
                switch (p.ConnectType) {
                    case ParamInfos.ParamConnectType.Param0_1: return nbLink < 1;
                    case ParamInfos.ParamConnectType.Param0_N: return true;
                    case ParamInfos.ParamConnectType.Param1_1: return nbLink < 1;
                    case ParamInfos.ParamConnectType.Param1_N: return true;
                }
            }
        }
        foreach(ParamInfos p in _nodePrefab.outputs) {
            if(p.Name == paramName) {
                switch (p.ConnectType) {
                    case ParamInfos.ParamConnectType.Param0_1: return nbLink < 1;
                    case ParamInfos.ParamConnectType.Param0_N: return true;
                    case ParamInfos.ParamConnectType.Param1_1: return nbLink < 1;
                    case ParamInfos.ParamConnectType.Param1_N: return true;
                }
            }
        }

        throw new Exception("not found param " + paramName);
    }

    public void SetLinkToParam(NodeLink link) {

        if (link.infos.FromID == NodeInfos.id ||
            link.infos.ToID == NodeInfos.id) {
            foreach(NodeLink l in _links) {
                if(l.infos.FromID == link.infos.FromID &&
                    l.infos.FromParam == link.infos.FromParam &&
                    l.infos.ToID == link.infos.ToID &&
                    l.infos.ToParam == link.infos.ToParam) {
                    return;
                }
            }                
            _links.Add(link);
        }
        UpdateLinks();
    }

    void Clamp() {
       
        NodeInfos.posX = Mathf.Clamp(NodeInfos.posX, -_rectTransform.rect.width * 0.5f, _rectTransform.rect.width * 0.5f);
        NodeInfos.posY = Mathf.Clamp(NodeInfos.posY, -_rectTransform.rect.height * 0.5f, _rectTransform.rect.height * 0.5f);

        transform.localPosition = new Vector2(NodeInfos.posX, NodeInfos.posY);
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (!_wasDraged && Editor.CanSelect()) {
            Editor.SetActiveNode(this);
        }
    }

    public void OnBeginDrag(PointerEventData eventData) {
        _dragDelta = Input.mousePosition - transform.position;
        _wasDraged = false;
    }

    public void OnDrag(PointerEventData eventData) {

        if (!Editor.CanDragNode())
            return;

        _wasDraged = true;

        Vector3 inputPosition = Input.mousePosition - _dragDelta;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Editor.GetModelZone(), inputPosition, null, out localPoint);
        NodeInfos.posX = localPoint.x;
        NodeInfos.posY = localPoint.y;        

        transform.position = inputPosition;

        UpdateLinks();

        Clamp();
    }

    public void OnEndDrag(PointerEventData eventData) {
        _wasDraged = false;
    }

    private void UpdateLinks() {

        foreach (NodeLink l in _links) {
            if(l.infos.FromID == NodeInfos.id) {
                l.SetInputPosition(GetParamAttachPoint(l.infos.FromParam));
            }
            if(l.infos.ToID == NodeInfos.id) {
                l.SetOutputPosition(GetParamAttachPoint(l.infos.ToParam));
            }
        }
    }

    private Vector2 GetParamAttachPoint(string paramName) {
        return _buttons[paramName].transform.position;
    }
}