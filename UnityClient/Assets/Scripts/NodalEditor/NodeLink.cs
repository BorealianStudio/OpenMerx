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
using UnityEngine.UI.Extensions;

public class NodeLink : MonoBehaviour {

    [SerializeField, Tooltip("")]
    Button buttonSelect = null;

    UILineRenderer _line = null;
    RectTransform _rectTrasnform;

    Vector2 from = Vector2.zero;
    Vector2 to = Vector2.zero;

    public NodalEditor Editor { get; set; }

    public LinkInfo infos { get; set; }

    private void Awake() {
        _line = GetComponent<UILineRenderer>();
        _line.color = Color.red;
        _rectTrasnform = GetComponent<RectTransform>();
        _rectTrasnform.pivot = Vector2.zero;

        buttonSelect.onClick.AddListener(OnClic);

        UpdateView();
    }

    public void SetColor(Color color) {
        _line.color = color;
        buttonSelect.image.color = color;
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
    
    private void OnClic() {
        if (Editor.CanSelect()){
            Editor.SetActiveLink(this);
        }
    }

    void UpdateView() {

        float dist = Vector2.Distance(from, to);
        _line.BezierSegmentsPerCurve = System.Convert.ToInt32(dist / 10.0f);

        if (from.x < to.x) { 
            if (to.y > from.y) {
                float distX = to.x - from.x;
                float distY = to.y - from.y;

                _rectTrasnform.sizeDelta = new Vector2(Mathf.Abs(distX), Mathf.Abs(distY));

                Vector2 basGauche = new Vector2(from.x, from.y);

                _rectTrasnform.localPosition = basGauche;

                Vector2[] points = new Vector2[4];
                points[0] = new Vector2(0.0f, 0.0f);
                points[1] = new Vector2(_rectTrasnform.rect.width * 0.5f, 0.0f);
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
