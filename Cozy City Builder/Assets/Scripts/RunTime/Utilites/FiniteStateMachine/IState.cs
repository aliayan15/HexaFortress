

namespace MyUtilities.FSM
{
    /// <summary>
    /// State inteface for FSM
    /// </summary>
    public interface IState
    {
        public void Tick(); // Update call every frame

        public void OnEnter(); // On state enter

        public void OnExit(); // On state exit
    }
}

