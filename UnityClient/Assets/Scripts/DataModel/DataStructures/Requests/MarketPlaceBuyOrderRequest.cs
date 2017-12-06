
public class MarketPlaceBuyOrderRequest : Request {

    public int ResourceType { get; private set; }
    public int Qte { get; private set; }
    public int Price { get; private set; }
    public int HangarID { get; private set; }

    public int Result { get; set; }
    public Hangar Hangar { get; set; }
    public Corporation Corp { get; set; }

    public MarketPlaceBuyOrderRequest(int type, int qte, int price, int hangarID) {
        ResourceType = type;
        Qte = qte;
        Price = price;
        HangarID = hangarID;
    }

    public override void Update(CBHolder callbacks) {
        callbacks.hangarCB(Hangar);
        callbacks.corpCB(Corp);
    }
}
