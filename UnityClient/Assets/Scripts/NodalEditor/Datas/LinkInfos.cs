using System.Collections.Generic;

public class LinkInfo {

    public int FromID { get; set; }
    public string FromParam { get; set; }
    public int ToID { get; set; }
    public string ToParam { get; set; }
    public ParamInfos.ParamType LinkType { get; set; }

    public LinkInfo(int fromID, string fromParam, int toID, string toParam, ParamInfos.ParamType linkType) {
        FromID = fromID;
        FromParam = fromParam;
        ToID = toID;
        ToParam = toParam;
        LinkType = linkType;
    }
}
