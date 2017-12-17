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

public class NodeParamLine : MonoBehaviour {
    [SerializeField, Tooltip("Bouton du input du parametre")]
    Button inputButton = null;

    [SerializeField, Tooltip("Bouton du output du parametre")]
    Button outputButton = null;

    [SerializeField, Tooltip("Le text qui affiche le nom du parametre")]
    Text paramName = null;

    public Button SetData(ParamInfos p) {

        paramName.text = p.Name;

        if(p.Direction == ParamInfos.ParamDirection.ParamIn) {
            outputButton.gameObject.SetActive(false);
            inputButton.gameObject.SetActive(true);
            return inputButton;
        } else {
            outputButton.gameObject.SetActive(true);
            inputButton.gameObject.SetActive(false);
            return outputButton;
        }
    }
}
