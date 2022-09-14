using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Player : MonoBehaviour
{
    [Header("Camera and Player")]
    [SerializeField] private new Camera camera;
    [SerializeField] private Vector3 cameraPositionOffset;
    [SerializeField] private Vector3 playerPosition;
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
    private CharacterController _controller;
    
    [Header("Head Bobbing")]
    [SerializeField] private bool headBobbing;
    [SerializeField] private float walkBobbingSpeed;
    [SerializeField] private float walkBobbingAmount;
    [SerializeField] private float sprintBobbingSpeed;
    [SerializeField] private float sprintBobbingAmount;
    private float defaultY;
    private float timer;
    

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
    private Transform _interactionHoldArea;
    
    [Header("Inventory")]
    public Inventory inventory;

    [Header("Debug")]
    [SerializeField] private Vector2 cameraRotation; 
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isJumping;
    [SerializeField] public bool isSprinting;
    [SerializeField] private bool secondJump;
    [SerializeField] private bool interactable;
    [SerializeField] private bool objectHeld;
    [SerializeField] private GameObject interactableObject;
    [SerializeField] private bool isInteractable;
    [SerializeField] private bool isHoldable;
    [SerializeField] private SaveData saveData;
    private PlayerProperties _properties;
    public PlayerInput input;
    
    public static Player Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        input = GetComponent<PlayerInput>();
    }

    private void Load()
    {
        inventory.Load(GameManager.Instance.currentSaveName, GameManager.Instance.currentSavePath);
        _controller.enabled = false;
        transform.position = saveData.playerPosition;
        transform.rotation = saveData.playerRotation;
        cameraRotation = saveData.playerCameraRotation;
        _controller.enabled = true;
    }

    public void Save()
    {
        saveData.playerPosition = transform.position;
        saveData.playerRotation = transform.rotation;
        saveData.playerCameraRotation = cameraRotation;
    }

    private void Start()
    {
        saveData = SaveSystem.Instance.saveData;
        _properties = PlayerProperties.Instance;
        _controller = GetComponent<CharacterController>();
        _interactionHoldArea = camera.transform.Find("HoldArea");
        _interactionHoldArea.localPosition = objectHoldArea;
        //objectHoldArea = _interactionHoldArea.position;
        currentSpeed = speed;
        
        if(!SaveSystem.Instance.newGame)
            Load();
    }

    private void FixedUpdate()
    {
        CameraFollow();
        MovePlayer();
        HeadBobbing();
    }

    private void Update()
    {
        // Character Controller isgrounded is funky, using raycast makes player grounded when in trigger collider. using controller isgrounded in update is a workaround.
        isGrounded = Physics.Raycast(transform.position, -Vector3.up, _controller.bounds.extents.y + 0.1f);
        Gravity();
        HoldObject();
        CenterHeldObject();
        if (!isSprinting && _properties.tempTrigger)
        {
            _properties.tempTrigger = false;
            StartCoroutine(_properties.RegenerateStamina());
        }
        
        var cameraTransform = camera.transform;
        //Debug.DrawRay(camera.transform.position, camera.transform.forward * interactDistance, Color.red);
        Physics.Raycast(cameraTransform.position, cameraTransform.forward, out _hit, interactDistance);
    }

    public void GetAxis(InputAction.CallbackContext context)
    {
        _move =  context.ReadValue<Vector2>();
    }
    
    public void Jump(InputAction.CallbackContext context)
    {
        isJumping = context.ReadValue<float>() > 0;
    }
    
    public void Sprint(InputAction.CallbackContext context)
    {
        isSprinting = holdToSprint ? context.performed : !isSprinting;
    }
    
    public void Interact(InputAction.CallbackContext context)
    {
        switch (context.performed)
        {
            // Action when pressed
            case true when interactable && context.interaction is PressInteraction:
                InteractWithObject();
                break;
            // Action when held
            case true when interactable && context.interaction is HoldInteraction && isHoldable :
                _objectRigidbody = interactableObject.GetComponent<Rigidbody>();
                HoldInteraction();
                break;
        }
    }

    private void MovePlayer()
    {
        // Sprinting
        currentSpeed = isSprinting && PlayerProperties.Instance.stamina > 0 ? currentSpeed = sprintSpeed : currentSpeed = speed;
        // Smooth Movement (smoothing input values, movement is the same)
        if (smoothMovement)
            _smoothMove = _move.magnitude > 0 ? Vector2.SmoothDamp(Vector2.zero, _move, ref _smoothSpeed, smoothTime, 1f, Time.deltaTime) : Vector2.SmoothDamp(_move, Vector2.zero, ref _smoothSpeed, smoothTime, 1f, Time.deltaTime);
        // move player in direction (choose movement type: smooth or not) (transform player direction to world space) with specified speed
        _controller.Move(transform.TransformDirection(smoothMovement ? new Vector3(_smoothMove.x, 0, _smoothMove.y) * 10f : new Vector3(_move.x, 0, _move.y) / 10f) * (currentSpeed * Time.deltaTime));
    }

    private void HeadBobbing()
    {
        if(!headBobbing)
            return;
        if(!isGrounded)
            return;
        if(Mathf.Abs(_move.x) > 0.1 || Mathf.Abs(_move.y) > 0.1)
        {
            timer += Time.deltaTime * (isSprinting ? sprintBobbingSpeed : walkBobbingSpeed);
            camera.transform.localPosition = new Vector3(camera.transform.localPosition.x, defaultY + Mathf.Sin(timer) * (isSprinting ? sprintBobbingAmount : walkBobbingAmount), camera.transform.localPosition.z);
        }
        else
        {
            timer = 0;
            camera.transform.localPosition = new Vector3(camera.transform.localPosition.x, defaultY, camera.transform.localPosition.z);
        }
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
        transform.Rotate(invertMouseY
            ? new Vector3(0, -look.x * Time.deltaTime * mouseSensitivity, 0)
            : new Vector3(0, look.x * Time.deltaTime * mouseSensitivity, 0));

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
        camera.transform.position = transform.position + cameraPositionOffset;
        defaultY = camera.transform.localPosition.y;
    }

    private void HoldObject()
    {
        if (objectHeld) 
            return;

        if(_hit.collider)
        {
            interactable = true;
            interactableObject = _hit.collider.gameObject;
            isHoldable = interactableObject.GetComponent<Rigidbody>();
        }
        else
        {
            interactable = false;
            interactableObject = null;
            isHoldable = false;
        }
    }

    private void HoldInteraction()
    {
        if (!objectHeld)
        {
            interactableObject.transform.parent = _interactionHoldArea;
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
        if(Vector3.Distance(interactableObject.transform.position, _interactionHoldArea.position) > 0.1f)
            _objectRigidbody.AddForce((_interactionHoldArea.position - interactableObject.transform.position) * objectHoldForce);
        if(Vector3.Distance(interactableObject.transform.position, _interactionHoldArea.position) > 10f)
            interactableObject.transform.position = _interactionHoldArea.position;
    }
    
    public GameObject GetRaycastObject()
    {
        return interactableObject;
    }
    
    private void CollectItems()
    {
        var item = interactableObject.GetComponent<ItemObject>();
        if (!item)
            return;
        inventory.AddItem(item.item, item.amount);
        Destroy(interactableObject);
    }
    
    private void InteractWithObject()
    {
        if (interactableObject == null)
            return;
        if(interactableObject.GetComponent<ItemObject>())
            CollectItems();
        if (interactableObject.GetComponent<InteractableObject>())
            interactableObject.GetComponent<InteractableObject>().Interact();
    }
}
