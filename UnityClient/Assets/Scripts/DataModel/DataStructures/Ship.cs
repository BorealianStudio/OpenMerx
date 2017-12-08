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
