using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour{
    
    [SerializeField] private GameObject stoveOnGameObject;
    [SerializeField] private GameObject particlesGameObject;
    [SerializeField] private StoveCounter stoveCounter;

    private void Start() {
        stoveCounter.OnStateChanged += StoveCounterOnOnStateChanged;
    }

    private void StoveCounterOnOnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e){
        switch (e.State) {
            case StoveCounter.State.Idle:
                HideParticles();
                HideStoveOn();
                break;
            case StoveCounter.State.Frying:
                ShowParticles();
                ShowStoveOn();
                break;
            case StoveCounter.State.Fried:
                break;
            case StoveCounter.State.Burned:
                HideParticles();
                break;
        }
    }


    private void ShowParticles(){
        particlesGameObject.SetActive(true);
    }
    
    private void ShowStoveOn(){
        stoveOnGameObject.SetActive(true);
    }
    
    private void HideParticles(){
        particlesGameObject.SetActive(false);
    }
    
    private void HideStoveOn(){
        stoveOnGameObject.SetActive(false);
    }
}
