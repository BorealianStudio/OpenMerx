public class LoadCharacterRequest : Request {

    public int CharacterID{get; private set;}

    public Character Character { get; set; }
    public MailBox MailBox { get; set; }

    public LoadCharacterRequest(int charID) {
        CharacterID = charID;    
    }

    public override void Update(CBHolder callbacks) {
        callbacks.charCB(Character);
        if(null != MailBox) {
            callbacks.mailboxCB(MailBox);
        }
    }
}
