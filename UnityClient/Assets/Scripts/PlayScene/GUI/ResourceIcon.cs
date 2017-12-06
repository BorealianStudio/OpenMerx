using UnityEngine;
using UnityEngine.UI;

public class ResourceIcon : MonoBehaviour {

    [SerializeField] Text Qte = null;
    [SerializeField] Text Name = null;

    public ResourceStack Stack { get; private set; }

    public void SetStack(ResourceStack stack) {
        Stack = stack;
        Qte.text = stack.Qte.ToString();
        ResourceInfos i = LocalDataManager.instance.ResourceDataManager.GetResourceInfos(stack.Type);
        if (i != null)
            Name.text = i.Name;
        else
            return;
    }
}