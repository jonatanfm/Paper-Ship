using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    [SerializeField]
    float damage = 20f;
    bool active = false;
    List<int> enemiesHit = new List<int>();
    bool entered = false;
    public bool arrow = false;

    private void OnTriggerEnter(Collider other) {
        if (active && other.gameObject.CompareTag("Enemy") && !enemiesHit.Contains(other.GetInstanceID())) {
            other.GetComponent<EnemyLife>().Hit(damage);
            enemiesHit.Add(other.GetInstanceID());
            entered = true;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (arrow && !entered && active && other.gameObject.CompareTag("Enemy") && !enemiesHit.Contains(other.GetInstanceID())) {
            other.GetComponent<EnemyLife>().Hit(damage);
            enemiesHit.Add(other.GetInstanceID());
            entered = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (active && other.gameObject.CompareTag("Enemy")) {
            entered = false;
        }
    }

    public void ActivateWeapon() {
        active = true;
        enemiesHit = new List<int>();
    }
}
