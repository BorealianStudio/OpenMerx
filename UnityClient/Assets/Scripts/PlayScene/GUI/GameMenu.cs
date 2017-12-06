using UnityEngine;

public class GameMenu : MonoBehaviour {

    public void OnReturnToGameClic() {
        CanvasGroup cg = GetComponent<CanvasGroup>();
        cg.alpha = 0.0f;
        cg.blocksRaycasts = false;
        cg.interactable = false;
        gameObject.SetActive(false);
    }
}
