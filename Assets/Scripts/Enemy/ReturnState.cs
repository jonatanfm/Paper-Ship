using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnState : IEnemyState {

    EnemyAI enemy;
    private Vector3 initPosition;

    public ReturnState(EnemyAI enemy) {
        this.enemy = enemy;
        initPosition = enemy.transform.position;
    }

    public void UpdateState() {
        enemy.navMeshAgent.destination = initPosition;
        if (enemy.navMeshAgent.remainingDistance < 0.1f)
            GoToIdleState();
        else if (Vector3.Distance(enemy.transform.position, enemy.player.transform.position) < 10f)
            GoToFollowState();
    }

    public void GoToFollowState() { }

    public void GoToAttackState() {
        enemy.animator.SetBool("Walk", false);
        enemy.currentState = enemy.attackState;
    }

    public void GoToIdleState() {
        enemy.animator.SetBool("Walk", false);
        enemy.currentState = enemy.idleState;
    }

    public void GoToReturnState() {
        enemy.currentState = enemy.returnState;
    }

    public void OnTriggerEnter(Collider col) { }

    public void Reset() { }
}
