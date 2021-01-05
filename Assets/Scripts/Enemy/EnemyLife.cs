using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemyLife : MonoBehaviour {

    [SerializeField]
    float totalLife = 100f;
    float currentLife;

    EnemyAI enemyAI;
    [SerializeField]
    GameObject hitParticle;
    [SerializeField]
    GameObject deathParticle;
    [SerializeField]
    AudioClip deadSound;

    AudioSource audioSource;
    public Slider lifeSlider;

    // Start is called before the first frame update
    void Start() {
        audioSource = GetComponent<AudioSource>();
        enemyAI = GetComponent<EnemyAI>();
        currentLife = totalLife;
        lifeSlider.maxValue = totalLife;
        lifeSlider.value = totalLife;
    }

    public void Hit(float damage) {
        GameObject particle = Instantiate(hitParticle, transform.position, Quaternion.identity);
        Destroy(particle, 3f);
        audioSource.Play();
        currentLife -= damage;
        enemyAI.Reset();
        lifeSlider.value = currentLife;
        if (currentLife <= 0) {
            GetComponent<Animator>().SetBool("Death", true);
            audioSource.PlayOneShot(deadSound);
            enemyAI.Stop();
            enemyAI.enabled = false;
            Destroy(this.gameObject, 1f);
            GameObject particle2 = Instantiate(deathParticle, transform.position, Quaternion.identity);
            Destroy(particle2, 3f);
        } else 
            GetComponent<Animator>().SetTrigger("Hit");
    }
}
