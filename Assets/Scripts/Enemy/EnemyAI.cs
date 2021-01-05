using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour {

    [HideInInspector]
    public IdleState idleState;
    [HideInInspector]
    public FollowState followState;
    [HideInInspector]
    public AttackState attackState;
    [HideInInspector]
    public ReturnState returnState;
    [HideInInspector]
    public IEnemyState currentState;

    [HideInInspector]
    public NavMeshAgent navMeshAgent;
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public Animator animator;

    void Start() {
        idleState = new IdleState(this);
        followState = new FollowState(this);
        attackState = new AttackState(this);
        returnState = new ReturnState(this);

        currentState = idleState;
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();

        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update() {
        currentState.UpdateState();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            currentState.OnTriggerEnter(other);
        }
    }

    public void Reset() {
        currentState.Reset();
    }

    public void Stop() {
        navMeshAgent.enabled = false;
    }
}
