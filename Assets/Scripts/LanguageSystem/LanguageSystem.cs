using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Xml;

public class LanguageSystem : MonoBehaviour
{
    public enum Lang {
        en,
        es
    }

    private Lang currentLang;

    [HideInInspector]
    public UnityEvent languageChangeEvent;

    public Dictionary<string, string> currentDictionary;

    void Start() {
        switch(Application.systemLanguage) {
            case SystemLanguage.Spanish:
                currentLang = Lang.es;
                    break;
            default:
                currentLang = Lang.en;
                    break;
        }

        StartCoroutine(InvokeTextUpdate());
    }

    private IEnumerator InvokeTextUpdate() {
        yield return new WaitForEndOfFrame();
        string lang = GetComponent<PrefManager>().GetLanguage();
        if (lang == Lang.es.ToString())
            currentLang = Lang.es;
        else
            currentLang = Lang.en;
        LoadNewDic();
        languageChangeEvent.Invoke();
    }

    public void Change(string newLang) {
        if (currentLang.ToString() != newLang) {
            currentLang = newLang == Lang.en.ToString() ? Lang.en : Lang.es;

            LoadNewDic();
            languageChangeEvent.Invoke();
        }
    }

    private void LoadNewDic() {
        currentDictionary = new Dictionary<string, string>();
        var xml = Resources.Load<TextAsset>("Languages/" + currentLang.ToString());
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml.text);

        foreach (XmlNode talent in xmldoc["language"]) {
            currentDictionary.Add(talent.Name, talent.InnerText);
        }
    }

    public bool IsSpanish() {
        return currentLang == Lang.es;
    }
}

