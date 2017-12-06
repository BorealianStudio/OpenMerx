using System.Collections.Generic;

public class FlightPlan : DataObject {


    public int ID { get; private set; }
    public string Name { get; set; }
    public string Data { get; set; }

    public FlightPlan(int id) {
        ID = id;
    }

    public override List<EventHolder> Update(DataObject obj) {
        List<EventHolder> result = new List<EventHolder>();
        FlightPlan c = obj as FlightPlan;
        if (null == c)
            return result;

        string data = Newtonsoft.Json.JsonConvert.SerializeObject(c);
        var serializerSettings = new Newtonsoft.Json.JsonSerializerSettings { ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace };
        Newtonsoft.Json.JsonConvert.PopulateObject(data, this, serializerSettings);

        return result;
    }
}
