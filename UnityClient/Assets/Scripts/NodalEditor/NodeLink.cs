using UnityEngine;
using UnityEngine.UI.Extensions;

public class NodeLink : MonoBehaviour {

    UILineRenderer _line = null;
    RectTransform _rectTrasnform;

    Vector2 from = Vector2.zero;
    Vector2 to = Vector2.zero;

    public NodalEditor Editor { get; set; }

    public LinkInfo infos { get; set; }

    private void Awake() {
        _line = GetComponent<UILineRenderer>();
        _line.BezierMode = UILineRenderer.BezierType.Quick;
        _line.color = Color.red;
        _rectTrasnform = GetComponent<RectTransform>();
        _rectTrasnform.pivot = Vector2.zero;

        UpdateView();
    }

    public void SetColor(Color color) {
        _line.color = color;
    }

    public void SetInputPosition(Vector2 inPos) {
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Editor.GetModelZone(), inPos, null, out localPos);
        from = localPos;

        UpdateView();
    }

    public void SetOutputPosition(Vector2 outPos) {

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Editor.GetModelZone(), outPos, null, out localPos);
        to = localPos;

        UpdateView();
    }

    void UpdateView() {

        if (from.x < to.x) { 
            if (to.y > from.y) {
                float distX = to.x - from.x;
                float distY = to.y - from.y;

                _rectTrasnform.sizeDelta = new Vector2(Mathf.Abs(distX), Mathf.Abs(distY));

                Vector2 basGauche = new Vector2(from.x, from.y);

                _rectTrasnform.localPosition = basGauche;

                Vector2[] points = new Vector2[4];
                points[0] = new Vector2(0.0f, 0.0f);
                points[1] = new Vector2(_rectTrasnform.rect.width * 0.5f, 10.0f);
                points[2] = new Vector2(_rectTrasnform.rect.width * 0.5f, _rectTrasnform.rect.height);
                points[3] = new Vector2(_rectTrasnform.rect.width, _rectTrasnform.rect.height);
                _line.Points = points;
            } else {
                float distX = to.x - from.x;
                float distY = to.y - from.y;

                _rectTrasnform.sizeDelta = new Vector2(Mathf.Abs(distX), Mathf.Abs(distY));

                Vector2 basGauche = new Vector2(from.x, to.y);

                _rectTrasnform.localPosition = basGauche;

                Vector2[] points = new Vector2[4];
                points[0] = new Vector2(0.0f, _rectTrasnform.rect.height);
                points[1] = new Vector2(_rectTrasnform.rect.width * 0.5f, _rectTrasnform.rect.height);
                points[2] = new Vector2(_rectTrasnform.rect.width * 0.5f, 0.0f);
                points[3] = new Vector2(_rectTrasnform.rect.width, 0.0f);
                _line.Points = points;
            }
        } else {
            if (to.y > from.y) {
                float distX = from.x - to.x;
                float distY = from.y - to.y;

                _rectTrasnform.sizeDelta = new Vector2(Mathf.Abs(distX), Mathf.Abs(distY));

                Vector2 basGauche = new Vector2(to.x, from.y);

                _rectTrasnform.localPosition = basGauche;

                Vector2[] points = new Vector2[4];
                points[0] = new Vector2(0.0f, _rectTrasnform.rect.height);
                points[1] = new Vector2(-_rectTrasnform.rect.width * 0.5f, _rectTrasnform.rect.height);
                points[2] = new Vector2(_rectTrasnform.rect.width * 1.5f, 0.0f);
                points[3] = new Vector2(_rectTrasnform.rect.width, 0.0f);
                _line.Points = points;
            } else {
                float distX = from.x - to.x;
                float distY = from.y - to.y;

                _rectTrasnform.sizeDelta = new Vector2(Mathf.Abs(distX), Mathf.Abs(distY));

                Vector2 basGauche = new Vector2(to.x, to.y);

                _rectTrasnform.localPosition = basGauche;

                Vector2[] points = new Vector2[4];
                points[1] = new Vector2(0.0f, 0.0f);
                points[1] = new Vector2(-_rectTrasnform.rect.width * 0.5f, 0.0f);
                points[2] = new Vector2(_rectTrasnform.rect.width * 1.5f, _rectTrasnform.rect.height);
                points[3] = new Vector2(_rectTrasnform.rect.width, _rectTrasnform.rect.height);
                _line.Points = points;
            }
        }
    }
}
