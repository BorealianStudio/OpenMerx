using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class MailsView : MonoBehaviour {

    [SerializeField] Transform overviewContent = null;
    [SerializeField] MailOverviewLine overviewLinePrefab = null;

    [SerializeField] Text detailContent = null;
    [SerializeField] Button deleteButton = null;

    MailBox _mailbox = null;
    Mail _selectedMail = null;

    private void Start() {
        LocalDataManager.instance.OnMailboxChange += OnMailboxChange;
        deleteButton.gameObject.SetActive(false);
    }

    private void OnDestroy() {
        LocalDataManager.instance.OnMailboxChange -= OnMailboxChange;
    }

    public void SetMailbox(MailBox m) {
        _mailbox = m;
        OnMailboxChange(m);
    }

    private void OnMailboxChange(MailBox m) {
        if (_mailbox == null || _mailbox.ID != m.ID)
            return;

        Dictionary<int, Mail> mailsID = new Dictionary<int, Mail>();
        foreach(Mail mail in _mailbox.Mails) {
            mailsID.Add(mail.ID,mail);
        }
        foreach(Transform t in overviewContent) {
            Mail mail = t.GetComponent<MailOverviewLine>().Mail;
            if (!mailsID.ContainsKey(mail.ID)) {
                Destroy(t.gameObject);
            } else {
                mailsID.Remove(mail.ID);
            }
        }

        foreach(Mail mail in mailsID.Values) {
            MailOverviewLine line = Instantiate(overviewLinePrefab, overviewContent);
            line.SetMail(mail);
            line.OnButtonClic += OnMailSelect;
        }
        overviewContent.Sort(t => t.GetComponent<MailOverviewLine>().Mail.ID);

        OnMailSelect(null);
    }   

    private void OnMailSelect(Mail m) {
        if (m == null) {
            detailContent.text = "";
        } else {            
            detailContent.text = m.Content;
        }

        deleteButton.gameObject.SetActive(m != null);
        _selectedMail = m;
    }

    public void OnDeleteClic() {
        if (null != _selectedMail) {
            WideDataManager.Request(new DeleteMailRequest(_mailbox.ID, _selectedMail.ID));
        }
    }
    
}
