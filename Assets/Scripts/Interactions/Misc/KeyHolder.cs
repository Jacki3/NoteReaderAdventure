using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class KeyHolder
{
    private static List<Key.KeyType> keys = new List<Key.KeyType>();

    public static void AddKey(Key.KeyType key) => keys.Add(key);

    public static void RemoveKey(Key.KeyType key) => keys.Remove(key);

    public static bool ContainsKey(Key.KeyType key) => keys.Contains(key);
}
