public class CorporationChangeEvent : EventHolder {

    Corporation _corp = null;
    CorporationCB _toCall = null;

    public CorporationChangeEvent(Corporation corp, CorporationCB toCall) {
        _corp = corp;
        _toCall = toCall;
    }

    public override void Exectute() {
        _toCall(_corp);
    }
}
