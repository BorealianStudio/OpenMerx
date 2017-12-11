
public  static class ExecutableNodeFactory {

    public static ExecutableNode GetNode(Fleet fleet, NodalEditor.SaveStruct data, int nodeID) {
        if (!data.nodes.ContainsKey(nodeID))
            throw new System.Exception("NodeID n'existe pas lors de l'instaciation d'une executionNode");
                
        NodeInfos n = data.nodes[nodeID];
        switch (n.type) {
            case "Start": return new StartNode(fleet, data, nodeID, SimulatedWideDataManager.Container);
            case "End": return new EndNode(fleet, data, nodeID, SimulatedWideDataManager.Container);
            case "Explore": return new ExploreNode(fleet, data, nodeID, SimulatedWideDataManager.Container);
            case "Move To": return new MoveToNode(fleet, data, nodeID, SimulatedWideDataManager.Container);
        }

        throw new System.Exception(n.type + " : Unkown type of executionNode");        
    }
}
