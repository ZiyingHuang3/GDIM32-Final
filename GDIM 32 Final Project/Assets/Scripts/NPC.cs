using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class NPC : MonoBehaviour
{
    public enum State { Idle, Attack }

    [Header("Detection")]
    [SerializeField] private float alertRange = 3f;
    [SerializeField] private Transform player;

    [Header("Combat")]
    [SerializeField] private int damage = 1; 
    [SerializeField] private float damageCooldown = 1f;
    private float lastDamageTime = -999f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private State state = State.Idle;
    private void Reset()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, alertRange);
    }
    private void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        SetState(State.Idle);
    }

    private void Update()
    {
        if (player == null) return;
        float d = Vector3.Distance(transform.position, player.position);

        if (d <= alertRange && state != State.Attack)
            SetState(State.Attack);
        else if (d > alertRange && state != State.Idle)
            SetState(State.Idle);
    }
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (Time.time - lastDamageTime < damageCooldown) return;
        lastDamageTime = Time.time;
       Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }
    private void SetState(State newState)
    {
        state = newState;
        animator.SetBool("Attack", state == State.Attack);
    }
}
 