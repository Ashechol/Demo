using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Demo.Framework.Gameplay
{
    public class Combat : MonoBehaviour
    {
        private Character _character;
        
        public Transform weapon;
        public Transform sheathSlot;
        public Transform handSlot;

        private void Awake()
        {
            _character = GetComponent<Character>();

            _character.anim.weaponCallback += DrawWeapon;
        }

        public void DrawWeapon()
        {
            weapon.SetParent(handSlot);
            weapon.localPosition = Vector3.zero;
            weapon.localRotation = Quaternion.Euler(Vector3.zero);

            weapon.GetChild(0).DOScale(Vector3.one, 0.67f);
        }

        public void SheathWeapon()
        {
            weapon.SetParent(sheathSlot);
            // weapon.position = Vector3.zero;
            // weapon.rotation = Quaternion.Euler(Vector3.zero);
            
            weapon.DOScale(Vector3.one * 0.8f, 0.67f);
        }
    }
}
