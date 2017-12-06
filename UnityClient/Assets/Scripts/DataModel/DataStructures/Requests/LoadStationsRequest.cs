using System;
using System.Collections.Generic;

public class LoadStationsRequest : Request {

    public List<Station> Stations { get; set; }

    public LoadStationsRequest() {
    }

    public override void Update(CBHolder callbacks) {
        foreach (Station s in Stations) {
            callbacks.stationCB(s);
        }
    }
}
