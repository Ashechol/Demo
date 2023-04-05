using Palmmedia.ReportGenerator.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Settings", fileName = "PlayerSettings")]
    public class PlayerSettings : ScriptableObject
    {
        [Header("Input Settings")]
        public InputActionAsset actions;
        public string defaultMap;
    }
}
