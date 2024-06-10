using HexaFortress.Game;
using MyUtilities.EventChannel;
using NaughtyAttributes;
using UnityEngine;

namespace HexaFortress.GamePlay
{
    public class CastleTile : BasicTower
    {
        [Header("Refs")] public Transform PathPoint;
        [HorizontalLine] [Header("Castle")] 
        [SerializeField] private int castleHealth;
        [SerializeField] private short goldPerDay = 10;

        private int _currentCastleHealth;

        public override void Init(HexGridNode myNode)
        {
            base.Init(myNode);
            myNode.SetIsWalkable(true);
            _currentCastleHealth = castleHealth;
            GameModel.Instance.PlayerData.AddGoldPerDay(goldPerDay);
            InvokeHealthChangeEvent();
        }

        public void TakeDamage(short damage)
        {
            _currentCastleHealth -= damage;
            if (_currentCastleHealth <= 0)
            {
                GameManager.Instance.SetState(GameStates.GAMEOVER);
                //Debug.Log("Game over");
            }

            InvokeHealthChangeEvent();
        }

        public void UpgradeHealth(int bonusHealth)
        {
            castleHealth += bonusHealth;
            _currentCastleHealth += bonusHealth;
            InvokeHealthChangeEvent();
            Player.Instance.PlayPartical(transform.position);
        }

        public void RepairHealth(short repairAmount)
        {
            _currentCastleHealth += repairAmount;
            if (_currentCastleHealth > castleHealth)
                _currentCastleHealth = castleHealth;
            InvokeHealthChangeEvent();
        }

        private void InvokeHealthChangeEvent()
        {
            var evt = Events.CastleHealthChangeEvent;
            evt.MaxHealth = castleHealth;
            evt.CurrentHealth = _currentCastleHealth;
            EventManager.Broadcast(evt);
        }

        protected override void Update()
        {
        }

        protected override void OnTurnStateChange(TurnStateChangeEvent evt)
        {
            if (evt.TurnState == TurnStates.TurnBegin)
                InvokeHealthChangeEvent();
        }
    }
}