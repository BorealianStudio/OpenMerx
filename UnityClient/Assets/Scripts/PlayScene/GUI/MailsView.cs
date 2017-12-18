// MIT License

// Copyright(c) 2017 Andre Plourde
// part of https://github.com/BorealianStudio/OpenMerx

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.


// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

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
