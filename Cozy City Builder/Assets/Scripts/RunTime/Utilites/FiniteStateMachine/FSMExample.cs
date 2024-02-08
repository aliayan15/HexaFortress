using System;
using UnityEngine;
using UnityEngine.AI;

namespace MyUtilities.FSM
{
    /*
    public class Target
    {
        internal Vector3 position;
        internal bool IsDepleted;
    }

    public class FSMExample : MonoBehaviour
    {

        private Animator animator;
        private NavMeshAgent navMeshAgent;
        private StateMachineBese _stateMachine;
        private Target target;
        private float TimeStuck;
        private int _gathered;
        private int _maxCarried;

        private void Start()
        {
            //*** create state machine ***
            _stateMachine = new StateMachineBese();

            //*** Create states ***
            var search = new SearchForResource(this);
            var moveToSelected = new MoveToSelectedResource(this, navMeshAgent, animator);
            var harvest = new HarvestResource(this, animator);

            // *** Transitions using at metot below ***
            At(moveToSelected, search, StuckForOverASecond());
            At(moveToSelected, harvest, ReachedResource());
            At(harvest, search, TargetIsDepletedAndICanCarryMore());

            _stateMachine.AddAnyTransition(search, ReachedResource());

            // *** Set Start State
            _stateMachine.SetState(search);

            // *** Some bool functions ***
            //*** Conditions for transition ***

            Func<bool> HasTarget() => () => target != null;
            Func<bool> StuckForOverASecond() => () => TimeStuck > 1f;
            Func<bool> ReachedResource() => () => target != null &&
                                                  Vector3.Distance(transform.position, target.position) < 1f;

            Func<bool> TargetIsDepletedAndICanCarryMore() => () => (target == null || target.IsDepleted) && !InventoryFull().Invoke();
            Func<bool> InventoryFull() => () => _gathered >= _maxCarried;

        }

        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

        // *** Update ***
        private void Update() => _stateMachine.Tick();


    }

    // *** State Class Example ***
    internal class HarvestResource : IState
    {
        private FSMExample fSMExample;
        private Animator animator;

        public HarvestResource(FSMExample fSMExample, Animator animator)
        {
            this.fSMExample = fSMExample;
            this.animator = animator;
        }

        public void OnEnter()
        {
            throw new NotImplementedException();
        }

        public void OnExit()
        {
            throw new NotImplementedException();
        }

        public void Tick()
        {
            throw new NotImplementedException();
        }
    }

    public class MoveToSelectedResource : IState
    {
        private FSMExample fSMExample;
        private NavMeshAgent navMeshAgent;
        private Animator animator;

        public MoveToSelectedResource(FSMExample fSMExample, NavMeshAgent navMeshAgent, Animator animator)
        {
            this.fSMExample = fSMExample;
            this.navMeshAgent = navMeshAgent;
            this.animator = animator;
        }

        public void OnEnter()
        {

        }

        public void OnExit()
        {

        }

        public void Tick()
        {

        }
    }

    public class SearchForResource : IState
    {
        private readonly FSMExample _fsmExample;

        public SearchForResource(FSMExample fsmExample)
        {
            _fsmExample = fsmExample;
        }
        public void Tick()
        {
            // Do some thing...
        }


        public void OnEnter() { }
        public void OnExit() { }
    }
    */

}
