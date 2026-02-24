using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCState : MonoBehaviour
{
    public enum State { Idle, Attack }
    [SerializeField] private float alertRange = 3f;
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator;
    private State state = State.Idle;
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

    private void SetState(State newState)
    {
        state = newState;
        animator.SetBool("Attack", state == State.Attack);
    }
}
