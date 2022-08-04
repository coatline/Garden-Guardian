using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Getter
{
    Dictionary<string, Object> dict;
    Object[] ts;

    public int Length { get; private set; }

    public Getter(Object[] ts)
    {
        dict = new Dictionary<string, Object>();

        this.ts = ts;
        Length = ts.Length;

        for (int i = 0; i < ts.Length; i++)
        {
            if (ts[i] == null) { Debug.Log("Null Data!"); continue; }
            dict.Add(ts[i].name, ts[i]);
        }
    }


    /// <summary>
    /// Use: 'as (TYPE)' to convert to type
    /// </summary>
    public Object this[string n]
    {
        get
        {
            dict.TryGetValue(n, out Object j);
            if (!j) { Debug.LogError($"Couldn't get {n}"); }
            return j;
        }
    }

    /// <summary>
    /// Use: 'as (TYPE)' to convert to type
    /// </summary>
    public Object this[int n]
    {
        get
        {
            return ts[n];
        }
    }

    /// <summary>
    /// Use: 'as (TYPE)' to convert to type
    /// </summary>
    /// <returns>A random Object</returns>
    public Object GetRandomObject()
    {
        return ts[Random.Range(0, ts.Length)];
    }
}
