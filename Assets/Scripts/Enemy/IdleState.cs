using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState {

    EnemyAI enemy;

    public IdleState(EnemyAI enemy) {
        this.enemy = enemy;
    }

    public void UpdateState() {
        if (Vector3.Distance(enemy.transform.position, enemy.player.transform.position) < 10f)
            GoToFollowState();
    }

    public void GoToFollowState() {
        enemy.animator.SetBool("Walk", true);
        enemy.currentState = enemy.followState;
    }

    public void GoToAttackState() {
        enemy.currentState = enemy.attackState;
    }

    public void GoToReturnState() {
        enemy.animator.SetBool("Walk", true);
        enemy.currentState = enemy.returnState;
    }

    public void GoToIdleState() { }

    public void OnTriggerEnter(Collider col) { }

    public void Reset() { }
}
