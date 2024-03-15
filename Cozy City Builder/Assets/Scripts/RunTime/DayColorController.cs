using Managers;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;


public class DayColorController : MonoBehaviour
{

    [Space(10)]
    [SerializeField] private float lerpTime;
    [HorizontalLine]
    [Header("Floor")]
    [SerializeField] private Color dayColor;
    [SerializeField] private Color nightColor;
    [SerializeField] private Material floorMat;
    [HorizontalLine]
    [Header("Floor")]
    [SerializeField] private Color lightDayColor;
    [SerializeField] private Color lightNightColor;
    [SerializeField] private Light myLight;


    private void Start()
    {
        floorMat.color = dayColor;
    }

    private void DayCircle(bool dayBegin)
    {
        if (dayBegin)
        {
            floorMat.DOColor(dayColor, lerpTime);
            myLight.DOColor(lightDayColor, lerpTime);
        }
        else
        {
            // enemy spawn
            floorMat.DOColor(nightColor, lerpTime);
            myLight.DOColor(lightNightColor, lerpTime);
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

