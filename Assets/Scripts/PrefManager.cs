using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefManager : MonoBehaviour {

    const string LANGUAGE_KEY = "lang";
    const string VOLUME_KEY = "vol";
    const string SOUND_KEY = "sound";

    private string LANGUAGE_INIT = LanguageSystem.Lang.en.ToString();
    const float VOLUME_INIT = 1f;
    const float SOUND_INIT = 1f;

    void Start() {
        if (!PlayerPrefs.HasKey(LANGUAGE_KEY)) 
            PlayerPrefs.SetString(LANGUAGE_KEY, LANGUAGE_INIT);

        if (!PlayerPrefs.HasKey(VOLUME_KEY)) 
            PlayerPrefs.SetFloat(VOLUME_KEY, VOLUME_INIT);

        //change global volume level

        if (!PlayerPrefs.HasKey(SOUND_KEY)) 
            PlayerPrefs.SetFloat(SOUND_KEY, SOUND_INIT);

        //change global sound level
    }


    public string GetLanguage() {
        return PlayerPrefs.GetString(LANGUAGE_KEY);
    }


    public void SetLanguage(string lang) {
        PlayerPrefs.SetString(LANGUAGE_KEY, lang);
    }

    public float GetVolume() {
        return PlayerPrefs.GetFloat(VOLUME_KEY);
    }

    public void SetVolume(float volume) {
        PlayerPrefs.SetFloat(VOLUME_KEY, volume);
        //change global volume level
    }

    public float GetSound() {
        return PlayerPrefs.GetFloat(SOUND_KEY);
    }

    public void SetSound(float sound) {
        PlayerPrefs.SetFloat(SOUND_KEY, sound);
        //change global sound level
    }
}
