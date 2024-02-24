using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CastleTile : TileBase
{
    public Transform PathPoint;
    public int CastleHealth => castleHealth;

    [HorizontalLine]
    [Header("Settings")]
    [SerializeField] private int castleHealth;

    public override void Init(HexGridNode myNode)
    {
        base.Init(myNode);
        myNode.SetIsWalkable(true);
    }

    public void TakeDamage(short damage)
    {
        Debug.Log("Castle is under attack: " + damage);
    }
}

