using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CuttingCounterVisual : MonoBehaviour{

    private const string CUT = "Cut";
    
    [SerializeField] private CuttingCounter cuttingCounter;
    
    private Animator _animator;

    private void Awake(){
        _animator = GetComponent<Animator>();
    }

    private void Start(){
        cuttingCounter.OnCut += CuttingCounterOnOnCut;
    }

    private void CuttingCounterOnOnCut(object sender, EventArgs e){
        if(cuttingCounter.HasRecipeWithInput(cuttingCounter.GetKitchenObject().GetKitchenObjectSO())) {
            _animator.SetTrigger(CUT);
        }
    }
    
}
