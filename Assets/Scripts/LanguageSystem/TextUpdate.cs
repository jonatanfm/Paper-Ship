using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextUpdate : MonoBehaviour {

    private LanguageSystem languageSystem;

    public string dictionaryKey;

    private TMP_Text text;

    private string concatString = "";

    void Start() {
        ManualStart();
    }

    public void ManualStart() {
        text = GetComponent<TMP_Text>();

        languageSystem = FindObjectOfType<LanguageSystem>();

        languageSystem.languageChangeEvent.AddListener(LanguageChanged);
        if (languageSystem.currentDictionary != null)
            UpdateText();
    }

    private void LanguageChanged() {
        UpdateText();
    }

    public void UpdateText() {
        if (text != null && languageSystem != null && languageSystem.currentDictionary != null) 
            text.text = languageSystem.currentDictionary[dictionaryKey] + concatString;
        
    }

    public void AddConcatString(string text) {
        concatString = text;
        UpdateText();
    }
}
