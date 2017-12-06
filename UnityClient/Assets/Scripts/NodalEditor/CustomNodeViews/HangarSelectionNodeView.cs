using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class HangarSelectionNodeView : MonoBehaviour {

    [SerializeField, Tooltip("Dropdown contenant les hangars disponibles")]
    Dropdown hangars = null;
    
    private Node _node = null;

    private List<Hangar> _hangarList = new List<Hangar>();
     
    private void Awake() {        
        hangars.options.Clear();

        int localCorp = LocalDataManager.instance.LocalCorporation.ID;
        foreach (Station s in LocalDataManager.instance.GetStations()) {
            if (s.Hangars.ContainsKey(localCorp)) {
                _hangarList.Add(LocalDataManager.instance.GetHangarInfo(s.Hangars[localCorp]));
                hangars.options.Add(new Dropdown.OptionData(s.Name));
            }
        }
        hangars.value = 0;
        hangars.RefreshShownValue();
        
        hangars.onValueChanged.AddListener(OnHangarChange);
    }

    public void SetNode(Node node) {
        _node = node;
        int hangarID = node.NodeInfos.nodeParams.GetInt("HangarID", -1);
        if(hangarID > 0) {
            for(int i = 0; i < _hangarList.Count; i++) {
                if(_hangarList[i].ID == hangarID) {
                    hangars.value = i;
                    hangars.RefreshShownValue();
                    break;
                }
            }
        }        
    }

    private void OnHangarChange(int value) {

        int hangarID = _hangarList[value].ID;
        _node.NodeInfos.nodeParams.Set("HangarID", hangarID);
        
    }
}
