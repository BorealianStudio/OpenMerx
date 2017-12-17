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
