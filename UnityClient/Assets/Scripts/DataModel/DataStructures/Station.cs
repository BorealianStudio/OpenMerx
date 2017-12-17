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

public class Station : DataObject {

    public int ID { get; private set; }
    public string Name { get; set; }
    public Dictionary<int,int> Hangars { get; set; }    /// les hangars par corpo hangars[1] donne l'id du hangar de la corp 1

    public Station(int id) {
        ID = id;
        Hangars = new Dictionary<int, int>();
    }

    public override List<EventHolder> Update(DataObject o) {
        List<EventHolder> result = new List<EventHolder>();        
        Station s = o as Station;
        if (null == s)
            return result;


        string data = Newtonsoft.Json.JsonConvert.SerializeObject(s);
        var serializerSettings = new Newtonsoft.Json.JsonSerializerSettings { ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace };
        Newtonsoft.Json.JsonConvert.PopulateObject(data, this, serializerSettings);

        result.Add(new StationChangeEvent(this, LocalDataManager.instance.OnStationChange));

        return result;
    }
}
