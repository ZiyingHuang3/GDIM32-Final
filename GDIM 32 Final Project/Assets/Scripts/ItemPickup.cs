using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ItemPickup : MonoBehaviour
{
    [Header("Item Data")]
    [SerializeField] private ItemId itemId = ItemId.Mirror;

    [Header("Pickup Settings")]
    [SerializeField] private float interactDistance = 3f; 

    [Header("Optional Feedback")]
    [SerializeField] private AudioClip pickupSfx;

    private void OnMouseDown()
    {
        var player = FindObjectOfType<PlayerController>();
        if (player == null) return;

        float distance = Vector3.Distance(
            player.transform.position,
            transform.position
        );

        if (distance > interactDistance)
        {
            Debug.Log("Too far to pick up");
            return;
        }

        player.Inventory.Add(itemId);
        PlayClip(pickupSfx);
        gameObject.SetActive(false);
    }

    private void PlayClip(AudioClip clip)
    {
        if (clip == null) return;
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }
}