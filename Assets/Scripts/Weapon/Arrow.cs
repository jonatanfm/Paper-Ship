using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    Rigidbody rb;
    float range = 100f;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }


    public void Shoot(float force, bool useGround) {
        GetComponent<Weapon>().ActivateWeapon();
        transform.parent = null;
        rb.isKinematic = false;
        Vector3 velocity = Vector3.ProjectOnPlane(-transform.up, Vector3.up);
        
        if(!useGround) {
            Vector3 end;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range)) {
                velocity = hit.point - transform.position;
                end = hit.point;
            } else {
                velocity = Camera.main.transform.position + (Camera.main.transform.forward * range) - transform.position;
                end = Camera.main.transform.position + (Camera.main.transform.forward * range);
            }

            transform.LookAt(end);
            transform.Rotate(new Vector3(0f, -90f, 90f));
        }

        rb.velocity = velocity.normalized * force;
    }

    void FixedUpdate() {
        if (rb.velocity != Vector3.zero)
            transform.Rotate(0, 30f, 0);
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player") && !other.CompareTag("Enemy")) {
            GetComponent<BoxCollider>().enabled = false;
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
