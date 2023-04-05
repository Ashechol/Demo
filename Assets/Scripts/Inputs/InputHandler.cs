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
            Initialize();
        }

        private void Initialize()
        {
            _playerInput = GetComponent<PlayerInput>();
            if (!_playerInput) _playerInput = gameObject.AddComponent<PlayerInput>();
            
            _playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
            _playerInput.actions = actions;
            _playerInput.defaultActionMap = actions.actionMaps[0].name;
        }
    }
}
