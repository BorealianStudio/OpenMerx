public class ResourceStackChangeEvent : EventHolder {

    ResourceStack _data = null;
    ResourceStackCB _toCall = null;

    public ResourceStackChangeEvent(ResourceStack data, ResourceStackCB toCall) {
        _data = data;
        _toCall = toCall;
    }

    public override void Exectute() {
        _toCall(_data);
    }
}
