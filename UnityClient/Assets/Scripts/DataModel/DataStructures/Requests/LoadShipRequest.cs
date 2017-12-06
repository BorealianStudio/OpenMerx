public class LoadShipRequest : Request {

    public int ShipID { get; private set; }

    public Ship Ship { get; set; }

    public LoadShipRequest(int shipID) {
        ShipID = shipID;
    }

    public override void Update(CBHolder callbacks) {
        callbacks.shipCB(Ship);
    }
}
