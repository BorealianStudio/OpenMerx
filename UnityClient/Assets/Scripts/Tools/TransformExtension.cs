using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public static class TransformExtension {

    public static void Sort(this Transform t, System.Func<Transform, int> function) {

        List<Transform> childs = new List<Transform>();
        foreach(Transform c in t) {
            childs.Add(c);
        }

        int i = 0;      
        foreach(Transform c in childs.OrderBy(function)) {
            c.SetSiblingIndex(i++);
        }
    }
}
