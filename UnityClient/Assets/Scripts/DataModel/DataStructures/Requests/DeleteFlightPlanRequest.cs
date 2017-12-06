
public class DeleteFlightPlanRequest : Request {

    public int PlanID { get; private set; }
    public int CorpID { get; private set; }

    public Corporation Corporation { get; set; }

    public DeleteFlightPlanRequest(int planID, int corpID) {
        PlanID = planID;
        CorpID = corpID;
    }

    public override void Update(CBHolder callbacks) {
        callbacks.corpCB(Corporation);
    }
}
