// MIT License

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

public class Ship : DataObject {
    public int ID { get; private set; }

    public string Name { get; set; }
    public int Corp { get; set; }
    public int Hangar { get; set; }
    public string Status { get; set; }
    public string Logs { get; set; }
    public int Fleet { get; set; }

    public Ship(int id) {
        ID = id;
    }

    public void AddLog(string log) {
        Logs += "\n" + log;
    }

    public override List<EventHolder> Update(DataObject o) {
        List<EventHolder> result = new List<EventHolder>();
        Ship s = o as Ship;
        if (null == s)
            return result;
                
        string data = Newtonsoft.Json.JsonConvert.SerializeObject(s);
        var serializerSettings = new Newtonsoft.Json.JsonSerializerSettings { ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace };
        Newtonsoft.Json.JsonConvert.PopulateObject(data, this, serializerSettings);

        result.Add(new ShipChangeEvent(this,LocalDataManager.instance.OnShipChange));
        return result ;
    }


}
