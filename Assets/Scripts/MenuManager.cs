using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    const int GAME_SCENE = 1;
    const int INTRO_SCENE = 2;

    [SerializeField]
    private GameObject menu;

    [SerializeField]
    private GameObject settingsMenu;

    public void PlayGame() {
        SceneManager.LoadScene(GAME_SCENE);
    }

    public void PlayIntro() {
        SceneManager.LoadScene(INTRO_SCENE);
    }

    public void OpenSettings(bool open) {
        settingsMenu.SetActive(open);
    }

    public void OpenMenu(bool open) {
        menu.SetActive(open);
    }

    public void EndGame() {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }







}
