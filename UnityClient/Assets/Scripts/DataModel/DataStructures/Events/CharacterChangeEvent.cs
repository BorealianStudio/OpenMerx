public class CharacterChangeEvent : EventHolder {

    Character _char = null;
    EventHolder.CharacterCB _toCall = null;

    public CharacterChangeEvent(Character character, EventHolder.CharacterCB toCall) {
        _char = character;
        _toCall = toCall;
    }

    public override void Exectute() {
        _toCall(_char);
    }
}
