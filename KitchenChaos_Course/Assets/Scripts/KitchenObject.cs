using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class KitchenObject : MonoBehaviour{
    [SerializeField] private KitchenObjectSO kitchenObjectSo;

    private IKitchenObjectParent _kitchenObjectParent;
    
    public KitchenObjectSO GetKitchenObjectSO(){
        return kitchenObjectSo;
    }

    public IKitchenObjectParent GetKitchenObjectParent(){
        return _kitchenObjectParent;
    }

    public void DestroySelf(){
        _kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent){
        if (this._kitchenObjectParent != null) {
            this._kitchenObjectParent.ClearKitchenObject();
        }
        this._kitchenObjectParent = kitchenObjectParent;

        if (kitchenObjectParent.HasKitchenObject()) {
            Debug.LogError("IKitchenObjectParent already has a KitchenObject");
        }
        
        _kitchenObjectParent.SetKitchenObject(this);
        
        transform.parent = _kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSo, IKitchenObjectParent kitchenObjectParent){
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSo.prefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);

        return kitchenObject;
    } 
    
}
