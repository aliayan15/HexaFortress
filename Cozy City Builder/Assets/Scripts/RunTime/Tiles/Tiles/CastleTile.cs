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
        GameManager.Instance.player.AddGoldPerDay(goldPerDay);
    }

    public void TakeDamage(short damage)
    {
        _currentCastleHealth -= damage;
        if (_currentCastleHealth <= 0)
        {
            GameManager.Instance.SetState(GameStates.GAMEOVER);
            //Debug.Log("Game over");
        }
        UIManager.Instance.gameCanvasManager.UpdateCastleHealthUI();
    }

    public void UpgradeHealth(int bonusHealth)
    {
        castleHealth += bonusHealth;
        _currentCastleHealth += bonusHealth;
        UIManager.Instance.gameCanvasManager.UpdateCastleHealthUI();
        GameManager.Instance.player.PlayPartical(transform.position);
    }
    public void RepairHealth(short repairAmount)
    {
        _currentCastleHealth += repairAmount;
        if (_currentCastleHealth > castleHealth)
            _currentCastleHealth = castleHealth;
        UIManager.Instance.gameCanvasManager.UpdateCastleHealthUI();
    }

    protected override void OnTurnStateChange(TurnStates state)
    {
        base.OnTurnStateChange(state);
    }


}

