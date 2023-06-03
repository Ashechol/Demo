using System;
using Demo.Framework.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Demo.Framework.Input
{
    /// Binding and process input action
    public class InputHandler : MonoBehaviour
    {
        private PlayerInput _playerInput;

        private InputActionAsset _actions;

        public event Action OnMoveCanceled;
        public event Action<bool> OnDash;

        public event Action OnMovePerformed;

        #region Values

        private Vector2 _rawMoveInput;
        private Vector2 _rawLookInput;
        private bool _jumpInput;
        private bool _dashInput;
        private bool _drawSheathInput;

        public Vector2 MoveInput => _rawMoveInput;
        public bool IsMoveInput => _rawMoveInput != Vector2.zero;
        public float MoveInputX => _rawMoveInput.x;
        public float MoveInputY => _rawMoveInput.y;
        public bool DashInput => _dashInput;

        // 处理鼠标输入不能乘以 Time.deltaTime
        /// Fixed YawInput: do not multiply delta time.
        public float YawInput => IsCurrentDeviceMouse ? _rawLookInput.x : _rawLookInput.x * Time.deltaTime;
        public float PitchInput => IsCurrentDeviceMouse ? _rawLookInput.y : _rawLookInput.y * Time.deltaTime;
        
        public bool JumpInput
        {
            get
            {
                var tmp = _jumpInput;
                if (_jumpInput) _jumpInput = false; // Consume jump input
                return tmp;
            }
        }

        public bool DrawSheathInput
        {
            get
            {
                var tmp = _drawSheathInput;
                if (_drawSheathInput) _drawSheathInput = false;
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

        private void OnDestroy()
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
            
            ActionBinding();
        }

        private void ActionBinding()
        {
            _playerInput.onActionTriggered += OnMoveAction;
            _playerInput.onActionTriggered += OnLookAction;
            _playerInput.onActionTriggered += OnJumpAction;
            _playerInput.onActionTriggered += OnDashAction;
            _playerInput.onActionTriggered += OnDrawSheathAction;
        }

        private void ActionUnBinding()
        {
            _playerInput.onActionTriggered -= OnMoveAction;
            _playerInput.onActionTriggered -= OnMoveAction;
            _playerInput.onActionTriggered -= OnJumpAction;
            _playerInput.onActionTriggered -= OnDashAction;
            _playerInput.onActionTriggered -= OnDrawSheathAction;
        }

        private void OnMoveAction(InputAction.CallbackContext context)
        {
            if (context.action.name != "Move") return;
            
            _rawMoveInput = context.ReadValue<Vector2>();
            
            if (context.performed)
                OnMovePerformed?.Invoke();
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
            
            if (context.started)
                _jumpInput = true;
        }

        private void OnDashAction(InputAction.CallbackContext context)
        {
            if (context.action.name != "Dash") return;
            
            _dashInput = context.performed;
            
            OnDash?.Invoke(context.performed);
        }
        
        private void OnDrawSheathAction(InputAction.CallbackContext context)
        {
            if (context.action.name != "DrawSheath") return;
            
            if (context.started)
                _drawSheathInput = true;

            if (context.canceled)
                _drawSheathInput = false;
        }
        
    }
}
