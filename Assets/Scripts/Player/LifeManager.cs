using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour {

    Animator animator;
    [SerializeField]
    Slider lifeSlider;

    float totalLife = 200f;
    float currentLife;
    [SerializeField]
    GameObject hitParticle;


    void Start() {
        animator = GetComponent<Animator>();
        currentLife = totalLife;
        lifeSlider.maxValue = totalLife;
        lifeSlider.value = totalLife;

    }

    public void Hit(float damage) {        
        currentLife -= damage;
        lifeSlider.value = currentLife;
        GameObject particle = Instantiate(hitParticle, transform.position, Quaternion.identity);
        Destroy(particle, 3f);
        if (currentLife <= 0)
            Die();
        else
            animator.SetTrigger("Hit");
    }

    void Die() {
        animator.SetBool("Death", true);
    }
}
