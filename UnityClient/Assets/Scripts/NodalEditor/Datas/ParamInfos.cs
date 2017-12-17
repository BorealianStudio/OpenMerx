// MIT License

// Copyright(c) 2017 Andre Plourde
// part of https://github.com/BorealianStudio/OpenMerx

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.


// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using UnityEngine;

public class ParamInfos{

    public enum ParamDirection {
        ParamIn,
        ParamOut
    }

    public enum ParamType {
        ParamFlow,
        ParamShip,
        ParamHangar,
        ParamBookmark,
        ParamBoolean
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
