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

        private void Awake()
        {
            _playerInput = Functions.GetComponentSafe<PlayerInput>(gameObject);
        }

        private void Start()
        {
            _playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
            _playerInput.actions = actions;
            _playerInput.defaultActionMap = actions.actionMaps[0].name;
        }
    }
}
