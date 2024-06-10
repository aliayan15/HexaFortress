using System.Collections.Generic;
using HexaFortress.Game;
using Managers;
using MyUtilities;
using UnityEngine;
using UnityEngine.Events;

namespace HexaFortress.GamePlay
{
    public class Player : SingletonMono<Player>
    {
        [SerializeField] private UIEvents events;
        [SerializeField] private SOGameProperties gameData;
        [SerializeField] private float goNightTime = 2f;

        private bool _isSpaceHold = false;
        private float _goNightTimer = 0;

        private void Start()
        {
            GameManager.Instance.StartGame();
        }

        private void Update()
        {
            if (GameManager.Instance.GameState != GameStates.GAME)
                return;

            SkipToNightTimer();
        }

        private void SkipToNightTimer()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _goNightTimer = 0;
                SetSpaceHold(true);
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                SetSpaceHold(false);
            }

            if (!_isSpaceHold) return;
            _goNightTimer += Time.deltaTime;
            events.UpdateNightCircle.Invoke(_goNightTimer / goNightTime);

            if (!(_goNightTimer >= goNightTime)) return;
            // skip the day
            GameManager.Instance.SetTurnState(TurnStates.EnemySpawnStart);
            SetSpaceHold(false);
        }

        private void SetSpaceHold(bool isHold)
        {
            _isSpaceHold = isHold;
            events.ShowNightUI.Invoke(isHold);
        }

        #region State Change

        private void OnTurnStateChange(TurnStateChangeEvent evt)
        {
            // start turn again after a delay
            if (evt.TurnState == TurnStates.TurnEnd)
            {
                this.Timer(0.2f, () =>
                {
                    if (GameManager.Instance.GameState == GameStates.GAME)
                        GameManager.Instance.SetTurnState(TurnStates.TurnBegin);
                });
            }
            //TODO ToolTipSystem.Instance.CanShow3dWorldUI = _canBuild;
        }

        private void OnGameStateChange(GameStateChangeEvent evt)
        {
            //TODO ToolTipSystem.Instance.CanShowUI = evt.GameState == GameStates.GAME;
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

        public void PlayPartical(Vector3 pos)
        {
            var par = Instantiate(gameData.BonusPar, pos, Quaternion.identity);
            par.transform.position += Vector3.up * 0.2f;
            par.Play();
        }
    }
}