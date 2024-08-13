using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCounter : MonoBehaviour, IKitchenObjectParent{
    
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject _kitchenObject;

    public virtual void Interact(Player player){
        
    }
    public virtual void InteractAlternate(Player player){
        
    }
    
    public Transform GetKitchenObjectFollowTransform(){
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject){
        this._kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject(){
        return this._kitchenObject;
    }

    public void ClearKitchenObject(){
        this._kitchenObject = null;
    }

    public bool HasKitchenObject(){
        return _kitchenObject;
    }
}
