using System.Collections.Generic;

public class ResourceDataManager{

    private Dictionary<int, ResourceInfos> _infos = new Dictionary<int, ResourceInfos>();

    public ResourceDataManager() {
        _infos.Add(1, new ResourceInfos("Uraninite", 1, "tobernite", 1.0f));
        _infos.Add(2, new ResourceInfos("Tobernite", 2, "tobernite", 1.0f));
        _infos.Add(3, new ResourceInfos("Tennantite", 3, "tobernite", 1.0f));
        _infos.Add(4, new ResourceInfos("Liebigite", 4, "tobernite", 1.0f));
        _infos.Add(5, new ResourceInfos("Quartz", 5, "tobernite", 1.0f));
        _infos.Add(6, new ResourceInfos("Bauxite", 6, "tobernite", 1.0f));
        _infos.Add(7, new ResourceInfos("Graphite", 7, "tobernite", 1.0f));
        _infos.Add(8, new ResourceInfos("Glace", 8, "tobernite", 1.0f));
    }

    public ResourceInfos GetResourceInfos(int resourceType) {
        if (_infos.ContainsKey(resourceType)) {
            return _infos[resourceType];
        }
        return null;
    }

    public List<ResourceInfos> GetAllResources() {
        List<ResourceInfos> result = new List<ResourceInfos>(_infos.Values);
        return result;
    }
}
