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

public class MailBox : DataObject {

    public int ID { get; private set; }

    [Newtonsoft.Json.JsonProperty]
    List<Mail> mails = new List<Mail>();

    public MailBox(int id) {
        ID = id;    
    }

    public void AddMail(Mail m) {
        mails.Add(m);
    }    
    
    public void RemoveMail(int mailID) {
        foreach(Mail m in mails) {
            if(m.ID == mailID) {
                mails.Remove(m);
                break;
            }
        }
    }

    public List<Mail> Mails { get { return new List<Mail>(mails); } }

    public override List<EventHolder> Update(DataObject obj) {
        List<EventHolder> result = new List<EventHolder>();
        MailBox m = obj as MailBox;
        if (null == m)
            return result;

        
        if (NeedToUpdate(m)) {
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(m);
            var serializerSettings = new Newtonsoft.Json.JsonSerializerSettings { ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace };
            Newtonsoft.Json.JsonConvert.PopulateObject(data, this, serializerSettings);


            result.Add(new MailboxChangeEvent(this, LocalDataManager.instance.OnMailboxChange));
        }
        return result;
    }

    private bool NeedToUpdate(MailBox other) {
        int mailCount = mails.Count;
        if (mails.Count != other.mails.Count)
            return true;

        if (other.Loaded != Loaded)
            return true;

        for(int i = 0; i < mailCount; i++) {
            if (mails[i].ID != other.mails[i].ID)
                return true;
        }

        return false;
    }
}
