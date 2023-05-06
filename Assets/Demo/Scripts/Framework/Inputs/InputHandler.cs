using Demo.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System;

namespace Inputs
{
    /// Binding and process input action
    public class InputHandler : MonoBehaviour
    {
        private PlayerInput _playerInput = null;

        private InputActionAsset _actions;

        public event Action OnMoveCanceled;
        public event Action<bool> OnDash;

        #region Values

        private Vector2 _rawMoveInput;
        private Vector2 _rawLookInput;
        private bool _jumpInput;
        private bool _dashInput;

        public Vector2 MoveInput => _rawMoveInput;
        public bool IsMoveInput => _rawMoveInput != Vector2.zero;
        public float MoveInputX => _rawMoveInput.x;
        public float MoveInputY => _rawMoveInput.y;
        public float YawInput => _rawLookInput.x;
        // 处理鼠标输入不能乘以 Time.deltaTime
        /// Fixed YawInput: do not multiply delta time.
        public float YawInputFixed => IsCurrentDeviceMouse ? YawInput : YawInput * Time.deltaTime;
        public float PitchInput => _rawLookInput.y;
        /// Fixed PitchInput: do not multiply delta time.
        public float PitchInputFixed => IsCurrentDeviceMouse ? PitchInput : PitchInput * Time.deltaTime;
        public bool JumpInput
        {
            get
            {
                var tmp = _jumpInput;
                if (_jumpInput) _jumpInput = false; // Consume jump input
                return tmp;
            }
        }

        #endregion

        public bool IsCurrentDeviceMouse => _playerInput.currentControlScheme == "KeyboardMouse";

        private void Awake()
        {
            _playerInput = gameObject.GetComponentSafe<PlayerInput>();
            _actions = Resources.Load("Settings/Input/GameInputActions") as InputActionAsset;
        }

        private void OnEnable()
        {
            ActionBinding();
        }

        private void OnDisable()
        {
            ActionUnBinding();
        }

        private void Start()
        {
            _playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
            _playerInput.actions = _actions;
            _playerInput.defaultActionMap = _actions.actionMaps[0].name;
            // 设置了 defaultActionMap 必须调用一次 ActivateInput
            _playerInput.ActivateInput();
        }

        private void ActionBinding()
        {
            _playerInput.onActionTriggered += OnMoveAction;
            _playerInput.onActionTriggered += OnLookAction;
            _playerInput.onActionTriggered += OnJumpAction;
            _playerInput.onActionTriggered += OnDashAction;
        }

        private void ActionUnBinding()
        {
            _playerInput.onActionTriggered -= OnMoveAction;
            _playerInput.onActionTriggered -= OnMoveAction;
            _playerInput.onActionTriggered -= OnJumpAction;
            _playerInput.onActionTriggered -= OnDashAction;
        }

        private void OnMoveAction(InputAction.CallbackContext context)
        {
            if (context.action.name != "Move") return;
            
            _rawMoveInput = context.ReadValue<Vector2>();
            
            if (context.canceled)
                OnMoveCanceled?.Invoke();
        }

        private void OnLookAction(InputAction.CallbackContext context)
        {
            if (context.action.name != "Look") return;

            _rawLookInput = context.ReadValue<Vector2>();
        }

        private void OnJumpAction(InputAction.CallbackContext context)
        {
            if (context.action.name != "Jump") return;
            
            if (context.performed)
                _jumpInput = true;
        }

        private void OnDashAction(InputAction.CallbackContext context)
        {
            if (context.action.name != "Dash") return;
            
            OnDash?.Invoke(context.performed);
        }
        
    }
}
