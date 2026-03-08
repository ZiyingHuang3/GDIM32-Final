using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoorUnlock : MonoBehaviour

{
    public Animator doorAnimator;
    public ItemId requiredKey = ItemId.Key;
    [SerializeField] private Collider solidCollider;
    [SerializeField] private Collider ExitTrigger;
    [SerializeField] private GameObject GameWin;
    private Player player;
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

        player = other.GetComponent<Player>();
        if (player == null) return;

        if (!player.Inventory.Has(requiredKey)) return;

        doorAnimator.SetBool("OpenDoor", true);
        StartCoroutine(DisableColliderAfterDelay(9f));
        if (solidCollider != null)
            solidCollider.enabled = false;
        opened = true;
        
    }
    private void Update()
    {
        if (!opened || player == null) return;

        if (player.GetComponent<Collider>().bounds.Intersects(ExitTrigger.bounds))
        {
            GameWin.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}

