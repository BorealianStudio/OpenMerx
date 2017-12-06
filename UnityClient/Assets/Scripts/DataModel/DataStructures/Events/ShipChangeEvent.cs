public class ShipChangeEvent : EventHolder {

    Ship _ship = null;
    EventHolder.ShipCB _toCall = null;

    public ShipChangeEvent(Ship ship, EventHolder.ShipCB toCall) {
        _ship = ship;
        _toCall = toCall;
    }

    public override void Exectute() {
        _toCall(_ship);
    }
}
