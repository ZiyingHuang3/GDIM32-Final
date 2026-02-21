using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class ItemPickup3D : MonoBehaviour, IInteractable
{
    [Header("Item Data")]
    [SerializeField] private ItemId itemId = ItemId.Mirror;

    [Header("Optional Feedback")]
    [SerializeField] private AudioClip pickupSfx;
    [SerializeField] private AudioClip specialSfx;      
    [SerializeField] private bool playSpecialOnPickup = true;
public string GetHint()
    {
        return $"Press F to pick up {itemId}";
    }
public void Interact(PlayerController player)
    {
        if (player == null) return;
        player.Inventory.Add(itemId);
        switch (itemId)
        {
            case ItemId.MusicBox:
                if (playSpecialOnPickup) PlayClip(specialSfx);
                break;
        }
        gameObject.SetActive(false);
    }
    private void PlayClip(AudioClip clip)
    {
        if (clip == null) return;
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }
    private void Reset()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }
}
