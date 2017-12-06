using System.Collections.Generic;

public class FleetCreateRequest : Request {

    public int FromHangarID { get; private set; }
    public List<int> ShipIDs { get; private set; }
    public string Data{ get; private set; }    

    public Fleet Fleet { get; set; }
    public List<Ship> Ships { get; set; }

    public FleetCreateRequest(int fromHangarID, List<int>shipIDs, string data) {
        FromHangarID = fromHangarID;
        ShipIDs = shipIDs;
        Data = data;
    }

    public override void Update(CBHolder callbacks) {

        callbacks.fleetCB(Fleet);

        foreach(Ship s in Ships) {
            callbacks.shipCB(s);
        }
    }
}
