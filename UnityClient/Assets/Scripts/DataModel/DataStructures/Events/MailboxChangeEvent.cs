public class MailboxChangeEvent : EventHolder {

    MailBox _mailBox = null;
    EventHolder.MailboxCB _toCall = null;

    public MailboxChangeEvent(MailBox mailbox, EventHolder.MailboxCB toCall) {
        _mailBox = mailbox;
        _toCall = toCall;
    }

    public override void Exectute() {
        _toCall(_mailBox);
    }
}
