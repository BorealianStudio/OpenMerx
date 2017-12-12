using UnityEngine;
using UnityEngine.UI;

public class NodeParamLine : MonoBehaviour {
    [SerializeField, Tooltip("Bouton du input du parametre")]
    Button inputButton = null;

    [SerializeField, Tooltip("Bouton du output du parametre")]
    Button outputButton = null;

    [SerializeField, Tooltip("Le text qui affiche le nom du parametre")]
    Text name = null;

    public Button SetData(ParamInfos p) {

        name.text = p.Name;

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
