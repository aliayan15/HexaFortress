using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    
public class PathTile : TileBase
{
    public override void Init(HexGridNode myNode)
    {
        base.Init(myNode);
        myNode.SetIsWalkable(true);
    }


    protected override void OnDisable()
    {
        
    }
    protected override void OnEnable()
    {

    }
}

