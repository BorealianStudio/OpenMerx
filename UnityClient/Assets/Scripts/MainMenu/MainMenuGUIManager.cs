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

public class MainMenuGUIManager : MonoBehaviour {

    public void OnStartClic() {
        GameObject obj = new GameObject("simulatedData");
        DontDestroyOnLoad(obj);
        LocalDataManager.instance = obj.AddComponent<LocalDataManager>();
        WideDataManager.wideDataManager = obj.AddComponent<SimulatedWideDataManager>();
        WideDataManager.wideDataManager.localDataManager = LocalDataManager.instance;
        LocalDataManager.instance.SetLocalCharacterID(1);
        UnityEngine.SceneManagement.SceneManager.LoadScene("PlayScene");
    }

    public void OnLoadClic() {
        GameObject obj = new GameObject("simulatedData");
        DontDestroyOnLoad(obj);
        LocalDataManager.instance = obj.AddComponent<LocalDataManager>();
        SimulatedWideDataManager sm = obj.AddComponent<SimulatedWideDataManager>();
        string str = "";

        if (Application.platform == RuntimePlatform.WebGLPlayer) {
            str = PlayerPrefs.GetString("save");
        } else {
            str = System.IO.File.ReadAllText("savegame.txt");
        }
        sm.LoadFromString(str);
        sm._loadTime = 0.1f;
        WideDataManager.wideDataManager = sm;
        WideDataManager.wideDataManager.localDataManager = LocalDataManager.instance;
        LocalDataManager.instance.SetLocalCharacterID(1);
        UnityEngine.SceneManagement.SceneManager.LoadScene("PlayScene");

        
    }
}
