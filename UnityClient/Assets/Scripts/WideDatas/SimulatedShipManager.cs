using System.Collections.Generic;
using System.Linq;

public class SimulatedShipManager{

    SimulatedWideDataManager _manager = null;
    SimulatedWideDataManager.SerializeContainer _container = null;

    Dictionary<int, int> _nextChange = null;

    private ServerUpdate _currentUpdate = null;

    public SimulatedShipManager(SimulatedWideDataManager manager, SimulatedWideDataManager.SerializeContainer container) {
        _manager = manager;
        _container = container;
        _nextChange = new Dictionary<int, int>();
    }

    public void UpdateOneFrame() {
        int frame = _manager.Frame;

        List<Fleet> copy = new List<Fleet>(_container._fleets.Values);


        foreach (Fleet f in copy) {
            _currentUpdate = new ServerUpdate();

            if(!_nextChange.ContainsKey(f.ID) || _nextChange[f.ID] <= _container._currentFrame) {
                if (!_nextChange.ContainsKey(f.ID))
                    _nextChange.Add(f.ID, 0);
                 _nextChange[f.ID] = UpdateFlee(f);
            }
            _manager.SendServerUpdate(_currentUpdate);
            _currentUpdate = null;
        }
    }

    private void AddLog(Fleet f, string msg) {
        foreach (int s in f.ShipIDs) {            
            Ship ship = _container._ships[s];
            _currentUpdate.Add(ship);
            ship.Logs += "\n" + msg;
        }
    }

    private int UpdateFlee(Fleet f) {
        if (f.FleetParams == null) {
            f.FleetParams = new ParamHolder();
        }

        NodalEditor.SaveStruct s = Newtonsoft.Json.JsonConvert.DeserializeObject<NodalEditor.SaveStruct>(f.Data);
        if (s.nodes.ContainsKey(f.CurrentNode)) {
            return UpdateNode(f, s.nodes[f.CurrentNode], s);
        }
        throw new System.Exception("pas de node active???");
    }

    private int UpdateNode(Fleet f, NodeInfos n, NodalEditor.SaveStruct data) {
        switch (n.type) {
            case "Start": return UpdateStart(f, n, data);
            case "End": return UpdateStop(f, n, data);
            case "Explore": return UpdateExplore(f, n, data);
            case "Move To": return UpdateMoveTo(f, n, data);
        }
        throw new System.Exception("Node type unexpected " + n.type);
    }

    private int UpdateStart(Fleet f, NodeInfos n, NodalEditor.SaveStruct data) {

        AddLog(f,"Starting flight plan");
        
        foreach(LinkInfo l in data.links) {
            if(l.FromID == n.id && l.FromParam == "StartOutput") {
                f.CurrentNode = l.ToID;
                break;
            }
        }        
             
        return 0;
    }

    private int UpdateStop(Fleet f, NodeInfos n, NodalEditor.SaveStruct data) {
        _container._fleets.Remove(f.ID);
        foreach(int shipID in f.ShipIDs) {
            Ship s = _container._ships[shipID];
            _currentUpdate.Add(s);
            s.Fleet = 0;
        }
        AddLog(f, "End of flight plan");

        return 0;
    }

