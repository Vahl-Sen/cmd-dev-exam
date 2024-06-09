using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    #region Actions
    private PlayerInput _playerInput;
    private List<InputAction> _actions = new List<InputAction>();

    private InputAction _movementAction;
    private InputAction _jumpAction;
    private InputAction _interactAction;
    #endregion

    #region Input Variables
    public Vector2 Movement { get; private set; }
    public bool JumpPress { get; private set; }
    public bool JumpRelease { get; private set; }

    public float JumpStart { get; private set; }
    public float JumpDuration { get; private set; }

    public bool Interact { get; private set; }
    #endregion

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        _movementAction = _playerInput.actions["Movement"];
        _actions.Add(_movementAction);

        _jumpAction = _playerInput.actions["Jump"];
        _actions.Add(_jumpAction);

        _interactAction = _playerInput.actions["Interact"];
        _actions.Add(_interactAction);

        _movementAction.performed += OnMovement;
        _movementAction.canceled += OnMovement;

        _jumpAction.started += OnJump;
        _jumpAction.performed += OnJump;
        _jumpAction.canceled += OnJump;

        _interactAction.started += OnInteract;
        _interactAction.canceled += OnInteract;
    }

    private void OnEnable()
    {
        foreach(InputAction action in _actions)
        {
            action.Enable();
        }
    }

    private void OnDisable()
    {
        DisableControls();
    }

    public void DisableControls()
    {
        foreach (InputAction action in _actions)
        {
            action.Disable();
        }
    }

    private void OnMovement(InputAction.CallbackContext context)
    {
        Movement = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpPress = true;
            JumpRelease = false;
            JumpStart = Time.time;
        }

        if (context.performed)
        {
            JumpPress = false;
        }

        if (context.canceled)
        {
            JumpPress = false;
            JumpRelease = true;
            JumpDuration = Time.time - JumpStart;
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
            Interact = true;

        if (context.canceled)
            Interact = false;
    }
}
