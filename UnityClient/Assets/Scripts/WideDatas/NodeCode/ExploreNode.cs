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

using System;
using System.Collections.Generic;
using System.Linq;

public class ExploreNode : ExecutableNode, IBookmarkParam{

    public ExploreNode(Fleet f, NodalEditor.SaveStruct nodes, int nodeIndex, SimulatedWideDataManager.SerializeContainer data) : base(f,nodes,nodeIndex,data) {
    }

    public Bookmark GetBookmark(string paramName) {
        NodeInfos n = _nodes.nodes[_myID];
        int id = n.nodeParams.GetInt("found", -1);
        if(id != -1) {
            return _data._bookmarks[id];
        }
        return null;
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

                //choisir un POI si possible
                bool didFound = false;
                Random r = new Random();
                double a = r.NextDouble();
                NodeInfos n = _nodes.nodes[_myID];
                int sector = n.nodeParams.GetInt("zone",0);
                List<PointOfInterest> possible = _data._POIs.Values.Where(p => p.FindProba > a && p.Sector == sector).ToList();
                if (possible.Count > 0) {
                    didFound = true;
                    int index = r.Next(possible.Count);
                    PointOfInterest found = possible[index];

                    Bookmark bookmark = new Bookmark(_data._bookmarkIDs++, index);
                    bookmark.datas = found.DatasToBookmark();
                    _data._bookmarks.Add(bookmark.ID, bookmark);

                    n.nodeParams.Set("found", bookmark.ID);

/*
                    Corporation c = _container._corps[s.Corp];
                    SendMailRequest request = new SendMailRequest(-1, c.Owner);
                    request.Message = "You found something while exploring, congrat! \n\n" + found.Description;
                    request.Subject = "Exploration result";
                    _manager.SendRequest(request);
*/
                }

                foreach (int sID in _fleet.ShipIDs) {
                    Ship ship = _data._ships[sID];
                    ship.Status = "Moving back to station";
                    ship.AddLog("Done with exploration");
                    if (didFound) {
                        ship.AddLog("And found something...");
                    }
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
