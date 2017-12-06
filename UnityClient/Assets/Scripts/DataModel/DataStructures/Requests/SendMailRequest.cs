
public class SendMailRequest : Request {

    public int SenderID { get; private set; }
    public int DestID { get; private set; }

    public string Subject { get; set; }
    public string Message { get; set; }

    public MailBox MailBox { get; set; }

    public SendMailRequest(int senderID, int receiverID) {
        SenderID = senderID;
        DestID = receiverID;
    }

    public override void Update(CBHolder callbacks) {
        callbacks.mailboxCB(MailBox);
    }
}
