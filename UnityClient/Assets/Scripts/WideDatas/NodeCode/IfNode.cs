
public class IfNode : ExecutableNode{

    public const string InFlow = "inFlow";
    public const string TestValue = "testValue";
    public const string OutFlowTrue = "flowOutTrue";
    public const string OutFlowFalse = "flowOutFalse";
    

    public IfNode(Fleet f, NodalEditor.SaveStruct nodes, int nodeIndex, SimulatedWideDataManager.SerializeContainer data) : base(f,nodes,nodeIndex,data) {
    }

    public override int Update(ServerUpdate serverUpdate) {
        NodeInfos inputNode = null;
        string paramName = "";

        foreach (LinkInfo l in _nodes.links) {
            if(l.ToID == _myID && l.ToParam == TestValue) {
                inputNode = _nodes.nodes[l.FromID];
                paramName = l.FromParam;
            }
        }

        if (null == inputNode)
            throw new System.Exception("No input node for the IfNode");

        ExecutableNode node = GetNode(inputNode.id);
        IBooleanParam boolParam = node as IBooleanParam;

        bool result = boolParam.GetBool(paramName);

        if (result) {
            foreach(int i in _fleet.ShipIDs) {
                Ship ship = _data._ships[i];
                ship.AddLog("Condition checked");
            }
            MoveFlow(OutFlowTrue);
        } else {
            foreach (int i in _fleet.ShipIDs) {
                Ship ship = _data._ships[i];
                ship.AddLog("Condition failed");
            }
            MoveFlow(OutFlowFalse);            
        }

        return _fleet.LastUpdateFrame;
    }
}
