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

public class SimulatedPOIsManager {

    private SimulatedWideDataManager _manager = null;
    private SimulatedWideDataManager.SerializeContainer _container = null;

    System.Random random = new System.Random();

    private int _count = 0;
    private int _timeToDelete = 1000;
    private int _nbRemove = 3;

    public SimulatedPOIsManager(SimulatedWideDataManager manager, SimulatedWideDataManager.SerializeContainer container) {
        _manager = manager;
        _container = container;
        random = new System.Random();
    }

    public void UpdateOneFrame() {
        _count++;
        if(_count > _timeToDelete) {
            _count = 0;
            List<int> pois = new List<int>(_container._POIs.Keys);
            for (int i = 0; i < _nbRemove; i++) {
                if (pois.Count > 0) { 
                    int index = random.Next(pois.Count);                    
                    _container._POIs.Remove(pois[index]);
                    pois.RemoveAt(index);
                }                
            }
        }
        while(_container._POIs.Count < 100) {
            CreatePOI();
        }
    }

    private PointOfInterest CreatePOI() {        
        int sector = random.Next(3);
        double dice = random.NextDouble();

        PointOfInterest result = null;

        if(dice > 0.5f) {
            result = CreateMiningSite(sector);
        } else {
            result = CreateUselessSite(sector);
        }
        double x = random.NextDouble();
        double y = random.NextDouble();
        double z = random.NextDouble();
        double sum = 1 / System.Math.Sqrt(x * x + y * y + z * z);
        x *= sum;
        y *= sum;
        z *= sum;

        double dist = random.NextDouble();
        switch (sector) {
            case 0: dist *= 1.0; break;
            case 1: dist *= 2.0 + 1.0; break;
            case 2: dist *= 4.0 + 3.0; break;
        }
        x *= dist;
        y *= dist;
        z *= dist;

        result.Datas.Set("posX", x);
        result.Datas.Set("posY", y);
        result.Datas.Set("posZ", z);

        _container._POIs.Add(result.ID, result);
        return result;
    }
    
    private PointOfInterest CreateMiningSite(int sector) {
        PointOfInterest result = new PointOfInterest(_container._POIsIDs++, sector);
        result.FindProba = 0.15f;
        result.Datas.Set("type", "mining");
        result.Description = 1; ;
        int dice = random.Next(8)+1;
        result.Datas.Set("rockType", dice);

        return result;
    }

    private PointOfInterest CreateUselessSite(int sector) {
        PointOfInterest result = new PointOfInterest(_container._POIsIDs++, sector);

        result.Description = 2;
        result.Datas.Set("type", "useless");
        result.FindProba = 0.75;
        return result;
    }
}
