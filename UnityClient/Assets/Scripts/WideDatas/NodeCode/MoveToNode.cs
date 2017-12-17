
public class MoveToNode : ExecutableNode{

    public MoveToNode(Fleet f, NodalEditor.SaveStruct nodes, int nodeIndex, SimulatedWideDataManager.SerializeContainer data) : base(f,nodes,nodeIndex,data) {
    }

    public override int Update(ServerUpdate serverUpdate) {
        
        if (_data._currentFrame >= _fleet.NextUpdateFrame) {

            string lastDoneStep = _fleet.FleetParams.GetString("currentState", "none");
            switch (lastDoneStep) {
                case "none": { //undocking
                    //preparer les données

                    int hangarID = 1;
                    foreach (LinkInfo l in _nodes.links) {
                        if (l.ToID == _myID && l.ToParam == "Station") {
                            NodeInfos hangarSelectorNode = _nodes.nodes[l.FromID];
                            hangarID = hangarSelectorNode.nodeParams.GetInt("HangarID");
                        }
                    }
                    _fleet.NextHangar = hangarID;

                    //demarer
                    StartUndock(serverUpdate);
                } break;
                case "undocking": {
                    Undock(serverUpdate);

                    NextUpdateFrame(200);   //todo calculer le temps pour se rendre
                                        
                    _fleet.ShipIDs.ForEach((sID) => {
                        Ship ship = _data._ships[sID];
                        ship.Status = "Moving to station";
                        serverUpdate.Add(ship);
                    });

                    _fleet.FleetParams.Set("currentState", "moving");
                } break;
                case "moving": {
                    _fleet.ShipIDs.ForEach((sID) => {
                        Ship ship = _data._ships[sID];
                        ship.AddLog("Moved to target station");
                        serverUpdate.Add(ship);
                    });                        

                    _fleet.FleetParams.Set("currentState", "dock");
                } break;
                case "dock": {
                    StartDock(serverUpdate);
                } break;
                case "docking": {
                    Dock(serverUpdate);
                    StartFueling(serverUpdate);
                } break;
                case "fueling": {
                    Fuel(serverUpdate);
                } break;
                case "doneFueling": {
                    foreach(int sID in _fleet.ShipIDs) { 
                        Ship ship = _data._ships[sID];
                        ship.Status = "Idle in station";
                    }
                    MoveFlow("MoveOutput");
                } break;
            }
        }

        return _fleet.NextUpdateFrame;
    }
}
