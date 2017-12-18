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
