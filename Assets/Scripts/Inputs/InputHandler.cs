using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs
{
    /// <summary>
    /// Binding and process input action
    /// </summary>
    public class InputHandler : MonoBehaviour
    {
        private PlayerInput _playerInput;

        public InputActionAsset actions;
        
        #region Values

        private Vector2 _rawMoveInput;
        
        #endregion

        private void Awake()
        {
            _playerInput = Functions.GetComponentSafe<PlayerInput>(gameObject);
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
            _playerInput.actions = actions;
            _playerInput.defaultActionMap = actions.actionMaps[0].name;
        }

        private void ActionBinding()
        {
            _playerInput.onActionTriggered += OnMoveAction;
        }

        private void ActionUnBinding()
        {
            _playerInput.onActionTriggered -= OnMoveAction;
        }

        private void OnMoveAction(InputAction.CallbackContext context)
        {
            if (context.action.name != "Move") return;

            _rawMoveInput = context.ReadValue<Vector2>();
            Debug.Log(_rawMoveInput);
        }
        
    }
}
