using System.Collections.Generic;

public class ResourceStack : DataObject {
    public int ID { get; private set; }

    public int Qte { get; set; }
    public int Type { get; set; }

    public ResourceStack(int id) {
        ID = id;
    }

    public override List<EventHolder> Update(DataObject o) {
        List<EventHolder> result = new List<EventHolder>();
        ResourceStack r = o as ResourceStack;
        if (null == r || r.ID != ID)
            return result;

        string data = Newtonsoft.Json.JsonConvert.SerializeObject(r);
        var serializerSettings = new Newtonsoft.Json.JsonSerializerSettings { ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace };
        Newtonsoft.Json.JsonConvert.PopulateObject(data, this, serializerSettings);

        result.Add(new ResourceStackChangeEvent(this, LocalDataManager.instance.OnResourceStackChange));
        
        return result;
    }
}
