using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour {

    Animator animator;
    [SerializeField]
    Slider lifeSlider;
    [SerializeField]
    float totalLife = 200f;
    [SerializeField]
    float currentLife;
    [SerializeField]
    GameObject hitParticle;

    AudioSource audioSource;


    void Start() {
        animator = GetComponent<Animator>();
        if (currentLife == 0)
            currentLife = totalLife;
        lifeSlider.maxValue = totalLife;
        lifeSlider.value = currentLife;
        audioSource = GetComponent<AudioSource>();

    }
   
    public void ResetLife() {
        currentLife = totalLife;
        lifeSlider.value = currentLife;
    }

    public void Hit(float damage) {
        audioSource.Play();
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
