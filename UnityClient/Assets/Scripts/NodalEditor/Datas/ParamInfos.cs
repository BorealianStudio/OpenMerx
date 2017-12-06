using UnityEngine;

public class ParamInfos{

    public enum ParamDirection {
        ParamIn,
        ParamOut,
        FlowOut          
    }
    public enum ParamType {
        ParamFlow,
        ParamShip,
        ParamHangar
    }

    public ParamType Type { get; set; }
    public ParamDirection Direction { get; set; }
    public string Name { get; set; }

    public ParamInfos(ParamType type, ParamDirection dir, string name) {
        Type = type;
        Direction = dir;
        Name = name;
    }
}
