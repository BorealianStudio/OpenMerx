using UnityEngine;

public class TestLoad : MonoBehaviour {

	void Awake () {
        //chercher si il existe des données ou si on est en mode simulation
        LocalDataManager lm = FindObjectOfType<LocalDataManager>();
        if (null == lm) {
            StartSim();
        }
	}

    private void StartSim() {
        GameObject obj = new GameObject("simulatedData");
        LocalDataManager.instance = obj.AddComponent<LocalDataManager>();
        WideDataManager.wideDataManager = obj.AddComponent<SimulatedWideDataManager>();
        WideDataManager.wideDataManager.localDataManager = LocalDataManager.instance;
        LocalDataManager.instance.SetLocalCharacterID(1);
    }

}
