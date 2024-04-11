using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[CreateAssetMenu(menuName = "ScriptableObjects/Upgrade Data")]
public class SOUpgradeData : ScriptableObject
{
    public TileType TileType;
    public UpgradeType Upgrade;
}

