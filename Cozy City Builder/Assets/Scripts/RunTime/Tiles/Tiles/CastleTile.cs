using Managers;
using NaughtyAttributes;
using UnityEngine;


public class CastleTile : BasicTower
{
    public Transform PathPoint;
    public int CastleHealth => _currentCastleHealth;
    public int MaxCastleHealth => castleHealth;

    [HorizontalLine]
    [Header("Castle")]
    [SerializeField] private int castleHealth;
    [SerializeField] private short goldPerDay = 10;

    private int _currentCastleHealth;

    public override void Init(HexGridNode myNode)
    {
        base.Init(myNode);
        myNode.SetIsWalkable(true);
        _currentCastleHealth = castleHealth;
    }

    public void TakeDamage(short damage)
    {
        _currentCastleHealth -= damage;
        if (_currentCastleHealth < 0)
        {
            GameManager.Instance.SetState(GameStates.GAMEOVER);
            Debug.Log("Game over");
        }
        UIManager.Instance.gameCanvasManager.UpdateCastleHealthUI();
    }

    protected override void OnTurnStateChange(TurnStates state)
    {
        base.OnTurnStateChange(state);
        if (state == TurnStates.TurnBegin)
        {
            GameManager.Instance.player.AddGold(goldPerDay);
        }
    }
}

