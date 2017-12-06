
public class CreateFlightPlanRequest : Request {

    public int PlanID { get; private set; }
    public int CorpID { get; private set; }
    public string Data { get; private set; }
    public string Name { get; private set; }

    public Corporation Corp { get; set; }

    public CreateFlightPlanRequest(int planID, int corpID, string data, string name) {
        PlanID = planID;
        CorpID = corpID;
        Data = data;
        Name = name;
    }

    public override void Update(CBHolder callbacks) {
        callbacks.corpCB(Corp);            
    }
}
