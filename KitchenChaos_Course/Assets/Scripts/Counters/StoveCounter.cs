using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public class OnStateChangedEventArgs : EventArgs{
        public State State;
    }

    public enum State{
        Idle,
        Frying,
        Fried,
        Burned
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSoArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSoArray;
    [SerializeField] private ProgressBarUI progressBarUI;

    private State _state;
    private float _burnedTimer;
    private float _fryingTimer;
    private FryingRecipeSO _fryingRecipeSO;
    private BurningRecipeSO _burningRecipeSO;


    public override void Interact(Player player){
        if (!HasKitchenObject()) {
            if (player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                player.GetKitchenObject().SetKitchenObjectParent(this);

                _fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                _state = State.Frying;
                _fryingTimer = 0f;

                OnOnStateChanged(_state);
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                    progressNormalized = (float)_fryingTimer / _fryingRecipeSO.fryingTimerMax
                });
                
            }
        }
        else {
            if (!player.HasKitchenObject()) {
                GetKitchenObject().SetKitchenObjectParent(player);
                
                _state = State.Idle;
                OnOnStateChanged(_state);
            }
        }
    }

    private void Start(){
        _state = State.Idle;
        OnOnStateChanged(_state);
    }

    private void Update(){
        if (HasKitchenObject()) {
            switch (_state) {
                case State.Idle:
                    break;
                case State.Frying:
                    _fryingTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = (float)_fryingTimer / _fryingRecipeSO.fryingTimerMax
                    });
                    if (_fryingTimer > _fryingRecipeSO.fryingTimerMax) {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(_fryingRecipeSO.output, this);

                        _burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                        _burnedTimer = 0f;
                        _state = State.Fried;

                        OnOnStateChanged(_state);
                        
                    }

                    break;
                case State.Fried:
                    _burnedTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = (float)_burnedTimer / _burningRecipeSO.burningTimerMax
                    });
                    if (_burnedTimer > _burningRecipeSO.burningTimerMax) {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(_burningRecipeSO.output, this);

                        _state = State.Burned;

                        OnOnStateChanged(_state);
                        
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                            progressNormalized = 0f
                        });
                    }

                    break;
                case State.Burned:
                    break;
            }
        }
    }

    public override void InteractAlternate(Player player){
        /*if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) {
            _fryingProgress++;

            OnCut?.Invoke(this, EventArgs.Empty);
            FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs {
                progressNormalized = (float)_fryingProgress / fryingRecipeSO.fryingTimerMax
            });

            if (_fryingProgress >= fryingRecipeSO.fryingTimerMax) {
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
        }*/
    }

    public bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO){
        return GetFryingRecipeSOWithInput(inputKitchenObjectSO) != null;
    }


    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO){
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null) {
            return fryingRecipeSO.output;
        }

        return null;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO){
        foreach (var fryingRecipeSO in fryingRecipeSoArray) {
            if (fryingRecipeSO.input == inputKitchenObjectSO) {
                return fryingRecipeSO;
            }
        }

        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO){
        foreach (var burningRecipeSO in burningRecipeSoArray) {
            if (burningRecipeSO.input == inputKitchenObjectSO) {
                return burningRecipeSO;
            }
        }

        return null;
    }

    private
        protected virtual void OnOnStateChanged(State state){
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
            State = state
        });
    }

}