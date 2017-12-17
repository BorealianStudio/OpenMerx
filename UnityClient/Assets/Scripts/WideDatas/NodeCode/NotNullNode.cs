
public class NotNull : ExecutableNode, IBooleanParam{

    public const string InputParam = "inputParam";
    public const string OutputParam = "outputResult";

    public NotNull(Fleet f, NodalEditor.SaveStruct nodes, int nodeIndex, SimulatedWideDataManager.SerializeContainer data) : base(f,nodes,nodeIndex,data) {
    }

    public override int Update(ServerUpdate serverUpdate) {
        throw new System.Exception("NotNull node should never get the Flow focus");
    }

    public bool GetBool(string paramName) {

        LinkInfo l = GetSourceLink(InputParam);
        ExecutableNode node = GetNode(l.FromID);

        IBookmarkParam b = node as IBookmarkParam;
        Bookmark bookmark = b.GetBookmark(l.FromParam);

        return bookmark != null;
    }
}
