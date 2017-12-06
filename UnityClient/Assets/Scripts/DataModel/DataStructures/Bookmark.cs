using System.Collections.Generic;

public class Bookmark {

    public int ID { get; private set; }
    public int PointID { get; private set; }

    public ParamHolder datas { get; set; }

    public Bookmark(int id, int pointID) {
        ID = id;
        PointID = pointID;
    }
}
