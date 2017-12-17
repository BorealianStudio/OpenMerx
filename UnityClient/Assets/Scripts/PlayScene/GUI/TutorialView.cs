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

public class TutorialView : MonoBehaviour {

    public delegate void ButtonClic();
    public event ButtonClic OnButton1Clic = delegate { };
    public event ButtonClic OnButton2Clic = delegate { };

    [SerializeField]
    Text TextZone = null;
    
    [SerializeField]
    Button button1 = null;

    [SerializeField]
    Button button2 = null;

    private void Awake() {
        button1.onClick.AddListener(() => { OnButton1Clic(); });
        button2.onClick.AddListener(() => { OnButton2Clic(); });
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void SetButton1View(bool isVisible) {
        button1.gameObject.SetActive(isVisible);
    }

    public void SetTextButton1(string text) {
        button1.GetComponentInChildren<Text>().text = text;
    }

    public void SetButton2View(bool isVisible) {
        button2.gameObject.SetActive(isVisible);
    }

    public void SetTextButton2(string text) {
        button2.GetComponentInChildren<Text>().text = text;
    }

    public void SetText(string text) {
        TextZone.text = text;
    }
}
