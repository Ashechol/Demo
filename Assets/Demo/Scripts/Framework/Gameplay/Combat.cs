using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Demo
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
        }

        public void DrawWeapon()
        {
            weapon.SetParent(handSlot);
            weapon.position = Vector3.zero;
            weapon.rotation = Quaternion.Euler(Vector3.zero);

            weapon.DOScale(Vector3.one, 0.67f);
        }

        public void SheathWeapon()
        {
            weapon.SetParent(sheathSlot);
            weapon.position = Vector3.zero;
            weapon.rotation = Quaternion.Euler(Vector3.zero);
            
            weapon.DOScale(Vector3.one * 0.8f, 0.67f);
        }
    }
}
