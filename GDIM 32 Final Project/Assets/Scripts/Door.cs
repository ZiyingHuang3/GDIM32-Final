using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoorUnlock : MonoBehaviour

{
    public Animator doorAnimator;
    public ItemId requiredKey = ItemId.Key;
    [SerializeField] private Collider solidCollider;
    private bool opened = false;

    private void Awake()
    {
        if (doorAnimator == null)
            doorAnimator = GetComponent<Animator>();

        doorAnimator.SetBool("OpenDoor", false);
    }
    private IEnumerator DisableColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (solidCollider != null)
            solidCollider.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (opened) return;

        Player player = other.GetComponent<Player>();
        if (player == null) return;

        if (!player.Inventory.Has(requiredKey)) return;

        doorAnimator.SetBool("OpenDoor", true);
        StartCoroutine(DisableColliderAfterDelay(9f));
        if (solidCollider != null)
            solidCollider.enabled = false;
        opened = true;
    }
}

