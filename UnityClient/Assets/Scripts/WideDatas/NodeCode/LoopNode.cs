
public class LoopNode : ExecutableNode{

    public LoopNode(Fleet f, NodalEditor.SaveStruct nodes, int nodeIndex, SimulatedWideDataManager.SerializeContainer data) : base(f,nodes,nodeIndex,data) {
    }

    public override int Update(ServerUpdate serverUpdate) {

        int doneLoop = _fleet.FleetParams.GetInt("doneLoop", 0);
        int loopToDo = 2;

        if(doneLoop >= loopToDo - 1) {
            MoveFlow("FlowOutEnd");
        } else {
            _fleet.FleetParams.Set("doneLoop", doneLoop+1);
            MoveFlow("FlowOutRepeat");
        }

        return _fleet.NextUpdateFrame;
    }
}
