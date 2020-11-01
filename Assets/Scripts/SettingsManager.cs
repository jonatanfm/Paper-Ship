using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour {

    private PrefManager prefManager;
    private LanguageSystem languageSystem;

    [SerializeField]
    private Button englishButton, spanishButton;
    
    [SerializeField]
    private Slider volumeSlider, soundSlider;


    private void Start() {
        prefManager = GetComponent<PrefManager>();
        languageSystem = GetComponent<LanguageSystem>();
        SetLanguageButtonInteractable(prefManager.GetLanguage() == LanguageSystem.Lang.en.ToString());
        volumeSlider.value = prefManager.GetVolume();
        soundSlider.value = prefManager.GetSound();
    }

    public void SetLanguage(string lang) {
        languageSystem.Change(lang);
        prefManager.SetLanguage(lang);
        SetLanguageButtonInteractable(lang == LanguageSystem.Lang.en.ToString());
    }

    private void SetLanguageButtonInteractable(bool isEnglish) {
        spanishButton.interactable = isEnglish;
        englishButton.interactable = !isEnglish;
    }

    public void SetVolume(float volume) {
        prefManager.SetVolume(volume);
    }

    public void SetSound(float sound) {
        prefManager.SetSound(sound);
    }


}
