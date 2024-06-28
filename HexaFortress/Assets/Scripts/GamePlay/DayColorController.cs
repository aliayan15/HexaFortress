using DG.Tweening;
using HexaFortress.Game;
using NaughtyAttributes;
using UnityEngine;

namespace HexaFortress.GamePlay
{
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
                // night
                floorMat.DOColor(nightColor, lerpTime);
                myLight.DOColor(lightNightColor, lerpTime);
            }
        }

        #region State Change
        private void OnTurnStateChange(TurnStateChangeEvent evt)
        {
            if (evt.TurnState == TurnStates.TurnBegin)
                DayCircle(true);
            if (evt.TurnState == TurnStates.EnemySpawnStart)
                DayCircle(false);
        }
        private void OnEnable()
        {
            EventManager.AddListener<TurnStateChangeEvent>(OnTurnStateChange);
        }
        private void OnDisable()
        {
            EventManager.RemoveListener<TurnStateChangeEvent>(OnTurnStateChange);
        }
        #endregion
    }
}

