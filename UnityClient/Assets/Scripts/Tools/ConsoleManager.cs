using UnityEngine;

public class ConsoleManager : MonoBehaviour {

    public delegate void OnChangeAction();
    public event OnChangeAction OnChange = delegate{};

    public string text = "";

    void Start () {
        DontDestroyOnLoad(this.gameObject);
        Application.logMessageReceived += HandleLog;        
    }
    
    void HandleLog(string message, string stackTrace, LogType type) {
        text += message + "\n";
        OnChange();
    }

}
