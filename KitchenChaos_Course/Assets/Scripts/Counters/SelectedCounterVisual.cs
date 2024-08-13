using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class SelectedCounterVisual : MonoBehaviour{
    
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjects;
    
    private void Start(){
        Player.Instance.OnSelectedCounterChanged += PlayerOnSelectedCounterChanged;
    }

    private void PlayerOnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e){
        if (e.selectedCounter == baseCounter) {
            Show();
        }
        else {
            Hide();
        }
    }

    private void Show(){
        foreach (var gameObject in visualGameObjects) {
            gameObject.SetActive(true);
        }
    }

    private void Hide(){
        foreach (var gameObject in visualGameObjects) {
            gameObject.SetActive(false);
        }
    }
}