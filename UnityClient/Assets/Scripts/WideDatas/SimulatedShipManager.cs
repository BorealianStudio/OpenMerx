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

public class SimulatedShipManager{

    SimulatedWideDataManager _manager = null;
    SimulatedWideDataManager.SerializeContainer _container = null;

    Dictionary<int, int> _nextChange = null;

    private ServerUpdate _currentUpdate = null;

    public SimulatedShipManager(SimulatedWideDataManager manager, SimulatedWideDataManager.SerializeContainer container) {
        _manager = manager;
        _container = container;
        _nextChange = new Dictionary<int, int>();
    }

    public void UpdateOneFrame() {
        int frame = _manager.Frame;

        List<Fleet> copy = new List<Fleet>(_container._fleets.Values);


        foreach (Fleet f in copy) {
            _currentUpdate = new ServerUpdate();

            if (!_nextChange.ContainsKey(f.ID))
                _nextChange.Add(f.ID, 0);

            if (_nextChange[f.ID] <= _container._currentFrame) {
                 _nextChange[f.ID] = UpdateFlee(f);
            }
            _manager.SendServerUpdate(_currentUpdate);
            _currentUpdate = null;
        }
    }

    private void AddLog(Fleet f, string msg) {
        throw new System.NotImplementedException();

    }

    private int UpdateFlee(Fleet f) {
        if (f.FleetParams == null) {
            f.FleetParams = new ParamHolder();
        }

        NodalEditor.SaveStruct s = Newtonsoft.Json.JsonConvert.DeserializeObject<NodalEditor.SaveStruct>(f.Data);
        if (s.nodes.ContainsKey(f.CurrentNode)) {
            ExecutableNode node = ExecutableNodeFactory.GetNode(f, s, f.CurrentNode);
            int result = node.Update(_currentUpdate);
            f.Data = Newtonsoft.Json.JsonConvert.SerializeObject(s);
            return result; 
        }
        throw new System.Exception("pas de node active???");
    }
}