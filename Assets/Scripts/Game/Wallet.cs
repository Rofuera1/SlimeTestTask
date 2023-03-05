using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    public static Value Gems;
    public static Value Keys;

    public const int GemsPerKilledEnemy = 5;

    public static void Init(int gemsVal, int keysVal)
    {
        Gems = new Value(gemsVal, "GEMS");
        Keys = new Value(keysVal, "KEYS");
    }
}

public class Value
{
    private string _key;
    private int _val { get { return PlayerPrefs.GetInt("VALOF_" + _key, 0); } set { PlayerPrefs.SetInt("VALOF_" + _key, value); } }
    public int Val => _val;
    public UnityEngine.Events.UnityEvent OnChanged = new UnityEngine.Events.UnityEvent();

    public bool TryToRemove(int val)
    {
        if (_val < val)
            return false;

        _val -= val;
        OnChanged?.Invoke();
        return true;
    }
    public void Add(int val)
    {
        _val += val;
        OnChanged?.Invoke();
    }

    public Value(int val, string key)
    {
        _val = val;
        _key = key;
    }
}
