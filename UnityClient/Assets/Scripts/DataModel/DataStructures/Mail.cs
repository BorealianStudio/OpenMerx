public class Mail {

    public int ID { get; private set; }

    public string Title { get; set; }
    public string Content { get; set; }

    public Mail(int id) {
        ID = id;
    }

}
