using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class StartStats : ScriptableObject
{
    public Player.Stats.Types Type;
    public int StartValue;
    public int UpgradeCost;
    public int StartCost;
}
