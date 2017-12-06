using System.Collections.Generic;

public class Corporation : DataObject{


    public int ID { get; private set; }

    public string Name { get; set; }
    public int ICU { get; set; }
    public List<int> Ships { get; set; }

    public int Owner { get; set; }
    public int Station { get; set; }
    public List<int> Bookmarks { get; set; }
    public List<int> FlightPlans { get; set; }
    
    public Corporation(int id) {
        Ships = new List<int>();
        ID = id;
    }

    public override List<EventHolder> Update(DataObject o) {
        List<EventHolder> result = new List<EventHolder>();
        Corporation c = o as Corporation;
        if (null == c || c.ID != ID)
            return result;

        string data = Newtonsoft.Json.JsonConvert.SerializeObject(c);
        var serializerSettings = new Newtonsoft.Json.JsonSerializerSettings { ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace };
        Newtonsoft.Json.JsonConvert.PopulateObject(data, this, serializerSettings);

        result.Add(new CorporationChangeEvent(this, LocalDataManager.instance.OnCorporationChange));

        return result;
    }
}
