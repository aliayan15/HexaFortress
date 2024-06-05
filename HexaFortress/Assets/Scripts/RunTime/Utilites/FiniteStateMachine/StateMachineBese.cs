using System;
using System.Collections.Generic;

namespace MyUtilities.FSM
{

    public class StateMachineBese
    {
        private Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type, List<Transition>>();
        private List<Transition> _currentTranstions = new List<Transition>();
        private List<Transition> _anyTransitions = new List<Transition>();
        private static List<Transition> EmtyTransitions = new List<Transition>();

        private IState _currentState;

        /// <summary>
        /// Update call
        /// </summary>
        public void Tick()
        {
            var transition = GetTransition();
            if (transition != null) SetState(transition.To);

            _currentState?.Tick();
        }
        /// <summary>
        /// Set current state
        /// </summary>
        /// <param name="state"></param>
        public void SetState(IState state)
        {
            if (_currentState == state) return;

            _currentState?.OnExit();
            _currentState = state;

            _transitions.TryGetValue(_currentState.GetType(), out _currentTranstions);
            if (_currentTranstions == null) _currentTranstions = EmtyTransitions;

            _currentState?.OnEnter();
        }
        /// <summary>
        /// Add transition to state machine
        /// </summary>
        /// <param name="from">From state</param>
        /// <param name="to">To state</param>
        /// <param name="condition">Condition to translate.</param>
        public void AddTransition(IState from, IState to, Func<bool> condition)
        {
            if (_transitions.TryGetValue(from.GetType(), out var transition) == false)
            {
                transition = new List<Transition>();
                _transitions[from.GetType()] = transition;
            }

            transition.Add(new Transition(to, condition));
        }
        /// <summary>
        /// Add transition from any state.
        /// </summary>
        /// <param name="state">To state</param>
        /// <param name="condition">Condition to translate.</param>
        public void AddAnyTransition(IState state, Func<bool> condition)
        {
            _anyTransitions.Add(new Transition(state, condition));
        }


        private class Transition
        {
            public IState To { get; }
            public Func<bool> Condition { get; }

            public Transition(IState to, Func<bool> condition)
            {
                this.To = to;
                this.Condition = condition;
            }
        }

        private Transition GetTransition()
        {
            for (int i = 0; i < _anyTransitions.Count; i++)
            {
                if (_anyTransitions[i].Condition()) return _anyTransitions[i];
            }

            for (int i = 0; i < _currentTranstions.Count; i++)
            {
                if (_currentTranstions[i].Condition()) return _currentTranstions[i];
            }

            return null;
        }
    }
}

