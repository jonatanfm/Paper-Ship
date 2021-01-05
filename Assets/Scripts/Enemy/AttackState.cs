using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IEnemyState {

    EnemyAI enemy;
    float minTimeBetweenAttacks = 2.5f;
    float maxTimeBetweenAttacks = 5f;
    float nextAttackTime = 0f;
    float currentTime = 0;

    public AttackState(EnemyAI enemy) {
        this.enemy = enemy;
        nextAttackTime = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks);
    }

    public void UpdateState() {
        enemy.transform.LookAt(enemy.player.transform);

        currentTime += Time.deltaTime;
        if (currentTime > nextAttackTime) {
            enemy.animator.SetTrigger("Attack");
            currentTime = 0;
            nextAttackTime = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks);
        }
        if (Vector3.Distance(enemy.transform.position, enemy.player.transform.position) > 2f)
            GoToFollowState();
    }

    public void GoToFollowState() {
        currentTime = 0;
        enemy.navMeshAgent.isStopped = false;
        enemy.animator.SetBool("Walk", true);
        enemy.currentState = enemy.followState;
    }

    public void GoToAttackState() { }

    public void GoToReturnState() { }

    public void GoToIdleState() { }

    public void Impact() { }

    public void OnTriggerEnter(Collider other) {
        if (currentTime < 0.5f) 
            other.GetComponent<LifeManager>().Hit(20f);
    }

    public void Reset() {
        currentTime = 0;
    }
}
