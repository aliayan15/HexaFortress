using HexaFortress.Game;
using MyUtilities;
using UnityEngine;

namespace HexaFortress.GamePlay
{
    public class Player : SingletonMono<Player>
    {
        [SerializeField] private InputReader inputReader;
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
            if (!_isSpaceHold) return;
            _goNightTimer += Time.deltaTime;
            events.UpdateNightCircle.Invoke(_goNightTimer / goNightTime);

            if (!(_goNightTimer >= goNightTime)) return;
            // skip the day
            GameManager.Instance.SetTurnState(TurnStates.EnemySpawnStart);
            SetSkipKeyHold(false);
        }

        private void OnSkipKeyUp()
        {
            SetSkipKeyHold(false);
        }
        private void OnSkipKeyDown()
        {
            _goNightTimer = 0;
            SetSkipKeyHold(true);
        }
        private void SetSkipKeyHold(bool isHold)
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
        }

        private void OnGameStateChange(GameStateChangeEvent state)
        {
            events.ShowToolTipUI.Invoke(state.GameState == GameStates.GAME);
        }

        private void OnEnable()
        {
            EventManager.AddListener<TurnStateChangeEvent>(OnTurnStateChange);
            EventManager.AddListener<GameStateChangeEvent>(OnGameStateChange);
            inputReader.SkipEventDown += OnSkipKeyDown;
            inputReader.SkipEventUp += OnSkipKeyUp;
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<TurnStateChangeEvent>(OnTurnStateChange);
            EventManager.RemoveListener<GameStateChangeEvent>(OnGameStateChange);
            inputReader.SkipEventDown -= OnSkipKeyDown;
            inputReader.SkipEventUp -= OnSkipKeyUp;
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