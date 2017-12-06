using System.Collections.Generic;

public class PointOfInterest {

    public int ID { get; private set; }
    public int Sector { get; private set; }

    public double FindProba { get; set; }
    public int Description { get; set; }
    public ParamHolder Datas { get; set; }

    public PointOfInterest(int id, int sector) {
        ID = id;
        Sector = sector;
        Datas = new ParamHolder();
    }

    public ParamHolder DatasToBookmark() {
        ParamHolder result = new ParamHolder();
        result.Set("posX", Datas.GetString("posX"));
        result.Set("posY", Datas.GetString("posY"));
        result.Set("posZ", Datas.GetString("posZ"));

        return result;
    }
}
