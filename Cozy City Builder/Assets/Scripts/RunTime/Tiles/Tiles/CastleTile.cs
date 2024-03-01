using Managers;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CastleTile : BasicTower
{
    public Transform PathPoint;
    public int CastleHealth => castleHealth;

    [HorizontalLine]
    [Header("Castle")]
    [SerializeField] private int castleHealth;
    [SerializeField] private short goldPerDay = 10;

    public override void Init(HexGridNode myNode)
    {
        base.Init(myNode);
        myNode.SetIsWalkable(true);
    }

    public void TakeDamage(short damage)
    {
        castleHealth -= damage;
        if (castleHealth < 0)
        {
            GameManager.Instance.SetState(GameStates.GAMEOVER);
            Debug.Log("Game over");
        }
    }

    protected override void OnTurnStateChange(TurnStates state)
    {
        base.OnTurnStateChange(state);
        if(state==TurnStates.TurnBegin)
        {

        }
    }
}