    private int UpdateExplore(Fleet f, NodeInfos n, NodalEditor.SaveStruct data) {
        //preparation des parametres
        int doneFrameInNode = _container._currentFrame - f.TaskStartFrame;

        if(_container._currentFrame >= f.TaskStartFrame) {

            int timeToUndock = 100;
            int timeToMoveZone = timeToUndock + 200;
            int timeToExplore = timeToMoveZone + 500;
            int timeToMoveBack = timeToExplore + 200;
            int timeToDock = timeToMoveBack + 100;
            int timeToFuel = timeToDock + 200;

            string lastDoneStep = f.FleetParams.GetString("lastState","none");
            switch (lastDoneStep) {
                case "none": { //undocking
                    f.ShipIDs.ForEach((sID) => {
                        Ship ship = _container._ships[sID];                        
                        ship.Status = "Undocking";
                        _currentUpdate.Add(ship);
                    });
                    f.FleetParams.Set("lastState", "undock");
                    return f.TaskStartFrame + timeToUndock;
                }
                case "undock": { //moving to site
                    if (doneFrameInNode >= timeToUndock) {

                        f.FleetParams.Set("fromHangar", f.LastHangar);
                        f.FleetParams.Set("fromStation", f.LastStation);

                        Hangar hangar = _container._hangars[f.LastHangar];
                        _currentUpdate.Add(hangar);
                        f.ShipIDs.ForEach((sID) => {
                            Ship ship = _container._ships[sID];
                            ship.Status = "moving to site";
                            ship.Hangar = -1;
                            _currentUpdate.Add(ship);
                            hangar.Ships.Remove(sID);
                        });
                        AddLog(f, "undocked from station");

                        //prepare next step
                        f.FleetParams.Set("lastState", "moveOut");
                        return f.TaskStartFrame + timeToMoveZone;
                    } else {
                        return f.TaskStartFrame + timeToUndock;
                    }
                } 
                case "moveOut": { //Exploring
                    if (doneFrameInNode >= timeToMoveZone) {
                        f.ShipIDs.ForEach((sID) => {
                            Ship ship = _container._ships[sID];
                            ship.Status = "Exploring";
                        });
                        AddLog(f, "Moved to explore site");
                        f.FleetParams.Set("lastState", "explore");
                        return f.TaskStartFrame + timeToExplore;
                    } else {
                        return f.TaskStartFrame + timeToMoveZone;
                    } 
                }
                case "explore": { //moving back to station
                    if (doneFrameInNode >= timeToExplore) {
                        f.ShipIDs.ForEach((sID) => {
                            Ship ship = _container._ships[sID];
                            ship.Status = "Moving back to station";
                        });
                        AddLog(f, "Done with exploration");
                        f.FleetParams.Set("lastState", "moveIn");
                        return f.TaskStartFrame + timeToMoveBack;
                    } else {
                        return f.TaskStartFrame + timeToExplore;
                    }
                } 
                case "moveIn": { //Docking
                    if (doneFrameInNode >= timeToMoveBack) {
                        f.ShipIDs.ForEach((sID) => {
                            Ship ship = _container._ships[sID];
                            ship.Status = "Docking";
                        });
                        AddLog(f, "Returned to station");
                        f.FleetParams.Set("lastState", "docking");
                        return f.TaskStartFrame + timeToDock;
                    } else {
                        return f.TaskStartFrame + timeToMoveBack;
                    }
                }
                case "docking": { //Fueling
                    if (doneFrameInNode >= timeToDock) {
                        f.ShipIDs.ForEach((sID) => {
                            Ship ship = _container._ships[sID];
                            ship.Status = "Fueling";
                        });
                        AddLog(f, "Docked to station");
                        f.FleetParams.Set("lastState", "fueling");
                        return f.TaskStartFrame + timeToFuel;
                    } else {
                        return f.TaskStartFrame + timeToDock;
                    }
                }
                case "fueling": { //next step
                    if (doneFrameInNode >= timeToFuel) {
                        int hangarID = f.FleetParams.GetInt("fromHangar");

                        Hangar hangar = _container._hangars[hangarID];
                        _currentUpdate.Add(hangar);
                        f.ShipIDs.ForEach((sID) => {
                            Ship ship = _container._ships[sID];
                            ship.Hangar = hangarID;
                            _currentUpdate.Add(ship);
                            hangar.Ships.Add(sID);
                        });
                        AddLog(f, "undocked from station");

                        //prepare fueling
                        AddLog(f, "Fueled for X ICU");
                        f.FleetParams.Set("lastState", "nextStep");
                        return f.TaskStartFrame + timeToFuel;
                    } else {
                        return f.TaskStartFrame + timeToFuel;
                    }
                }
                case "nextStep": { //all done, next node
                    if (doneFrameInNode >= timeToFuel) {
                        f.ShipIDs.ForEach((sID) => {
                            Ship ship = _container._ships[sID];
                            ship.Status = "Idle in station";
                        });
                        foreach(LinkInfo l in data.links) {
                            if(l.FromID == n.id && l.FromParam == "ExploreOutput") {
                                f.CurrentNode = l.ToID;
                                break;
                            }
                        }
                        f.FleetParams.Set("lastState", "none");                        
                        f.TaskStartFrame = f.TaskStartFrame + timeToFuel;
                        return f.TaskStartFrame + timeToFuel;
                    } else {
                        return f.TaskStartFrame + timeToFuel;
                    }
                };
            }
        }

        return f.TaskStartFrame;
    }

