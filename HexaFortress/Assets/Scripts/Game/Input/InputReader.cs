using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace HexaFortress.Game
{
    [CreateAssetMenu(fileName = "Input Reader", menuName = "ScriptableObject/Input Reader")]
    public class InputReader : ScriptableObject
    {
        [SerializeField] private InputActionAsset inputActionAsset;

        public event UnityAction SelectEvent;
        public event UnityAction DeselectEvent;
        public event UnityAction SkipEventDown;
        public event UnityAction SkipEventUp;
        public event UnityAction<Vector2> MovementEvent;
        public event UnityAction<float> ScrollEvent;

        private InputAction _selectAction;
        private InputAction _deselectAction;
        private InputAction _skipAction;
        private InputAction _movementAction;
        private InputAction _scrollAction;

        private void OnEnable()
        {
            if (!inputActionAsset) return;

            _selectAction = inputActionAsset.FindAction("Select");
            _deselectAction = inputActionAsset.FindAction("Deselect");
            _skipAction = inputActionAsset.FindAction("Skip");
            _movementAction = inputActionAsset.FindAction("Movement");
            _scrollAction = inputActionAsset.FindAction("Scroll");

            _selectAction.performed += OnSelect;
            _deselectAction.performed += OnDeselect;
            _skipAction.performed += OnSkip;
            _skipAction.canceled += OnSkip;
            _movementAction.performed += OnMove;
            _scrollAction.performed += OnScroll;
            _movementAction.canceled += OnMove;

            _selectAction.Enable();
            _deselectAction.Enable();
            _skipAction.Enable();
            _movementAction.Enable();
            _scrollAction.Enable();
        }

        private void OnDisable()
        {
            if (!inputActionAsset) return;

            _selectAction.performed -= OnSelect;
            _deselectAction.performed -= OnDeselect;
            _skipAction.performed -= OnSkip;
            _skipAction.canceled -= OnSkip;
            _movementAction.performed -= OnMove;
            _movementAction.canceled -= OnMove;
            _scrollAction.performed -= OnScroll;

            _selectAction.Disable();
            _deselectAction.Disable();
            _skipAction.Disable();
            _movementAction.Disable();
            _scrollAction.Disable();
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            MovementEvent?.Invoke(context.ReadValue<Vector2>());
        }

        private void OnScroll(InputAction.CallbackContext context)
        {
            ScrollEvent?.Invoke(context.ReadValue<Vector2>().y);
        }

        private void OnSkip(InputAction.CallbackContext context)
        {
            if (context.performed)
                SkipEventDown?.Invoke();
            else if (context.canceled)
                SkipEventUp?.Invoke();
        }

        private void OnDeselect(InputAction.CallbackContext context)
        {
            DeselectEvent?.Invoke();
        }

        private void OnSelect(InputAction.CallbackContext context)
        {
            SelectEvent?.Invoke();
        }
    }
}