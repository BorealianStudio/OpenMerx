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


public class IfNode : ExecutableNode{

    public const string InFlow = "inFlow";
    public const string TestValue = "testValue";
    public const string OutFlowTrue = "flowOutTrue";
    public const string OutFlowFalse = "flowOutFalse";
    

    public IfNode(Fleet f, NodalEditor.SaveStruct nodes, int nodeIndex, SimulatedWideDataManager.SerializeContainer data) : base(f,nodes,nodeIndex,data) {
    }

    public override int Update(ServerUpdate serverUpdate) {
        NodeInfos inputNode = null;
        string paramName = "";

        foreach (LinkInfo l in _nodes.links) {
            if(l.ToID == _myID && l.ToParam == TestValue) {
                inputNode = _nodes.nodes[l.FromID];
                paramName = l.FromParam;
            }
        }

        if (null == inputNode)
            throw new System.Exception("No input node for the IfNode");

        ExecutableNode node = GetNode(inputNode.id);
        IBooleanParam boolParam = node as IBooleanParam;

        bool result = boolParam.GetBool(paramName);

        if (result) {
            foreach(int i in _fleet.ShipIDs) {
                Ship ship = _data._ships[i];
                ship.AddLog("Condition checked");
            }
            MoveFlow(OutFlowTrue);
        } else {
            foreach (int i in _fleet.ShipIDs) {
                Ship ship = _data._ships[i];
                ship.AddLog("Condition failed");
            }
            MoveFlow(OutFlowFalse);            
        }

        return _fleet.LastUpdateFrame;
    }
}
