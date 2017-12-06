
public class BuyHangarRequest : Request {

    public int StationID { get; private set; }
    public int CorpID{ get; private set; }

    public Hangar Hangar { get; set; }
    public Station Station { get; set; }

    public BuyHangarRequest(int stationID, int corpID) {
        StationID = stationID;
        CorpID = corpID;
    }

    public override void Update(CBHolder callbacks) {
        callbacks.hangarCB(Hangar);
        callbacks.stationCB(Station);
    }
}
