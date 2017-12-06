public class StationChangeEvent : EventHolder {

    Station _station = null;
    StationCB _toCall = null;

    public StationChangeEvent(Station station, StationCB toCall) {
        _station = station;
        _toCall = toCall;
    }

    public override void Exectute() {
        _toCall(_station);
    }
}
