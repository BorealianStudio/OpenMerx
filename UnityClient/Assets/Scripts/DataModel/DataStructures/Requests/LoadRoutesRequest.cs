using System;

public class LoadRoutesRequest : Request {

    public Routes Routes { get; set; }

    public LoadRoutesRequest() {
    }

    public override void Update(CBHolder callbacks) {
        //rien a faire        
    }
}
