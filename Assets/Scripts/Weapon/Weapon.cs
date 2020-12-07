using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    [SerializeField]
    float damage = 20f;
    bool active = false;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Enemy") && active)
            other.GetComponent<EnemyLife>().Hit(damage);
    }

    public void ActivateWeapon() {
        active = true;
    }
}
