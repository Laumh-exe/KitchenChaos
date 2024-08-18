using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlatesCounter : BaseCounter{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;
    
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    
    private float _spawnPlateTimer;
    private const float SpawnInterval = 5f;
    private int _plateSpawnAmount;
    private int _plateSpawnAmountMax = 5;

    public void Start(){
        _plateSpawnAmount = 0;
    }

    public void Update(){
        _spawnPlateTimer += Time.deltaTime;
        if (_spawnPlateTimer > SpawnInterval) {
            if (_plateSpawnAmount < _plateSpawnAmountMax) {
                _plateSpawnAmount++;
                
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
            _spawnPlateTimer = 0;
        }
    }
    
    public override void Interact(Player player){
        if(!player.HasKitchenObject() && _plateSpawnAmount > 0) {
            KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
            _plateSpawnAmount--;
            
            OnPlateRemoved?.Invoke(this,EventArgs.Empty);
        }
    }
}
