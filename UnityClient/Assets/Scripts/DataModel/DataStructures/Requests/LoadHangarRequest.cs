using System.Collections.Generic;

public class LoadHangarRequest : Request {

    public int HangarID { get; private set; }

    public Hangar Hangar { get; set; }
    public List<ResourceStack> Stacks { get; set; }

    public LoadHangarRequest(int hangarID) { 
        HangarID = hangarID;
    }

    public override void Update(CBHolder callbacks) {
        callbacks.hangarCB(Hangar);
        if(null != Stacks) {
            foreach(ResourceStack r in Stacks) {
                callbacks.resourceStackCB(r);
            }
        }
    }
}
