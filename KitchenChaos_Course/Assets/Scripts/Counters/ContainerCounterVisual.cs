using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour{

    private const string OPEN_CLOSE = "OpenClose";
    [SerializeField] private ContainerCounter containerCounter;
    private Animator _animator;

    private void Awake(){
        _animator = GetComponent<Animator>();
    }

    private void Start(){
        containerCounter.onPlayerGrabbedObject += ContainerCounterOnPlayerGrabbedObject;
    }

    private void ContainerCounterOnPlayerGrabbedObject(object sender, EventArgs e){
        _animator.SetTrigger(OPEN_CLOSE);
    }
}
