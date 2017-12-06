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
