using Managers;

namespace HexaFortress.Game
{
    public static class Events
    {
        public static OnPlayerBuildModeEvent OnPlayerBuildModeEvent=new ();
        public static TurnStateChangeEvent TurnStateChangeEvent=new ();
        public static GameStateChangeEvent GameStateChangeEvent=new ();
        public static CastleHealthChangeEvent CastleHealthChangeEvent=new ();
    }
    
    public class OnPlayerBuildModeEvent : IGameEvent
    {
        public bool IsBuilding;
    }

    public class TurnStateChangeEvent:IGameEvent
    {
        public TurnStates TurnState;
    }
    public class GameStateChangeEvent:IGameEvent
    {
        public GameStates GameState;
    }
    public class CastleHealthChangeEvent:IGameEvent
    {
        public int MaxHealth;
        public int CurrentHealth;
    }
}