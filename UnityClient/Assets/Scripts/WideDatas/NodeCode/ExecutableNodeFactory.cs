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


public static class ExecutableNodeFactory {

    public static ExecutableNode GetNode(Fleet fleet, NodalEditor.SaveStruct data, int nodeID) {
        if (!data.nodes.ContainsKey(nodeID))
            throw new System.Exception("NodeID n'existe pas lors de l'instaciation d'une executionNode");
                
        NodeInfos n = data.nodes[nodeID];
        switch (n.type) {
            case "Start": return new StartNode(fleet, data, nodeID, SimulatedWideDataManager.Container);
            case "End": return new EndNode(fleet, data, nodeID, SimulatedWideDataManager.Container);
            case "Explore": return new ExploreNode(fleet, data, nodeID, SimulatedWideDataManager.Container);
            case "If": return new IfNode(fleet, data, nodeID, SimulatedWideDataManager.Container);
            case "Move To": return new MoveToNode(fleet, data, nodeID, SimulatedWideDataManager.Container);
            case "NotNull": return new NotNull(fleet, data, nodeID, SimulatedWideDataManager.Container);
            case "Loop": return new LoopNode(fleet, data, nodeID, SimulatedWideDataManager.Container);            
        }

        throw new System.Exception(n.type + " : Unkown type of executionNode");        
    }
}
