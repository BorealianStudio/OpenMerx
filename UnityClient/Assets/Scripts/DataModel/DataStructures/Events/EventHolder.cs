using System.Collections.Generic;

public abstract class EventHolder{

    #region CB
    public delegate void BasicCB();
    public delegate void DataObjectCB(DataObject obj);
    public delegate void CharacterCB(Character c);
    public delegate void CorporationCB(Corporation c);
    public delegate void HangarCB(Hangar h);
    public delegate void ResourceStackCB(ResourceStack r);
    public delegate void ShipCB(Ship c);
    public delegate void StationListCB(List<Station> c);
    public delegate void StationCB(Station c);
    public delegate void RoutesCB(Routes routes);
    public delegate void MailboxCB(MailBox mailbox);
    public delegate void FlightPlanCB(FlightPlan flighPlan);
    public delegate void FleetCB(Fleet fleet);
    #endregion

    public abstract void Exectute();
}
