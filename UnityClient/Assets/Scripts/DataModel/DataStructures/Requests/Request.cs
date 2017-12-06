
public abstract  class Request {
    public long RequestID { get; set; }

    public abstract void Update(CBHolder callbacks);
}
