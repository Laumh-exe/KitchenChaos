using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;
    
    private IHasProgress _hasProgress;

    private void Start(){
        _hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (_hasProgress == null) {
            Debug.LogError("Game Object " + hasProgressGameObject + " does not have an component that implements IHasProgress");
        }
        
        _hasProgress.OnProgressChanged += HasProgressOnProgressChanged;

        barImage.fillAmount = 0f;
        
        HideProgressBar();
    }

    private void HasProgressOnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e){
        barImage.fillAmount = e.progressNormalized;
        if (e.progressNormalized == 0f || e.progressNormalized == 1f) {
            HideProgressBar();
        }
        else {
            ShowProgressBar();
        }
    }

    private void HideProgressBar(){
        gameObject.SetActive(false);
    }

    private void ShowProgressBar(){
        gameObject.SetActive(true);
    }
}