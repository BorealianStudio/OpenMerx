
public class LoadResourceStackRequest : Request {

    public int StackID { get; private set; }

    public ResourceStack Stack { get; set; }

    public LoadResourceStackRequest(int stackID) {
        StackID = stackID;
    }

    public override void Update(CBHolder callbacks) {
        callbacks.resourceStackCB(Stack);
    }
}
