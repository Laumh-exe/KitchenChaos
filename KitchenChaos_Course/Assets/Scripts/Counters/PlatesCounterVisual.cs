using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private Transform plateVisualPrefab;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private PlatesCounter platesCounter;

    private List<GameObject> _platesVisualGameObjects;

    private void Awake(){
        _platesVisualGameObjects = new List<GameObject>();
    }

    private void Start(){
        platesCounter.OnPlateSpawned += PlatesCounterOnOnPlateSpawned;
        platesCounter.OnPlateRemoved += PlatesCounterOnOnPlateRemoved; 
    }

    private void PlatesCounterOnOnPlateRemoved(object sender, EventArgs e){
        GameObject lastPlate = _platesVisualGameObjects[^1];
        _platesVisualGameObjects.Remove(lastPlate);
        Destroy(lastPlate);
    }

    private void PlatesCounterOnOnPlateSpawned(object sender, EventArgs e){
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);
        float plateOffesetY = .1f*_platesVisualGameObjects.Count;
        plateVisualTransform.localPosition = new Vector3(0,0 + plateOffesetY,0);
        _platesVisualGameObjects.Add(plateVisualTransform.gameObject);
    }
    
}
