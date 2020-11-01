using UnityEngine;
using UnityEngine.UI;

public class TextUpdate : MonoBehaviour {

    private LanguageSystem languageSystem;
    [SerializeField]
    private string dictionaryKey;

    private Text text;

    private string concatString = "";

    void Start() {
        text = GetComponent<Text>();
        languageSystem = FindObjectOfType<LanguageSystem>();

        languageSystem.languageChangeEvent.AddListener(LanguageChanged);
        if (languageSystem.currentDictionary != null)
            UpdateText();
    }

    private void LanguageChanged() {
        UpdateText();
    }

    private void UpdateText() {
        text.text = languageSystem.currentDictionary[dictionaryKey] + concatString;
    }

    public void AddConcatString(string text) {
        concatString = text;
        UpdateText();
    }

    
}
