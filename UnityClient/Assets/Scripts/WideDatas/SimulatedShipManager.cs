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