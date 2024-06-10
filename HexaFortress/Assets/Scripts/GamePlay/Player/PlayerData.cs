using HexaFortress.Game;
using NaughtyAttributes;
using UnityEngine;

namespace HexaFortress.GamePlay
{
    public class PlayerData : MonoBehaviour
    {
        [SerializeField] private UIEvents events;
        [field: HorizontalLine]
        [field: SerializeField] public int MyGold { get; private set; }
        public int GoldPerDay { get; private set; }
        public int ExpensesPerDay { get; private set; }
        public int DayCount { get; private set; } = 1;

        private void Awake()
        {
            GameModel.Instance.PlayerData = this;
        }

        #region Gold

        public void AddGold(int amount)
        {
            MyGold += amount;
            if (MyGold < 0)
                MyGold = 0;
            events.OnGoldChange.Invoke(MyGold);
        }

        public void AddGoldPerDay(int amount)
        {
            GoldPerDay += amount;
            events.OnGoldIncomeChange.Invoke(GoldPerDay,ExpensesPerDay);
        }

        public void AddExpensesPerDay(int amount)
        {
            ExpensesPerDay += amount;
            events.OnGoldIncomeChange.Invoke(GoldPerDay,ExpensesPerDay);
        }

        #endregion

        #region State Change
        private void OnTurnStateChange(TurnStateChangeEvent evt)
        {
            if (evt.TurnState == TurnStates.TurnBegin)
            {
                AddGold(GoldPerDay);
                AddGold(-ExpensesPerDay);
            }

            if (evt.TurnState == TurnStates.TurnEnd)
            {
                DayCount++;
                events.OnDayChange.Invoke(DayCount);
            }
        }

        private void OnGameStateChange(GameStateChangeEvent evt)
        {
            if (evt.GameState == GameStates.GAME)
                DayCount = 1;
        }

        private void OnEnable()
        {
            EventManager.AddListener<TurnStateChangeEvent>(OnTurnStateChange);
            EventManager.AddListener<GameStateChangeEvent>(OnGameStateChange);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<TurnStateChangeEvent>(OnTurnStateChange);
            EventManager.RemoveListener<GameStateChangeEvent>(OnGameStateChange);
        }

        #endregion
    }
}