    private int UpdateMoveTo(Fleet f, NodeInfos n, NodalEditor.SaveStruct data) {
        //preparation des parametres       

        if (_container._currentFrame >= f.TaskStartFrame) {

            int doneFrameInNode = _container._currentFrame - f.TaskStartFrame;

            int timeToUndock = 100;
            int timeToMove = timeToUndock + 500;
            int timeToDock = timeToMove + 100;
            int timeToFuel = timeToDock + 50;

            string lastDoneStep = f.FleetParams.GetString("lastState", "none");
            switch (lastDoneStep) {
                case "none": { //undocking
                    //analyse des parametres en input
                    if (doneFrameInNode >= 0) {
                        int hangarID = 1;
                        foreach (LinkInfo l in data.links) {
                            if(l.ToID == n.id && l.ToParam == "Station") {
                                NodeInfos hangarSelectorNode = data.nodes[l.FromID];
                                hangarID = hangarSelectorNode.nodeParams.GetInt("HangarID");
                            }
                        }                        
                        f.FleetParams.Set("targetHangar", hangarID);

                        //undocking
                        f.ShipIDs.ForEach((sID) => {
                            Ship ship = _container._ships[sID];
                            ship.Status = "Undocking";
                            _currentUpdate.Add(ship);
                        });                        
                        f.FleetParams.Set("lastState", "undock");
                        return f.TaskStartFrame + timeToUndock;
                    } else {
                        return f.TaskStartFrame;
                    }
                }
                case "undock": {
                    if (doneFrameInNode >= timeToUndock) {
                        //undock result + moving
                        AddLog(f, "Undocked from station");

                        Hangar hangar = _container._hangars[f.LastHangar];
                        _currentUpdate.Add(hangar);

                        f.ShipIDs.ForEach((sID) => {
                            Ship ship = _container._ships[sID];
                            ship.Status = "Moving to station";
                            ship.Hangar = -1;
                            _currentUpdate.Add(ship);
                            hangar.Ships.Remove(ship.ID);
                        });

                        f.FleetParams.Set("lastState", "moving");
                        return f.TaskStartFrame + timeToMove;
                    } else {
                        return f.TaskStartFrame + timeToUndock;
                    }
                }
                case "moving": {
                    if (doneFrameInNode >= timeToMove) {
                        AddLog(f, "Moved to target station");

                        f.FleetParams.Set("lastState", "dock");
                        return f.TaskStartFrame + timeToDock;
                    } else {
                        return f.TaskStartFrame + timeToMove;
                    }
                }
                case "dock": {
                    if (doneFrameInNode >= timeToDock) {
                        //docking result
                        int hangarID = f.FleetParams.GetInt("targetHangar");
                        Hangar hangar = _container._hangars[hangarID];
                        _currentUpdate.Add(hangar);
                        f.ShipIDs.ForEach((sID) => {
                            Ship ship = _container._ships[sID];
                            ship.Status = "Docking";
                            ship.Hangar = hangarID;
                            hangar.Ships.Add(ship.ID);
                            _currentUpdate.Add(ship);
                        });
                        AddLog(f, "Docked in station");

                        //start fueling
                        f.FleetParams.Set("lastState", "fuel");
                        return f.TaskStartFrame + timeToFuel;
                    } else {
                        return f.TaskStartFrame + timeToDock;
                    }
                }
                case "fuel": {
                    if (doneFrameInNode >= timeToFuel) {
                        AddLog(f, "Fueled for Y ICU");

                        //fueling is done
                        f.ShipIDs.ForEach((sID) => {
                            Ship ship = _container._ships[sID];
                            ship.Status = "Idle in station";
                        });
                        foreach (LinkInfo l in data.links) {
                            if (l.FromID == n.id && l.FromParam == "MoveOutput") {
                                f.CurrentNode = l.ToID;
                                break;
                            }
                        }
                        f.FleetParams.Set("lastState", "none");
                        f.TaskStartFrame = f.TaskStartFrame + timeToFuel;
                        return f.TaskStartFrame;
                    }
                    return f.TaskStartFrame + timeToFuel;
                }
            }
        }
        return f.TaskStartFrame;
    }


