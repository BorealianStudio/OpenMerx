﻿// MIT License

// Copyright(c) 2017 Andre Plourde
// part of https://github.com/BorealianStudio/OpenMerx

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.


// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

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
