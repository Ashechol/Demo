using Demo.Framework.Gameplay;
using DG.Tweening;
using UnityEngine;

namespace Demo.CombatSystem
{
    public class Combat : MonoBehaviour
    {
        private Character _character;
        
        public Transform weapon;
        public Transform sheathSlot;
        public Transform handSlot;

        private bool _isWeaponDrawn;

        public bool IsWeaponDrawn => _isWeaponDrawn;
        
        private void Awake()
        {
            _character = GetComponent<Character>();

            _character.anim.onWeaponDrawSheath += DrawSheathWeapon;
        }

        public void DrawSheathWeapon()
        {
            weapon.SetParent(_isWeaponDrawn ? sheathSlot : handSlot);
            weapon.localPosition = Vector3.zero;
            weapon.localRotation = Quaternion.Euler(Vector3.zero);

            weapon.DOScale(_isWeaponDrawn ? Vector3.one * 0.8f : Vector3.one, 0.3f);
            _isWeaponDrawn = !_isWeaponDrawn;
        }
    }
}