using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[RequireComponent(typeof(Collider))]
public class ItemPickup : MonoBehaviour
{
    [Header("Item Data")]
    [SerializeField] private ItemId itemId = ItemId.Mirror;

    [Header("Optional Feedback")]
    [SerializeField] private AudioClip pickupSfx;
    [SerializeField] private AudioClip specialSfx;
    [SerializeField] private bool playSpecialOnPickup = true;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }
    private void OnMouseDown()
    {

        var player = FindObjectOfType<PlayerController>();
        if (player == null) return;

        float dist = Vector3.Distance(player.transform.position, transform.position);
        if (dist > 3f) return;
        Pickup(player);
    }

    private void Pickup(PlayerController player)
    {
        player.Inventory.Add(itemId);
        PlayClip(pickupSfx);

        if (itemId == ItemId.MusicBox && playSpecialOnPickup)
            PlayClip(specialSfx);

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
        col.isTrigger = false; 
    }
}