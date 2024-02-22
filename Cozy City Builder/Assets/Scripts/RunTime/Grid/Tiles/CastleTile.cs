using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    
public class CastleTile : TileBase
{
    public Transform PathPoint;

    public override void Init(HexGridNode myNode)
    {
        base.Init(myNode);
        myNode.SetIsWalkable(true);
    }
}

