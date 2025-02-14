using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SerializeDictionary<TKey, TValue>
{
    private List<TKey> keys;
    private List<TValue> values;

    public List<TKey> Keys => keys;
    public List<TValue> Values => values;


    public SerializeDictionary(Dictionary<TKey, TValue> dictionary)
    {
        keys = new List<TKey>();
        values = new List<TValue>();

        foreach (var kvp in dictionary)
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }
}