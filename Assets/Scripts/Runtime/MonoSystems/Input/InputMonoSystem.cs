using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace DOH.MonoSystem
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputMonoSystem : MonoBehaviour, IInputMonoSystem
    {
        [SerializeField] private PlayerInput _input;

        private InputAction _moveAction;
        private InputAction _lookAction;
        private InputAction _jumpAction;
        private InputAction _interactAction;
        private InputAction _rAction;

        public UnityEvent JumpAction { get; private set; }
        public UnityEvent InteractCallback { get; private set; }
        public UnityEvent RCallback { get; private set; }

        public Vector2 RawMovement { get; private set; }
        public Vector2 RawLook { get; private set; }

        private void HandleMoveAction(InputAction.CallbackContext e)
        {
            RawMovement = e.ReadValue<Vector2>();
        }

        private void HandleLookAction(InputAction.CallbackContext e)
        {
            RawLook = e.ReadValue<Vector2>();
        }

        private void HandleInteractAction(InputAction.CallbackContext e)
        {
            InteractCallback.Invoke();
        }

        private void HandleRAction(InputAction.CallbackContext e)
        {
            RCallback.Invoke();
        }

        private void HandleJumpAction(InputAction.CallbackContext e)
        {
            JumpAction.Invoke();
        }

        private void Awake()
        {
            if (!_input) _input = GetComponent<PlayerInput>();

            JumpAction       = new UnityEvent();
            InteractCallback = new UnityEvent();
            RCallback = new UnityEvent();

            _moveAction       = _input.actions["Move"];
            _lookAction       = _input.actions["Look"];
            _jumpAction       = _input.actions["Jump"];
            _interactAction   = _input.actions["Interact"];
            _rAction = _input.actions["R"];

            _moveAction.performed       += HandleMoveAction;
            _lookAction.performed       += HandleLookAction;
            _jumpAction.performed       += HandleJumpAction;
            _interactAction.performed   += HandleInteractAction;
            _rAction.performed += HandleRAction;
        }

        private void OnDestroy()
        {
            _moveAction.performed       -= HandleMoveAction;
            _lookAction.performed       -= HandleLookAction;
            _jumpAction.performed       -= HandleJumpAction;
            _interactAction.performed   -= HandleInteractAction;
            _rAction.performed -= HandleRAction;
        }
    }
}
