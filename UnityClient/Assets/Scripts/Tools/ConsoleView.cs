using UnityEngine;
using UnityEngine.UI;

public class ConsoleView : MonoBehaviour {
        
    [SerializeField] Text text = null;

    ConsoleManager _manager = null;

    private void Start() {
        _manager = FindObjectOfType<ConsoleManager>();
        _manager.OnChange += OnChange;
        OnChange();
    }

    private void OnDestroy() {
        _manager.OnChange -= OnChange;
    }

    private void OnChange() {
        text.text = _manager.text;
    }

}
