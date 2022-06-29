using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FindAllChildren
{
    public static List<T>
    GetComponentsInChildrenRecursively<T>(
        this Transform _transform,
        List<T> _componentList
    )
    {
        foreach (Transform t in _transform)
        {
            T[] components = t.GetComponents<T>();

            foreach (T component in components)
            {
                if (component != null)
                {
                    _componentList.Add (component);
                }
            }

            GetComponentsInChildrenRecursively<T> (t, _componentList);
        }

        return _componentList;
    }
}
