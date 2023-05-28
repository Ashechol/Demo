using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionTest : MonoBehaviour
{
    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        Debug.Log(_anim.humanScale);
    }

    private void OnAnimatorMove()
    {
        ;
    }
}
