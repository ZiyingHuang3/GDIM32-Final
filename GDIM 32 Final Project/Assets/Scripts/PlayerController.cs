using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class PlayerController : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private UIManager ui;
    public int CurrentHealth { get; private set; }

    [Header("Interact")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private LayerMask interactLayers = ~0;

    public Inventory Inventory { get; private set; } = new Inventory();

    private void Awake()
    {
        CurrentHealth = maxHealth;

        if (ui == null) ui = FindObjectOfType<UIManager>();
        if (playerCamera == null) playerCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryClickInteract();
        }
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

        ui.SetHealth(CurrentHealth);
        Debug.Log("Player Health: " + CurrentHealth);

        if (CurrentHealth == 0)
        {
            Debug.Log("Player Died");
            ui.ShowGameOver();
        }
    }
}

*/
