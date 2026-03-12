using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [Header("Health")]
    [SerializeField] private int maxHealth = 5;
    public int CurrentHealth { get; private set; }
    

    [Header("Movement")]
    [SerializeField] private Rigidbody _playerRigidbody;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jump;
    [Header("Hand Visual")]
    [SerializeField] private Transform handSocket;        
    [SerializeField] private ItemId keyItemId;   
    [SerializeField] private GameObject keyPrefabInHand;
    private GameObject keyInstance;
    private float vertical;
    private float horizontal;
    private bool _canMove = false;
    private bool _isGrounded;
   

    [Header("Interact")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private LayerMask interactLayers = ~0;

    [Header("Camera")]
    [SerializeField] private float mouseSensitivity = 100f;
    private float xRotation = 0f;
    public Inventory Inventory { get; private set; } = new Inventory();

    private void Awake()
    {
        //singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        //
        CurrentHealth = maxHealth;

        if (playerCamera == null) playerCamera = Camera.main;

        //camera 
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnEnable()
    {
        GameEvents.OnGameStarted += EnableMovement;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStarted -= EnableMovement;
    }

    private void EnableMovement()
    {
        _canMove = true;
    }
    public void SetCanMove(bool value)
{
    _canMove = value;
    if (!value)
    {
        vertical = 0f;
        horizontal = 0f;
    }
}

public bool CanMove => _canMove;
    void Update()
    {

        //movement
        if (!_canMove) return; 
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
 
        //interact
        
         if (Input.GetMouseButtonDown(0))
        {
            TryClickInteract();
            
        }

        //jump
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _playerRigidbody.AddForce(Vector3.up * _jump, ForceMode.Impulse);
            _isGrounded = false;
        }


    }
    private void FixedUpdate()
    {
        if (!_canMove) return;

        //movement with camera
         Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;
        Vector3 movement = moveDirection * _moveSpeed * Time.fixedDeltaTime;
        _playerRigidbody.MovePosition(_playerRigidbody.position + movement);

    }

    private void LateUpdate()
    {
        
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        Quaternion rot = Quaternion.Euler(0f, mouseX, 0f);
        _playerRigidbody.MoveRotation(_playerRigidbody.rotation * rot);
        
    }
    
    private void TryClickInteract()
    {
        if (playerCamera == null) return;
    

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayers, QueryTriggerInteraction.Ignore))
        {
            var interactable = hit.collider.GetComponentInParent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact(this);
            }
        }
    }
    public void TakeDamage(int amount)
    {
        if (CurrentHealth <= 0) return;

        CurrentHealth -= amount;
        if (CurrentHealth < 0) CurrentHealth = 0;

        GameEvents.OnHealthChanged?.Invoke(CurrentHealth);
        Debug.Log("Player Health: " + CurrentHealth);

        if (CurrentHealth == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _canMove = false;
        vertical = 0f;
        horizontal = 0f;

        GameEvents.OnPlayerDied?.Invoke();
        Debug.Log("Player Died");
    }
    public void ForceGameOver()
    {
        _canMove = false;
        vertical = 0f;
        horizontal = 0f;

        GameEvents.OnPlayerDied?.Invoke();
        Debug.Log("Forced Game Over");
    }
    
   public void RefreshKeyInHand(Inventory inv)
{
    bool hasKey = inv != null && inv.Has(keyItemId);

    if (hasKey && keyInstance == null)
    {
        keyInstance = Instantiate(keyPrefabInHand, handSocket);
        keyInstance.SetActive(true); 

        keyInstance.transform.localPosition = Vector3.zero;
        keyInstance.transform.localRotation = Quaternion.identity;
    }
    else if (!hasKey && keyInstance != null)
    {
        Destroy(keyInstance);
        keyInstance = null;
    }
}
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
    }
}

