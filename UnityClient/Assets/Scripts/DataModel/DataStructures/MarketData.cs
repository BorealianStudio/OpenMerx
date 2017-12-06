
public class MarketData{
    public int ResourceType { get; set; }
    public int Qte { get; set; }
    public int Price { get; set; }
    public bool Buying { get; set; }

    public MarketData(int resType, int qte, int price, bool buying) {
        ResourceType = resType;
        Qte = qte;
        Price = price;
        Buying = buying;
    }
}
