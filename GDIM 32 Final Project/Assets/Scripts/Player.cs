using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [Header("Health")]
    [SerializeField] private int maxHealth = 5;
    //[SerializeField] private UIManager ui;
    public int CurrentHealth { get; private set; }
    

    [Header("Movement")]
    [SerializeField] private Rigidbody _playerRigidbody;
    [SerializeField] private float _turnSpeed = 1f;
    [SerializeField] private float _moveSpeed;

    private float vertical;
    private float horizontal;
    private bool _canMove = false;
    //private bool _canMove = true;

    [Header("Interact")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private LayerMask interactLayers = ~0;

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

        //if (ui == null) ui = FindObjectOfType<UIManager>();
        if (playerCamera == null) playerCamera = Camera.main;
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
        
    }
    private void FixedUpdate()
    {
        if (!_canMove) return;

        // forward/back movement 
        Vector3 movement = transform.forward * vertical * _moveSpeed * Time.fixedDeltaTime;
        _playerRigidbody.MovePosition(_playerRigidbody.position + movement);

        // rotate
        float turn = horizontal * _turnSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0, turn, 0);
        _playerRigidbody.MoveRotation(_playerRigidbody.rotation * turnRotation);
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

       // ui.SetHealth(CurrentHealth);
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

        //ui.ShowGameOver();
        GameEvents.OnPlayerDied?.Invoke();
        Debug.Log("Player Died");
    }
    

}
