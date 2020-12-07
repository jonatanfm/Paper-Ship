using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowState : IEnemyState {

    EnemyAI enemy;

    public FollowState(EnemyAI enemy) {
        this.enemy = enemy;
    }

    public void UpdateState() {
        enemy.navMeshAgent.destination = enemy.player.transform.position;
        float dist = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);
        if (dist < 2f)
            GoToAttackState();
        else if (dist > 10f)
            GoToReturnState();
    }

    public void GoToFollowState() { }

    public void GoToAttackState() {
        enemy.navMeshAgent.isStopped = true;
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

    public void Impact() {
        GoToAttackState();
    }

    public void OnTriggerEnter(Collider col) { }

    public void Reset() { }
}
