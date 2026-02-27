using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class NPC : MonoBehaviour
{
    public enum State { Idle, Attack }

    [Header("Detection")]
    [SerializeField] private Transform alertOrigin;
    [SerializeField] private float alertRange = 3f;
    [SerializeField, Range(0f, 180f)] private float viewAngle = 90f;
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
        Vector3 origin = alertOrigin ? alertOrigin.position : transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin, alertRange);
        Vector3 leftDir = Quaternion.Euler(0, -viewAngle * 0.5f, 0) * transform.forward;
        Vector3 rightDir = Quaternion.Euler(0, viewAngle * 0.5f, 0) * transform.forward;
        Gizmos.DrawRay(origin, leftDir * alertRange);
        Gizmos.DrawRay(origin, rightDir * alertRange);
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
        Vector3 origin = alertOrigin ? alertOrigin.position : transform.position;
        Vector3 toPlayer = player.position - origin;
        float dist = toPlayer.magnitude;

        bool inRange = dist <= alertRange;

        bool inFOV = false;
        if (inRange && dist > 0.001f)
        {
            float angle = Vector3.Angle(transform.forward, toPlayer);
            inFOV = angle <= viewAngle * 0.5f;
        }

        if (inRange && inFOV && state != State.Attack)
            SetState(State.Attack);
        else if ((!inRange || !inFOV) && state != State.Idle)
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
 