using UnityEngine;

public class ParamInfos{

    public enum ParamDirection {
        ParamIn,
        ParamOut
    }

    public enum ParamType {
        ParamFlow,
        ParamShip,
        ParamHangar
    }

    public enum ParamConnectType {
        Param0_1,
        Param0_N,
        Param1_1,
        Param1_N        
    }

    public ParamType Type { get; private set; }
    public ParamDirection Direction { get; private set; }
    public ParamConnectType ConnectType { get; private set; }
    public string Name { get; private set; }
    
    public ParamInfos(ParamType type, ParamDirection dir, ParamConnectType connectType, string name) {
        Type = type;
        Direction = dir;
        ConnectType = connectType;
        Name = name;
    }
}
