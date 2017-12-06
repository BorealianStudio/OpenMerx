
public class CreateCorpRequest : Request {

    public string Name { get; private set; }
    public int Char { get; private set; }
    public int Station { get; private set; }

    public Corporation Corporation { get; set; }
    public Character Character { get; set; }
    public Hangar Hangar { get; set; }
    public Ship Ship { get; set; }

    public CreateCorpRequest(int charID, int stationID, string name) {
        Name = name;
        Char = charID;
        Station = stationID;
    }

    public override void Update(CBHolder callbacks) {
        callbacks.charCB(Character);
        callbacks.corpCB(Corporation);
        callbacks.hangarCB(Hangar);
        callbacks.shipCB(Ship);
    }
}
