
// une node executable est la version code d'une node provenant d'un NodalEditor
public abstract class ExecutableNode {

    protected NodalEditor.SaveStruct _nodes = null;
    protected int _myID = 0;
    protected Fleet _fleet = null;
    protected SimulatedWideDataManager.SerializeContainer _data = null;

    public ExecutableNode(Fleet f, NodalEditor.SaveStruct nodes, int nodeIndex, SimulatedWideDataManager.SerializeContainer datas) {
        _nodes = nodes;
        _data = datas;
        _myID = nodeIndex;
        _fleet = f;
    }

    public abstract int Update(ServerUpdate serverUpdate);
    
    protected void NextUpdateFrame(int frameToAdd) {
        _fleet.LastUpdateFrame = _fleet.NextUpdateFrame;
        _fleet.NextUpdateFrame = _fleet.LastUpdateFrame + frameToAdd;
    }

    protected void MoveFlow(string paramName) {
        foreach (LinkInfo l in _nodes.links) {
            if (l.FromID == _myID && l.FromParam == paramName) {
                _fleet.CurrentNode = l.ToID;
                break;
            }
        }
        _fleet.FleetParams.Set("currentState", "none");
    }

    protected void StartUndock(ServerUpdate serverUpdate) {
        NextUpdateFrame(50);

        foreach (int sID in _fleet.ShipIDs) {
            Ship ship = _data._ships[sID];
            ship.Status = "Undocking";
            serverUpdate.Add(ship);
        }
        _fleet.FleetParams.Set("currentState", "undocking");
    }

    protected void Undock(ServerUpdate serverUpdate) {
        Hangar hangar = _data._hangars[_fleet.LastHangar];
        serverUpdate.Add(hangar);
        foreach (int sID in _fleet.ShipIDs) {
            Ship ship = _data._ships[sID];
            ship.Hangar = -1;
            ship.AddLog("undocked from station");
            serverUpdate.Add(ship);
            hangar.Ships.Remove(sID);
        }
    }

    protected void StartDock(ServerUpdate serverUpdate) {
        NextUpdateFrame(50);

        foreach (int sID in _fleet.ShipIDs) {
            Ship ship = _data._ships[sID];
            ship.Status = "Docking";
            serverUpdate.Add(ship);
        }

        _fleet.FleetParams.Set("currentState", "docking");
    }

    protected void Dock(ServerUpdate serverUpdate) {
        
        Hangar hangar = _data._hangars[_fleet.NextHangar];
        serverUpdate.Add(hangar);

        foreach (int sID in _fleet.ShipIDs) {
            Ship ship = _data._ships[sID];
            ship.AddLog("Docked to station");
            ship.Hangar = _fleet.NextHangar;
            serverUpdate.Add(ship);
            hangar.Ships.Add(sID);
        }
    }

    protected void StartFueling(ServerUpdate serverUpdate) {

        NextUpdateFrame(100);

        foreach (int sID in _fleet.ShipIDs) {
            Ship ship = _data._ships[sID];
            ship.Status = "Fueling";
            serverUpdate.Add(ship);  
        }

        _fleet.FleetParams.Set("currentState", "fueling");
    }

    protected void Fuel(ServerUpdate serverUpdate) {
        foreach (int sID in _fleet.ShipIDs) {
            Ship ship = _data._ships[sID];
            ship.AddLog("Fueled for X ICU");
            serverUpdate.Add(ship);
        }
        _fleet.FleetParams.Set("currentState", "doneFueling");
    }

}
