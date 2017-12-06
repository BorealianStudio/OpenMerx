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
