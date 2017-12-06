using System.Collections.Generic;

public class ServerUpdate {
    public List<Ship> ships = new List<Ship>();
    public List<Hangar> hangars = new List<Hangar>();
    public List<ResourceStack> stacks = new List<ResourceStack>();
    public List<Corporation> corps = new List<Corporation>();

    public void Add(Ship ship) {
        if (!ships.Contains(ship))
            ships.Add(ship);
    }

    public void Add(Hangar hangar) {
        if (!hangars.Contains(hangar))
            hangars.Add(hangar);
    }
}
