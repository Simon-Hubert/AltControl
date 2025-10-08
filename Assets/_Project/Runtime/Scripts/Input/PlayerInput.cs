using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private InputActionReference _rightAxis;
    [SerializeField] private InputActionReference _leftAxis;
    [SerializeField] private InputActionReference _rightButton;
    [SerializeField] private InputActionReference _leftButton;

    private IControllable _controllable;
    

    private void Awake() {
        _controllable = GetComponent<IControllable>();
    }

    private void OnEnable() {
        _rightAxis.action.performed += RightAxis;
        _rightAxis.action.canceled += RightAxis;
        _leftAxis.action.performed += LeftAxis;
        _leftAxis.action.canceled += LeftAxis;
        _rightButton.action.started += RightButton;
        _leftButton.action.started += LeftButton;
    }
    
    private void OnDisable() {
        _rightAxis.action.performed -= RightAxis;
        _rightAxis.action.canceled -= RightAxis;
        _leftAxis.action.performed -= LeftAxis;
        _leftAxis.action.canceled -= LeftAxis;

        _rightButton.action.started -= RightButton;
        _leftButton.action.started -= LeftButton;
    }

    
    private void RightAxis(InputAction.CallbackContext ctx ) {
        _controllable.OnRightAxis(ctx.ReadValue<float>());
    }
    
    private void LeftAxis(InputAction.CallbackContext ctx) {
        _controllable.OnLeftAxis(ctx.ReadValue<float>());
    }
    
    private void RightButton(InputAction.CallbackContext ctx) {
        _controllable.OnRightButton();
    }
    
    private void LeftButton(InputAction.CallbackContext ctx) {
        _controllable.OnLeftButton();
    }
}
