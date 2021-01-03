using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {

    [SerializeField]
    private Image fadeImage;

    bool gameStarted = false;
    [HideInInspector]
    public static bool gameRunning = true;
    [SerializeField]
    private MenuManager menuManager;
    [SerializeField]
    private FreeLookCam freeLookCam;

    private void Start() {
        StartCoroutine(FadeIn());
    }

    void Update() {
        if (gameStarted && Input.GetKeyDown(KeyCode.Escape)) {
            Time.timeScale = gameRunning ? 0 : 1;
            gameRunning = !gameRunning;
            menuManager.OpenMenu(!gameRunning);
            freeLookCam.enabled = gameRunning;
        }
    }

    IEnumerator FadeIn() {
        fadeImage.gameObject.SetActive(true);
        Time.timeScale = 0;
        Color fadeColor = fadeImage.color;
        float opacity = 1;
        fadeColor.a = opacity;
        fadeImage.color = fadeColor;

        while (opacity >= 0) {
            opacity -= 0.05f;
            fadeColor.a = opacity;
            fadeImage.color = fadeColor;
            yield return new WaitForSecondsRealtime(0.05f);
        }
        Time.timeScale = 1;
        fadeImage.gameObject.SetActive(false);
        gameStarted = true;
    }
}
