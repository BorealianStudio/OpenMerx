using UnityEngine;

public abstract class WideDataManager : MonoBehaviour {
    public static WideDataManager wideDataManager = null;

    public LocalDataManager localDataManager = null;

    public delegate void RequestCB(Request r);
    public event RequestCB OnRequestResult = delegate { };

    public delegate void ServerUpdateAction(ServerUpdate s);
    public event ServerUpdateAction OnServerUpdate = delegate { };

    public abstract long SendRequest(Request r);

    public static long Request(Request r) {
        return wideDataManager.SendRequest(r);
    }

    protected void FinishRequest(Request r) {
        OnRequestResult(r);
    }

    protected void ServerUpdate(ServerUpdate s) {
        OnServerUpdate(s);
    }
}
