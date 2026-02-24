using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{   [Header("Health")]
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private UIManager ui;
    public int CurrentHealth { get; private set; }
    private void Awake() 
    {
        CurrentHealth = maxHealth;

        if (ui == null)
            ui = FindObjectOfType<UIManager>();
    }
    public Inventory Inventory { get; private set; } = new Inventory();
    private IInteractable currentInteractable;
    public void TakeDamage(int amount)
    {
        if (CurrentHealth <= 0) return;

        CurrentHealth -= amount;

        if (CurrentHealth < 0)
            CurrentHealth = 0;

        ui.SetHealth(CurrentHealth);
        Debug.Log("Player Health: " + CurrentHealth);

        if (CurrentHealth == 0)
        {
            Debug.Log("Player Died");
            ui.ShowGameOver();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && currentInteractable != null)
        {
            currentInteractable.Interact(this);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            currentInteractable = interactable;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            if (currentInteractable == interactable)
                currentInteractable = null;
        }
    }
}
