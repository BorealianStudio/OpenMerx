using System.Collections.Generic;
using System.Linq;

public class SimulatedHangarManager {

    private SimulatedWideDataManager _manager = null;
    private SimulatedWideDataManager.SerializeContainer _container = null;

    public SimulatedHangarManager(SimulatedWideDataManager manager, SimulatedWideDataManager.SerializeContainer container) {
        _manager = manager;
        _container = container;
    }

    public void AddResourceToHanger(int hangarID, int type, int qte) {
        Hangar h = _container._hangars[hangarID];

        ResourceStack stack = null;
        foreach (int id in h.StacksIDs) {
            ResourceStack tmp = _container._resourceStacks[id];
            if(tmp.Type == type) {
                stack = tmp;
                break;
            }
        }
        if (null == stack) {
            stack = new ResourceStack(_container._resourceStackIDs++);
            stack.Qte = 0;
            stack.Type = type;
            stack.Loaded = true;
            _container._resourceStacks.Add(stack.ID, stack);
            h.StacksIDs.Add(stack.ID);
        }

        stack.Qte += qte;

        ServerUpdate update = new ServerUpdate();
        update.stacks.Add(stack);
        update.hangars.Add(h);
        _manager.SendServerUpdate(update);
    }

    public bool RemoveResourceToHangar(int hangarID, int resType, int qte) {
        Hangar h = _container._hangars[hangarID];
        int tmpQte = 0;
        List<ResourceStack> stacks = new List<ResourceStack>();

        foreach(int i in h.StacksIDs) {
            ResourceStack r = _container._resourceStacks[i];
            if(r.Type == resType) {
                tmpQte += r.Qte;
                stacks.Add(r);
            }
        }

        if(tmpQte >= qte) {
            while(qte > 0) {
                if(stacks[0].Qte > qte) {
                    stacks[0].Qte -= qte;
                    qte = 0;                   
                } else {
                    qte -= stacks[0].Qte;
                    _container._resourceStacks.Remove(stacks[0].ID);
                    h.StacksIDs.Remove(stacks[0].ID);
                    stacks.RemoveAt(0);
                }
            }
            return true;
        }
        return false;
    }
}
