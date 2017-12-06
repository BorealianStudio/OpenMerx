
public class DeleteMailRequest : Request {

    public int MailboxID { get; private set; }
    public int MailID { get; private set; }

    public MailBox Mailbox { get; set; }

    public DeleteMailRequest(int mailboxID, int mailID) {
        MailboxID = mailboxID;
        MailID = mailID;
    }

    public override void Update(CBHolder callbacks) {
        callbacks.mailboxCB(Mailbox);
    }
}
