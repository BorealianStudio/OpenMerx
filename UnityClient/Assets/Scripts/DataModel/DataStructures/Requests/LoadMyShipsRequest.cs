using System.Collections.Generic;

public class LoadMyShipsRequest : Request {

    public int Corp { get; private set; }

    public List<Ship> Ships { get; set; }

    public LoadMyShipsRequest(int corpID) {
        Corp = corpID;        
    }

    public override void Update(CBHolder callbacks) {
        foreach (Ship s in Ships) {
            callbacks.shipCB(s);
        }
    }
}
