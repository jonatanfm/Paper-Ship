using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntroManager : MonoBehaviour {

    [SerializeField]
    private Image fadeImage;

    [SerializeField]
    private GameObject actionUI;
    [SerializeField]
    private TMP_Text actionMessage;

    [SerializeField]
    private GameObject textUI;
    [SerializeField]
    private TMP_Text npcName;
    [SerializeField]
    private TMP_Text conversation;
    private TextUpdate conversationTextUpdate;
    [SerializeField]
    private GameObject nextMessage;
    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private GameObject sword;
    [SerializeField]
    private GameObject bow;

    int currentStep = 0;
    bool enabledE = false;
    bool wakedUp = false;

    void Start() {
        conversationTextUpdate = conversation.GetComponent<TextUpdate>();

        StartCoroutine(FadeIn());
    }

    void Update() {
        if (enabledE && Input.GetKeyDown(KeyCode.E)) {
            switch (currentStep) {
                case 0:
                    WakeUp();
                    break;
                case 8:
                    EndIntro();
                    break;
                default:
                    NextMessage();
                    break;
            }
        }
    }

    void WakeUp() {
        if (!wakedUp) {
            enabledE = false;
            actionUI.SetActive(false);
            textUI.SetActive(false);
            StartCoroutine(WakeUpCoroutine());
        } else
            NextMessage();
    }

    IEnumerator WakeUpCoroutine() {
        playerAnimator.SetTrigger("wakeUp");
        yield return new WaitForSeconds(2.5f);
        wakedUp = true;
        ShowTalkMessage();
    }

    void ShowTalkMessage() {
        TextUpdate txtUpd = actionUI.GetComponentInChildren<TextUpdate>();
        txtUpd.dictionaryKey = "talk";
        txtUpd.UpdateText();
        actionUI.SetActive(true);
        enabledE = true;
    }

    void NextMessage() {
        enabledE = true;
        actionUI.SetActive(false);
        textUI.SetActive(true);
        currentStep++;
        conversationTextUpdate.dictionaryKey = "intromessage" + currentStep;
        conversationTextUpdate.UpdateText();
        nextMessage.SetActive(true);
        switch (currentStep) {
            case 1:
                npcName.text = "Eyra";
                break;
            case 7:
                sword.SetActive(true);
                bow.SetActive(true);
                break;
            case 8:
                FindObjectOfType<LifeManager>().ResetLife();
                break;
        }
    }

    void EndIntro() {
        enabledE = false;
        actionUI.SetActive(false);
        textUI.SetActive(false);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut () {
        fadeImage.gameObject.SetActive(true);

        Color fadeColor = fadeImage.color;
        float opacity = 0;
        fadeColor.a = opacity;
        fadeImage.color = fadeColor;
        
        while (opacity <= 1) {
            opacity += 0.05f;
            fadeColor.a = opacity;
            fadeImage.color = fadeColor;
            yield return new WaitForSeconds(0.05f);
        }

        FindObjectOfType<MenuManager>().PlayGame();
    }

    IEnumerator FadeIn() {
        fadeImage.gameObject.SetActive(true);
        Color fadeColor = fadeImage.color;
        float opacity = 1;
        fadeColor.a = opacity;
        fadeImage.color = fadeColor;

        while (opacity >= 0) {
            opacity -= 0.05f;
            fadeColor.a = opacity;
            fadeImage.color = fadeColor;
            yield return new WaitForSeconds(0.05f);
        }

        fadeImage.gameObject.SetActive(true);
        enabledE = true;
    }
}