    /*
        private int UpdateShip(Ship s) {

            if(s.CurrentTask < 0) {
                if (s.Running) {
                    ShipNextTask(s);
                } 
                if(s.CurrentTask < 0) { 
                    s.CurrentFrame = _manager.Frame;
                    return s.CurrentFrame + 10;
                }
            }

            if (s.CurrentTask < 0 || s.CurrentTask >= s.Tasks.Count)
                throw new System.Exception("current task out of range in ship " + s.ID);

            int taskID = s.Tasks[s.CurrentTask];
            if (!_container._tasks.ContainsKey(taskID))
                throw new System.Exception("task not found : " + taskID);


            Task t = _container._tasks[taskID];
            switch (t.Tasktype) {
                case Task.TaskType.TaskExplore:
                    return ProcessExploreTask(s, t);
                case Task.TaskType.TaskRelocate:
                    return ProcessRelocateTask(s, t);
                case Task.TaskType.TaskLoad:
                    return ProcessLoadTask(s, t);
            }


            throw new System.Exception("Pas de task?");
        }

        private int ProcessRelocateTask(Ship s, Task t) {
            if (!s.TaskParams.ContainsKey("state"))
                s.TaskParams.Add("state", "0");
            if (!s.TaskParams.ContainsKey("taskTime"))
                s.TaskParams.Add("taskTime", "1");

            int taskTime = int.Parse(s.TaskParams["taskTime"]);
            switch (s.TaskParams["state"]) {
                case "0": return Undocking(s,t);
                case "1": return MoveToStation(s, t);
                case "2": return Docking(s, t);
                case "3": return Fueling(s, t);
                case "4": return NextTask(s, t);
            }

            return s.CurrentFrame + taskTime;
        }

        private int ProcessLoadTask(Ship s, Task t) {
            return NextTask(s, t);
        }

        private int ProcessExploreTask(Ship s, Task t) {
            if (!s.TaskParams.ContainsKey("state")) 
                s.TaskParams.Add("state", "0");
            if (!s.TaskParams.ContainsKey("taskTime"))
                s.TaskParams.Add("taskTime", "1");

            int lastTaskTime = int.Parse(s.TaskParams["taskTime"]);

            switch (s.TaskParams["state"]) {
                case "0": { // undocking
                    if (t.GetParam("loop") == "loopAlways" || 
                        t.GetParam("loop") == "loopUntilFound") {
                        if(!s.TaskParams.ContainsKey("loop"))
                            s.TaskParams.Add("loop", "yes");
                    }
                    if (t.GetParam("loop") == "loopCount"){
                        if (!s.TaskParams.ContainsKey("loop"))
                            s.TaskParams.Add("loop", "yes");
                        if (!s.TaskParams.ContainsKey("nbLoop"))
                            s.TaskParams.Add("nbLoop", t.GetParam("loopCount"));
                    }
                    return Undocking(s, t);
                }
                case "1": { //moving to target
                    if (s.CurrentFrame + lastTaskTime <= _manager.Frame) {

                        NextState(s);
                        s.CurrentFrame += lastTaskTime;
                        int taskTime = 100;
                        s.TaskParams["taskTime"] = taskTime.ToString();
                        s.TaskParams["fuel"] = (int.Parse(s.TaskParams["fuel"]) + taskTime).ToString();

                        s.Status = "heading to sector " + (int.Parse(t.GetParam("zone")) + 1).ToString();

                        ServerUpdate update = new ServerUpdate();
                        update.ships.Add(s);
                        _manager.SendServerUpdate(update);

                        return s.CurrentFrame + taskTime;
                    }
                }
                break;
                case "2": {
                    if (s.CurrentFrame + lastTaskTime <= _manager.Frame) {


                        NextState(s);                    
                        s.CurrentFrame += lastTaskTime;

                        int taskTime = 500;
                        s.TaskParams["taskTime"] = taskTime.ToString();
                        s.TaskParams["fuel"] = (int.Parse(s.TaskParams["fuel"]) + taskTime).ToString();
                        s.Status = "Exploring";

                        ServerUpdate update = new ServerUpdate();
                        update.ships.Add(s);
                        _manager.SendServerUpdate(update);

                        return s.CurrentFrame + taskTime;
                    }
                }
                break;
                case "3": {
                    if (s.CurrentFrame + lastTaskTime <= _manager.Frame) {

                        NextState(s);
                        s.CurrentFrame += lastTaskTime;

                        int taskTime = 100;
                        s.TaskParams["taskTime"] = taskTime.ToString();
                        s.TaskParams["fuel"] = (int.Parse(s.TaskParams["fuel"]) + taskTime).ToString();
                        s.Status = "Returning to base";

                        //choisir un POI si possible
                        System.Random r = new System.Random();
                        double a = r.NextDouble();
                        int sector = int.Parse(t.GetParam("zone"));
                        List<PointOfInterest> possible = _container._POIs.Values.Where(p => p.FindProba > a && p.Sector == sector).ToList();
                        if (possible.Count > 0) {
                            int index = r.Next(possible.Count);
                            PointOfInterest found = possible[index];

                            Bookmark bookmark = new Bookmark(_container._bookmarkIDs++, index);
                            bookmark.datas = found.DatasToBookmark();

                            _container._bookmarks.Add(bookmark.ID, bookmark);


                            s.TaskParams.Add("found", found.ID.ToString());

                            //gerer le loop si besoin
                            if(t.GetParam("loop") == "loopUntilFound") {
                                if (s.TaskParams.ContainsKey("loop"))
                                    s.TaskParams.Remove("loop");
                            }

                            Corporation c = _container._corps[s.Corp];
                            SendMailRequest request = new SendMailRequest(-1, c.Owner);
                            request.Message = "You found something while exploring, congrat! \n\n" + found.Description;
                            request.Subject = "Exploration result";
                            _manager.SendRequest(request);
                        } else {
                            s.TaskParams.Add("found", "0");
                        }

                        ServerUpdate update = new ServerUpdate();
                        update.ships.Add(s);
                        _manager.SendServerUpdate(update);

                        return s.CurrentFrame + taskTime;
                    }
                }
                break;
                case "4": return Docking(s, t);
                case "5": return Fueling(s, t);
                case "6": return NextTask(s,t);
            }


            return s.CurrentFrame + lastTaskTime;
        }

        private int Undocking(Ship s, Task t) {
            int taskTime = 10;

            if (!s.Running) {
                s.CurrentFrame = _manager.Frame;
                s.TaskParams["taskTime"] = "1";
            } else {
                s.CurrentFrame += taskTime;
                NextState(s);
                s.TaskParams["taskTime"] = "10";
                Hangar h = _container._hangars[s.Hangar];
                h.Ships.Remove(s.ID);
                s.Status = "Undocking";

                if (!s.TaskParams.ContainsKey("fromStation"))
                    s.TaskParams.Add("fromStation", "0");
                s.TaskParams["fromStation"] = h.Station.ToString();
                s.TaskParams.Add("fuel", "0");

                ServerUpdate update = new ServerUpdate();
                update.ships.Add(s);
                update.hangars.Add(h);
                _manager.SendServerUpdate(update);
            }

            return s.CurrentFrame + taskTime;
        }

        private int Docking(Ship s, Task t) {

            int lastTaskTime = int.Parse(s.TaskParams["taskTime"]);

            if (s.CurrentFrame + lastTaskTime <= _manager.Frame) {

                NextState(s);
                s.CurrentFrame += lastTaskTime;

                int taskTime = 10;
                s.TaskParams["taskTime"] = taskTime.ToString();
                s.Status = "Docking";

                Hangar h = _container._hangars[t.EndHangar];

                h.Ships.Add(s.ID);
                s.Hangar = h.ID;

                ServerUpdate update = new ServerUpdate();

                //ajouter les ressources au hangar
                if (s.TaskParams.ContainsKey("found") && s.TaskParams["found"] != "0") {
                    int id = int.Parse(s.TaskParams["found"]);
                    PointOfInterest p = _container._POIs[id];
                    if (p.Datas["type"] == "mining") {
                        int rockType = int.Parse(p.Datas["rockType"]);
                        _manager._hangarManager.AddResourceToHanger(h.ID, rockType, 100);                    
                    }                
                }

                update.hangars.Add(h);
                update.ships.Add(s);            
                _manager.SendServerUpdate(update);

                return s.CurrentFrame + taskTime;
            }

            return s.CurrentFrame + lastTaskTime;
        }

        private int Fueling(Ship s, Task t) {
            int lastTaskTime = int.Parse(s.TaskParams["taskTime"]);

            if (s.CurrentFrame + lastTaskTime <= _manager.Frame) {

                s.CurrentFrame += lastTaskTime;

                s.Status = "Fueling";

                //try to fuel
                bool canFuel = false;
                int needPrice = int.Parse(s.TaskParams["fuel"]);
                Corporation corp = _container._corps[s.Corp];
                canFuel = corp.ICU >= needPrice;

                ServerUpdate update = new ServerUpdate();

                int taskTime = 0;
                if (canFuel) {

                    corp.ICU -= needPrice;
                    update.corps.Add(corp);

                    taskTime = 10;
                    NextState(s);

                } else {
                    taskTime = 1;                
                }

                s.TaskParams["taskTime"] = taskTime.ToString();

                update.ships.Add(s);
                _manager.SendServerUpdate(update);

                return s.CurrentFrame + taskTime;
            }

            return s.CurrentFrame + lastTaskTime;
        }

        private int NextTask(Ship s, Task t) {
            int lastTaskTime = int.Parse(s.TaskParams["taskTime"]);

            if (s.CurrentFrame + lastTaskTime <= _manager.Frame) {
                s.CurrentFrame += lastTaskTime;

                if (s.WillStop) {
                    Stop(s);
                    s.TaskParams.Clear();
                } else {
                    if (s.TaskParams.ContainsKey("loop")) {
                        if (s.TaskParams.ContainsKey("nbLoop")) {
                            int nbLoop = int.Parse(s.TaskParams["nbLoop"]);
                            int loopCount = 0;
                            if (s.TaskParams.ContainsKey("loopCount")) {
                                loopCount = int.Parse(s.TaskParams["loopCount"]);
                            }
                            loopCount++;
                            if (loopCount >= nbLoop) {
                                ShipNextTask(s);
                            } else { 
                                s.TaskParams.Clear();
                                s.TaskParams.Add("loopCount", loopCount.ToString());
                            }
                        } else {
                            s.TaskParams.Clear();
                        }
                    } else {
                        ShipNextTask(s);
                    }
                }

                return s.CurrentFrame;
            }

            return s.CurrentFrame + lastTaskTime;
        }

        private int MoveToStation(Ship s, Task t) {

            int lastTaskTime = int.Parse(s.TaskParams["taskTime"]);

            if (s.CurrentFrame + lastTaskTime <= _manager.Frame) {
                NextState(s);
                s.CurrentFrame += lastTaskTime;

                int fromStation = int.Parse(s.TaskParams["fromStation"]);
                int toStation = int.Parse(t.GetParam("targetStation"));

                int taskTime = 0;
                if(fromStation != toStation) {
                    List<int> path = _manager._routes.GetPath(fromStation, toStation);
                    taskTime = path.Count * 100;
                    s.TaskParams["fuel"] = (int.Parse(s.TaskParams["fuel"]) + taskTime).ToString();
                }


                s.TaskParams["taskTime"] = taskTime.ToString();

                s.Status = "En route for station";

                ServerUpdate update = new ServerUpdate();
                update.ships.Add(s);
                _manager.SendServerUpdate(update);

                return s.CurrentFrame + taskTime;
            }

            return s.CurrentFrame + lastTaskTime;
        }

        private int Stop(Ship s) {
            if (s.Hangar < 1)
                throw new System.Exception("Cannot stop ship while in space...");

            Hangar h = _container._hangars[s.Hangar];
            Station station = _container._stations[h.Station];

            s.Status = "Idle in " + station.Name;

            s.Running = false;
            s.TaskParams.Clear();

            ServerUpdate update = new ServerUpdate();
            update.ships.Add(s);
            _manager.SendServerUpdate(update);

            return s.CurrentFrame + 1;  
        }

        private void NextState(Ship s) {
            if (!s.TaskParams.ContainsKey("state"))
                s.TaskParams.Add("state", "0");
            else 
                s.TaskParams["state"] = (int.Parse(s.TaskParams["state"]) + 1).ToString();
        }

        private void ShipNextTask(Ship s) {
            if (s.CurrentTask < 0) {
                if (s.Tasks.Count > 0)
                    s.CurrentTask = 0;
            } else {
                s.CurrentTask++;
                if (s.CurrentTask >= s.Tasks.Count) {
                    s.CurrentTask = -1;
                    Stop(s);
                }
            }
            s.TaskParams.Clear();
        }
    */
}