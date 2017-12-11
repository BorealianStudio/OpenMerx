
public class EndNode : ExecutableNode{

    public EndNode(Fleet f, NodalEditor.SaveStruct nodes, int nodeIndex, SimulatedWideDataManager.SerializeContainer data) : base(f,nodes,nodeIndex,data) {
    }

    public override int Update(ServerUpdate serverUpdate) {

        SimulatedWideDataManager.SerializeContainer data = SimulatedWideDataManager.Container;


        foreach (int shipID in _fleet.ShipIDs) {
            Ship s = data._ships[shipID];
            serverUpdate.Add(s);
            s.Logs += "\n" + "End of flight plan";
        }

        data._fleets.Remove(_fleet.ID);       
        foreach (int shipID in _fleet.ShipIDs) {
            Ship s = data._ships[shipID];
            serverUpdate.Add(s);
            s.Fleet = 0;
        }

        return _fleet.NextUpdateFrame;
    }
}
