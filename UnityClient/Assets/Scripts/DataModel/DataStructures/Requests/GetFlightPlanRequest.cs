using System.Collections.Generic;

public class GetFlightPlanRequest : Request {

    public List<int> PlanIDs { get; private set; }
    public List<FlightPlan> Plans { get; set; }

    public GetFlightPlanRequest(List<int> planIDs) {
        PlanIDs = planIDs;
    }

    public override void Update(CBHolder callbacks) {
        foreach(FlightPlan f in Plans) {
            callbacks.flighPlanCB(f);
        }
    }
}
