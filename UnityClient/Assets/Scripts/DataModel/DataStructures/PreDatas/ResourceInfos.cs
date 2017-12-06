
public class ResourceInfos {
    public string Name { get; set; }
    public int Type { get; set; }
    public string IconName { get; set; }
    public float Volume { get; set; }

    public ResourceInfos(string name, int type, string iconName, float volume) {
        Name = name;
        Type = type;
        IconName = iconName;
        Volume = volume;
    }
}
