using System.Collections.Generic;
     
public class Routes {

    private Dictionary<int, Dictionary<int, float>> links = new Dictionary<int, Dictionary<int, float>>();

    public void AddRoute(int from, int to, float length) {
        if (!links.ContainsKey(from))
            links.Add(from, new Dictionary<int, float>());

        if (!links.ContainsKey(to))
            links.Add(to, new Dictionary<int, float>());

        if (!links[from].ContainsKey(to))
            links[from].Add(to, 0.0f);
        links[from][to] = length;

        if (!links[to].ContainsKey(from))
            links[to].Add(from, 0.0f);
        links[to][from] = length;
    }

    public List<int> GetPath(int from, int to) {
        List<int> path = new List<int>();
        if (from == to)
            return path;


        return null;
    }
}
