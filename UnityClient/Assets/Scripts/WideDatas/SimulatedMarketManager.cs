using System.Collections.Generic;
using System.Linq;

public class SimulatedMarketManager {

    private SimulatedWideDataManager _manager = null;
    private SimulatedWideDataManager.SerializeContainer _container = null;

    private Dictionary<int, int> _timeToUpdate = new Dictionary<int, int>();

    public SimulatedMarketManager(SimulatedWideDataManager manager, SimulatedWideDataManager.SerializeContainer container) {
        _manager = manager;
        _container = container;
    }

    public void UpdateOneFrame() {
        for(int i = 0; i < 6; i++) {
            if (!_timeToUpdate.ContainsKey(i))
                _timeToUpdate.Add(i,1);

            if (_timeToUpdate[i] > 100) {
                //on augmente la demande pour la ressource de type i
                List<MarketData> datas = GetBuyOrders(i);
                if (datas.Count == 0) {
                    MarketData d = new MarketData(i, 100, i * 100, true);
                    _container._market.Add(d);
                } else {
                    MarketData m = datas[0];
                    if (m.Qte == 0)
                        m.Qte = 1;
                    m.Qte += 1000 / (m.Qte * 5);
                }
                _timeToUpdate[i] = 0;
            } else {
                _timeToUpdate[i]++;
            }
        }
    }

    public List<MarketData> GetBuyOrders(int type) {
        List<MarketData> result = new List<MarketData>();
        
        foreach(MarketData m in _container._market) {
            if(m.Buying && m.ResourceType == type) {
                result.Add(m);
            }
        }

        return result;
    }

    public MarketData CreateSellOrder(int resType, int qte, int price) {
        List<MarketData> toDelete = new List<MarketData>();
        foreach(MarketData m in _container._market.Where(o => o.Buying && o.ResourceType == resType && o.Price >= price).OrderByDescending(o => o.Price)) {
            if(m.Qte > qte) {
                m.Qte -= qte;
                qte = 0;
            } else {
                toDelete.Add(m);
                qte -= m.Qte;
            }
        }
        foreach(MarketData m in toDelete) {
            _container._market.Remove(m);
        }

        if (qte > 0) {
            MarketData m = new MarketData(resType, qte, price, false);
            _container._market.Add(m);
            return m;
        }
        return null;
    }

    public MarketData CreateBuyOrder() {
        MarketData data = new MarketData(0, 0, 0, true);

        _container._market.Add(data);

        return data;
    }
}
