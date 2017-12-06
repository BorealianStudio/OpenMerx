using System.Collections.Generic;

public class MarketDataRequest : Request {

    public int ResourceType { get; private set; }

    public List<MarketData> Datas { get; set; }

    public MarketDataRequest(int resourceType) {
        ResourceType = resourceType;
    }

    public override void Update(CBHolder callbacks) {
    }
}
