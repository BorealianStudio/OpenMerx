using System.Collections.Generic;

public class Hangar : DataObject {
    public int ID { get; private set; }

    public int Station { get; set; }
    public int Corp { get; set; }
    public List<int> Ships { get; set; }
    public List<int> StacksIDs { get; set; }

    public Hangar(int id) {
        ID = id;
        Ships = new List<int>();
    }

    public override List<EventHolder> Update(DataObject o) {
        List<EventHolder> result = new List<EventHolder>();
        Hangar h = o as Hangar;
        if (null == h || h.ID != ID)
            return result;

        string data = Newtonsoft.Json.JsonConvert.SerializeObject(h);
        var serializerSettings = new Newtonsoft.Json.JsonSerializerSettings { ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace };
        Newtonsoft.Json.JsonConvert.PopulateObject(data, this, serializerSettings);

        result.Add(new HangarChangeEvent(this, LocalDataManager.instance.OnHangarChange));

        return result;
    }
}
