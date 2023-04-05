using System;
using System.Collections;
using System.Collections.Generic;
using Inputs;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _controller;
    private InputHandler _input;

    private void Awake()
    {
        _controller = Functions.GetComponentSafe<CharacterController>(gameObject);
        _input = Functions.GetComponentSafe<InputHandler>(gameObject);
    }

    private void Update()
    {
        
    }
}
