using System.Collections.Generic;

public class NodeInfos {
    public struct ParamDetail {
        public int NodeID;
        public string paramName;
    }

    public int id;
    public float posX;
    public float posY;
    public string type;
    public ParamHolder nodeParams = new ParamHolder();
}
