using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour, IKitchenObjectParent{
    public static Player Instance{ get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs{
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private bool _isWalking;
    private Vector3 _lastInteractDirection;
    private BaseCounter _selectedCounter;
    private KitchenObject _kitchenObject;

    private void Awake(){
        if (Instance != null) {
            Debug.LogError("There is more than one Player instance");
        }
        else {
            Instance = this;
        }
    }

    private void Start(){
        gameInput.OnInteractAction += GameInputOnOnInteractAction;
        gameInput.OnInteractAlternateAction += GameInputOnOnInteractAlternateAction;
    }

    private void GameInputOnOnInteractAlternateAction(object sender, EventArgs e){
        if (_selectedCounter != null) {
            _selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInputOnOnInteractAction(object sender, EventArgs e){
        if (_selectedCounter != null) {
            _selectedCounter.Interact(this);
        }
    }

    private void Update(){
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking(){
        return _isWalking;
    }

    private void HandleMovement(){
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirection, moveDistance);

        if (!canMove) { //Check if cant move in current direction
            Vector3 moveDirectionX = new Vector3(moveDirection.x, 0, 0).normalized;
            canMove = moveDirection.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionX, moveDistance);

            if (canMove) { //If can move in X -> only move X
                moveDirection = moveDirectionX;
            }
            else { //If cant move -> Check if can move in Z
                Vector3 moveDirectionZ = new Vector3(0, 0, moveDirection.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionZ, moveDistance);

                if (canMove) { //If can move in Z -> only move Z
                    moveDirection = moveDirectionZ;
                }
            }
        }

        if (canMove) { //If couldnt move before movedirection is either only x or z
            transform.position += moveDirection * moveDistance;
        }

        _isWalking = moveDirection != Vector3.zero;

        const float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }

    private void HandleInteractions(){
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDirection != Vector3.zero) {
            _lastInteractDirection = moveDirection;
        }

        const float interactDistance = 2f;

        bool hit = Physics.Raycast(transform.position, _lastInteractDirection, out RaycastHit raycastHit, interactDistance, countersLayerMask);
        if (hit) {
            if (raycastHit.transform.TryGetComponent(out BaseCounter clearCounter)) {
                if (clearCounter != _selectedCounter) {
                    SetSelectedCounter(clearCounter);
                }
            }
            else {
                SetSelectedCounter(null);
            }
        }
        else {
            SetSelectedCounter(null);
        }
    }

    private void SetSelectedCounter(BaseCounter selectedCounter){
        this._selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform(){
        return kitchenObjectHoldPoint;
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