using System.Collections.Generic;

public class Character : DataObject {
    public int ID { get; private set; }
    public string Name { get; set; }
    public int Corp { get; set; }

    public Character(int id) {
        ID = id;
        Corp = -1;
    }

    public override List<EventHolder> Update(DataObject o) {
        List<EventHolder> result = new List<EventHolder>();
        Character c = o as Character;
        if (null == c)
            return result;

        string data = Newtonsoft.Json.JsonConvert.SerializeObject(c);
        var serializerSettings = new Newtonsoft.Json.JsonSerializerSettings { ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace };
        Newtonsoft.Json.JsonConvert.PopulateObject(data, this, serializerSettings);

        result.Add(new CharacterChangeEvent(this, LocalDataManager.instance.OnCharacterChange));
        
        return result;
    }
}
