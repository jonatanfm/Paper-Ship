using UnityEngine;

public interface IEnemyState {
    void UpdateState();
    void GoToAttackState();
    void GoToFollowState();
    void GoToIdleState();
    void GoToReturnState();
    void OnTriggerEnter(Collider other);
    void Reset();
}
