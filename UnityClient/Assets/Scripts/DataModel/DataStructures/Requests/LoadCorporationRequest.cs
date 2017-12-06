
public class LoadCorporationRequest : Request {

    public int CorpID { get; private set; }

    public Corporation Corp { get; set; }

    public LoadCorporationRequest(int corpID) {
        CorpID = corpID;
    }

    public override void Update(CBHolder callbacks) {
        callbacks.corpCB(Corp);
    }
}
