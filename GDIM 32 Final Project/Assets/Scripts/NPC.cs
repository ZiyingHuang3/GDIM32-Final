using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class NPC : MonoBehaviour
{
   [SerializeField] private int damage = 1; 
   [SerializeField] private float damageCooldown = 1f;
    private float lastDamageTime = -999f;
    private void Reset()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (Time.time - lastDamageTime < damageCooldown) return;
        lastDamageTime = Time.time;
       PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }
}
 