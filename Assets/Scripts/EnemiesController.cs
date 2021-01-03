using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour {

    public GameObject enemyPrefab;
    int enemies;

    Vector3[] positions;
    Quaternion[] rotations;
    

    void Start() {
        enemies = transform.childCount;
        positions = new Vector3[enemies];
        rotations = new Quaternion[enemies];
        for(int i = 0; i<enemies; i++) {
            positions[i] = transform.GetChild(i).transform.position;
            rotations[i] = transform.GetChild(i).transform.rotation;
        }
    }

    public void ReloadEnemies() {
        for (int i = 0; i < transform.childCount; i++) {
            Destroy(transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < enemies; i++) {
            var enemy = Instantiate(enemyPrefab, positions[i], rotations[i]);
            enemy.transform.parent = transform;
        }
    }

    
}
