
public class StartNode : ExecutableNode{

    public StartNode(Fleet f, NodalEditor.SaveStruct nodes, int nodeIndex, SimulatedWideDataManager.SerializeContainer data) : base(f,nodes,nodeIndex,data) {
    }

    public override int Update(ServerUpdate serverUpdate) {

        SimulatedWideDataManager.SerializeContainer data = SimulatedWideDataManager.Container;

        foreach (int shipID in _fleet.ShipIDs) {
            Ship s = data._ships[shipID];
            serverUpdate.Add(s);
            s.Logs += "\n" + "Starting flight plan";
        }

        //set to next node
        foreach (LinkInfo l in _nodes.links) {
            if (l.FromID == _myID && l.FromParam == "StartOutput") {
                _fleet.CurrentNode = l.ToID;
                break;
            }
        }

        return _fleet.NextUpdateFrame;
    }
}
