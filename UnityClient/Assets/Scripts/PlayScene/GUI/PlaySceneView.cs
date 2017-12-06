using System.Collections;

using UnityEngine;

public class PlaySceneView : MonoBehaviour {

    private LocalDataManager _localManager = null;

    [SerializeField] Header header = null;
    [SerializeField] GameObject loading = null;

    private void Start() {
        //on recupere le local data manager
        _localManager = FindObjectOfType<LocalDataManager>();

        Repaint();
        StartCoroutine(WaitReady());
    }

    private IEnumerator WaitReady() {
        loading.SetActive(true);

        while (!_localManager.Ready) {
            yield return null;
        }
        loading.SetActive(false);
    }

    private void Repaint() {
        header.gameObject.SetActive(true);
    }

    public void OnSave() {
        SimulatedWideDataManager m = FindObjectOfType<SimulatedWideDataManager>();
        if (null != m) {
            string str = m.SaveToString();
            if (Application.platform == RuntimePlatform.WebGLPlayer) {
                PlayerPrefs.SetString("save",str);
                Debug.Log("save : " + str);
            } else {
                System.IO.File.WriteAllText("savegame.txt", str);
            }
            
        }
    }
}
