
public class MineNode : ExecutableNode{

    public const string InFlow = "inFlow";
    public const string OutFlow = "outFlow";
    public const string Bookmark = "bookMark";

    public MineNode(Fleet f, NodalEditor.SaveStruct nodes, int nodeIndex, SimulatedWideDataManager.SerializeContainer data) : base(f,nodes,nodeIndex,data) {
    }

    public override int Update(ServerUpdate serverUpdate) {
        MoveFlow(OutFlow);

        return _fleet.LastUpdateFrame;
    }
}
