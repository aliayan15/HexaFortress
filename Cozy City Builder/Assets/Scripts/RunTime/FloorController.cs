using Managers;
using UnityEngine;
using DG.Tweening;


public class FloorController : MonoBehaviour
{
    [SerializeField] private Material floorMat;
    [Space(10)]
    [SerializeField] private Color dayColor;
    [SerializeField] private Color nightColor;
    [Space(3)]
    [SerializeField] private float larpTime;


    private void Start()
    {
        floorMat.color = dayColor;
    }

    private void DayCircle(bool dayBegin)
    {
        if (dayBegin)
        {
            floorMat.DOColor(dayColor, larpTime);
        }
        else
        {
            // enemy spawn
            floorMat.DOColor(nightColor, larpTime);
        }
    }

    #region State Change
    private void OnTurnStateChange(TurnStates state)
    {
        if (state == TurnStates.TurnBegin)
            DayCircle(true);
        if (state == TurnStates.EnemySpawnStart)
            DayCircle(false);
    }
    private void OnGameStateChange(GameStates state)
    {

    }

    private void OnEnable()
    {
        GameManager.OnTurnStateChange += OnTurnStateChange;
        GameManager.OnGameStateChange += OnGameStateChange;
    }
    private void OnDisable()
    {
        GameManager.OnTurnStateChange -= OnTurnStateChange;
        GameManager.OnGameStateChange -= OnGameStateChange;
    }
    #endregion
}

