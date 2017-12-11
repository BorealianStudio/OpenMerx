
public class ExploreNode : ExecutableNode{

    public ExploreNode(Fleet f, NodalEditor.SaveStruct nodes, int nodeIndex, SimulatedWideDataManager.SerializeContainer data) : base(f,nodes,nodeIndex,data) {
    }

    public override int Update(ServerUpdate serverUpdate) {

        if (_data._currentFrame < _fleet.NextUpdateFrame)
            return _fleet.NextUpdateFrame;

        string currentState = _fleet.FleetParams.GetString("currentState", "none");
        switch (currentState) {
            case "none": {
                Ship s1 = _data._ships[_fleet.ShipIDs[0]];
                _fleet.LastHangar = s1.Hangar;
                Hangar h = _data._hangars[s1.Hangar];
                _fleet.LastStation = h.Station;
                _fleet.NextHangar = _fleet.LastHangar;

                StartUndock(serverUpdate);
            } break;
            case "undocking": { 
                Undock(serverUpdate);

                //start moveTo
                NextUpdateFrame(100);

                foreach (int sID in _fleet.ShipIDs) {
                    Ship ship = _data._ships[sID];
                    ship.Hangar = -1;
                    ship.Status = "Moving to explore site";
                    serverUpdate.Add(ship);
                }
                _fleet.FleetParams.Set("currentState", "moveOut");
            } break;
            case "moveOut": {
                NextUpdateFrame(500);

                foreach(int sID in _fleet.ShipIDs) { 
                    Ship ship = _data._ships[sID];
                    ship.Status = "Exploring";
                    ship.AddLog("Moved to explore site");
                    serverUpdate.Add(ship);
                }
                _fleet.FleetParams.Set("currentState", "explore");
            } break;
            case "explore": {
                NextUpdateFrame(200);

                foreach (int sID in _fleet.ShipIDs) {
                    Ship ship = _data._ships[sID];
                    ship.Status = "Moving back to station";
                    ship.AddLog("Done with exploration");
                    serverUpdate.Add(ship);
                }
                _fleet.FleetParams.Set("currentState", "moveIn");
            } break;
            case "moveIn": { 
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
                MoveFlow("ExploreOutput");
            }
            break;
        }
        
        return _fleet.NextUpdateFrame;
    }
}
