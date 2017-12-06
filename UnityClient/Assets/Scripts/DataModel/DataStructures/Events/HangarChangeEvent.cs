public class HangarChangeEvent : EventHolder {

    Hangar _data = null;
    HangarCB _toCall = null;

    public HangarChangeEvent(Hangar data, HangarCB toCall) {
        _data = data;
        _toCall = toCall;
    }

    public override void Exectute() {
        _toCall(_data);
    }
}
