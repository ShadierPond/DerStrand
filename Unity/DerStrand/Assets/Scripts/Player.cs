using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Player : MonoBehaviour
{
    
    [Header("Camera and Player Rotation")]
    [SerializeField] private new Camera camera;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private bool invertMouseX;
    [SerializeField] private bool invertMouseY;
    
    [Header("Movement")]
    [SerializeField] private bool smoothMovement;
    [SerializeField] private float speed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private bool holdToSprint;
    [SerializeField] private float smoothTime;
    [SerializeField] private float currentSpeed;
    private Vector2 _smoothSpeed;
    private Vector2 _move;
    private Vector2 _smoothMove;
    private Vector3 _moveDirection;
    private CharacterController _controller;

    [Header("Gravity and Jump")]
    [SerializeField] private bool doubleJump;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;
    private Vector3 _jumpVelocity;
    
    [Header("Interaction")]
    [SerializeField] private float interactDistance;

    [SerializeField] private Vector3 objectHoldArea;
    [SerializeField] private float objectHoldForce;
    private RaycastHit _hit;
    private Rigidbody _objectRigidbody;
    
    [Header("Debug")]
    [SerializeField] private Vector2 cameraRotation; 
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isSprinting;
    [SerializeField] private bool secondJump;
    [SerializeField] private bool interactable;
    [SerializeField] private bool objectHeld;
    [SerializeField] private GameObject interactableObject;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        currentSpeed = speed;
    }

    private void FixedUpdate()
    {
        CameraFollow();
        MovePlayer();
    }

    private void Update()
    {
        // Character Controller isgrounded is funky, using raycast makes player grounded when in trigger collider. using controller isgrounded in update is a workaround.
        isGrounded = Physics.Raycast(transform.position, -Vector3.up, _controller.bounds.extents.y + 0.1f);
        Gravity();
        Interact();
        CenterHeldObject();
    }

    public void GetAxis(InputAction.CallbackContext context)
    {
        _move = context.ReadValue<Vector2>();
    }
    
    public void Jump(InputAction.CallbackContext context)
    {
        isJumping = context.ReadValue<float>() > 0;
    }
    
    public void Sprint(InputAction.CallbackContext context)
    {
        if(holdToSprint)
            isSprinting = context.performed;
        else
            isSprinting = !isSprinting;
    }
    
    public void Interact(InputAction.CallbackContext context)
    {
        // Action when perssed
        if (context.performed && interactable && context.interaction is PressInteraction)
        {
            interactableObject.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
        }
        // Action when held
        else if (context.performed && interactable && context.interaction is HoldInteraction && interactableObject.GetComponent<Rigidbody>() != null)
        {
            _objectRigidbody = interactableObject.GetComponent<Rigidbody>();
            HoldInteraction();
        }
    }

    private void MovePlayer()
    {
        // Sprinting
        currentSpeed = isSprinting ? currentSpeed = sprintSpeed : currentSpeed = speed;
        // Smooth Movement (smoothing input values, movement is the same)
        if (smoothMovement)
            _smoothMove = _move.magnitude > 0 ? Vector2.SmoothDamp(Vector2.zero, _move, ref _smoothSpeed, smoothTime, 1f, Time.deltaTime) : Vector2.SmoothDamp(_move, Vector2.zero, ref _smoothSpeed, smoothTime, 1f, Time.deltaTime);
        // get player direction (choose movement type: smooth or not)
        _moveDirection = smoothMovement ? new Vector3(_smoothMove.x, 0, _smoothMove.y) * 10f : new Vector3(_move.x, 0, _move.y) / 10f;
        // transform player direction to world space
        _moveDirection = transform.TransformDirection(_moveDirection);
        // move player in direction with specified speed
        _controller.Move(_moveDirection * (currentSpeed * Time.deltaTime));
    }
    
    private void Gravity()
    {
        // set jumping velocity to zero if player is on ground, else apply gravity on jumping velocity
        if (isGrounded)
        {
            secondJump = false;
            _jumpVelocity = Vector3.zero;
        }
        else
            _jumpVelocity.y -= gravity * Time.deltaTime;

        // apply jump force if player is jumping
        if (isJumping)
        {
            if (isGrounded)
                _jumpVelocity.y = jumpForce;
            else if (doubleJump && !secondJump)
            {
                _jumpVelocity.y = jumpForce;
                secondJump = true;
            }
            isJumping = false;
        }
        // Move player with jump velocity (used only for jumping)
        _controller.Move(_jumpVelocity * Time.deltaTime);
    }
    
    public void Look (InputAction.CallbackContext context)
    {
        var look = context.ReadValue<Vector2>();
        
        // Player Rotation in the Y axis (X and Z are fixed)
        if(invertMouseY)
            transform.Rotate(new Vector3(0, -look.x * Time.deltaTime * mouseSensitivity, 0));
        else
            transform.Rotate(new Vector3(0, look.x * Time.deltaTime * mouseSensitivity, 0));
        
        // Camera rotation in the X and Y Axis (with inverted possibility)
        if(invertMouseY)
            cameraRotation.y -= look.x * Time.deltaTime * mouseSensitivity;
        else
            cameraRotation.y += look.x * Time.deltaTime * mouseSensitivity;
        if(invertMouseX)
            cameraRotation.x += look.y * Time.deltaTime * mouseSensitivity;
        else
            cameraRotation.x -= look.y * Time.deltaTime * mouseSensitivity;
        // Limit vertical rotation to prevent the camera from flipping
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, -90f, 90f);
        // Apply the rotation to the camera
        camera.transform.eulerAngles = cameraRotation;
    }
    
    private void CameraFollow()
    {
        // Move the camera to the player position
        camera.transform.position = transform.position;
    }

    private void Interact()
    {
        if (objectHeld) 
            return;
        Debug.DrawRay(camera.transform.position, camera.transform.forward * interactDistance, Color.red);
        Physics.Raycast(camera.transform.position, camera.transform.forward, out _hit, interactDistance);
        if(_hit.collider != null)
        {
            interactable = true;
            interactableObject = _hit.collider.gameObject;
        }
        else
        {
            interactable = false;
            interactableObject = null;
        }
    }

    private void HoldInteraction()
    {
        if (!objectHeld)
        {
            interactableObject.transform.parent = camera.transform;
            _objectRigidbody.useGravity = false;
            _objectRigidbody.drag = 10;
            _objectRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            interactableObject.transform.parent = null;
            _objectRigidbody.useGravity = true;
            _objectRigidbody.drag = 1;
            _objectRigidbody.constraints = RigidbodyConstraints.None;
        }
        objectHeld = !objectHeld;
    }

    private void CenterHeldObject()
    {
        if(!objectHeld)
            return;
        if(Vector3.Distance(interactableObject.transform.position, camera.transform.position + objectHoldArea) > 0.1f)
        {
            _objectRigidbody.AddForce((camera.transform.position + objectHoldArea - interactableObject.transform.position) * objectHoldForce);
        }
    }
}
