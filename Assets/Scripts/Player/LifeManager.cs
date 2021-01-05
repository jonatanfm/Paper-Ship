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
    [SerializeField]
    AudioClip deadSound;


    [SerializeField]
    GameObject gameOverUI;

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
        currentLife -= damage;
        lifeSlider.value = currentLife;
        GameObject particle = Instantiate(hitParticle, transform.position, Quaternion.identity);
        Destroy(particle, 3f);
        if (currentLife <= 0)
            Die();
        else {
            animator.SetTrigger("Hit");
            audioSource.Play();
        }
    }

    void Die() {
        audioSource.PlayOneShot(deadSound);
        animator.SetBool("Death", true);
        StartCoroutine(GameOver());
    }

    IEnumerator GameOver() {
        foreach (EnemyAI enemyAI in FindObjectsOfType<EnemyAI>()) {
            enemyAI.Stop();
            enemyAI.enabled = false;
        }
        FindObjectOfType<FreeLookCam>().enabled = false;
        FindObjectOfType<GameManager>().gameStarted = false;
        yield return new WaitForSeconds(2f);
        gameOverUI.SetActive(true);
    }
}
