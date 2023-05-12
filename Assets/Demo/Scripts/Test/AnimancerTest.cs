using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

public class AnimancerTest : MonoBehaviour
{
    private AnimancerComponent _animancer;
    [SerializeField] private LinearMixerTransition _clip;

    [Range(-1, 1)] public float leanAmount = 0;

    private void Awake()
    {
        _animancer = GetComponent<AnimancerComponent>();
    }

    private void Start()
    {
        _animancer.Play(_clip);
    }

    private void Update()
    {
        _clip.State.Parameter = leanAmount;
    }
}
