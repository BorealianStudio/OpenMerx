using System.Collections.Generic;

public class Fleet : DataObject{


    public int ID { get; private set; }

    public string Data { get; set; }
    public List<int> ShipIDs { get; set; }
    public int LastStation { get; set; }
    public int LastHangar { get; set; }
    public int CurrentNode { get; set; }
    public int TaskStartFrame { get; set; }
    public ParamHolder FleetParams { get; set; }
    

    public Fleet(int id) {
        ID = id;
        ShipIDs = new List<int>();
    }

    public override List<EventHolder> Update(DataObject o) {
        List<EventHolder> result = new List<EventHolder>();
        Fleet c = o as Fleet;
        if (null == c || c.ID != ID)
            return result;

        string data = Newtonsoft.Json.JsonConvert.SerializeObject(c);
        var serializerSettings = new Newtonsoft.Json.JsonSerializerSettings { ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace };
        Newtonsoft.Json.JsonConvert.PopulateObject(data, this, serializerSettings);

        return result;
    }
}
