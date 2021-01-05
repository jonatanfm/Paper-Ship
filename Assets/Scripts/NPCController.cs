using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class NPCController : MonoBehaviour {
    [SerializeField]
    private Image fadeImage;
    [SerializeField]
    private GameObject talkUI;
    [SerializeField]
    private TextUpdate talkUIMessage;
    [SerializeField]
    private GameObject conversationUI;
    [SerializeField]
    private TMP_Text conversationText;
    private TextUpdate conversationTextUpdate;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject cam;
    [SerializeField]
    private EnemiesController enemiesController;
    [SerializeField]
    private TMP_Text endGameText;

    bool hasLogs = false;
    bool hasTools = false;

    [SerializeField]
    private Button exitButton,restButton, logsButton, toolButton, letsgoButton, yesButton, noButton;

    bool playerIsNear = false;
    bool isTalking = false;

    MissionController missionController;
    GameManager gameManager;

    enum TalkState {
        INTRO,
        REST,
        LOGS,
        TOOLS,
        LETGO
    }

    TalkState currentTalkState;

    private void Start() {
        conversationTextUpdate = conversationText.GetComponent<TextUpdate>();
        missionController = FindObjectOfType<MissionController>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update() {
        if (playerIsNear && !isTalking && Input.GetKeyDown(KeyCode.E)) {
            ShowTextMessage();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            playerIsNear = true;
            talkUIMessage.dictionaryKey = "talk";
            talkUIMessage.UpdateText();
            talkUI.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")) {
            playerIsNear = true;
            talkUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            playerIsNear = false;
            talkUI.SetActive(false);
        }
    }

    void ShowTextMessage() {
        isTalking = true;
        EnablePlayerMovement(false);
        conversationUI.SetActive(true);
        talkUI.SetActive(false);
        ShowTalkButtons(true);
        conversationTextUpdate.ManualStart();
        SetConverationText("whatcanido");
    }

    private void SetConverationText(string key) {
        conversationTextUpdate.dictionaryKey = key;
        conversationTextUpdate.UpdateText();
    }

    void ShowTalkButtons(bool optionsActive) {
        exitButton.gameObject.SetActive(optionsActive);
        restButton.gameObject.SetActive(optionsActive);
        toolButton.gameObject.SetActive(optionsActive && !hasTools);
        logsButton.gameObject.SetActive(optionsActive && !hasLogs);
        letsgoButton.gameObject.SetActive(optionsActive && hasLogs && hasTools);
        yesButton.gameObject.SetActive(!optionsActive);
        noButton.gameObject.SetActive(!optionsActive);
    }

    public void Rest() {
        currentTalkState = TalkState.REST;
        SetConverationText("restanswer");
        ShowTalkButtons(false);
    }

    public void Logs() {
        currentTalkState = TalkState.LOGS;
        SetConverationText("logsanswer");
        ShowTalkButtons(false);
    }

    public void Tools() {
        currentTalkState = TalkState.TOOLS;
        SetConverationText("toolsanswer");
        ShowTalkButtons(false);
    }

    public void Letgo() {
        currentTalkState = TalkState.LETGO;
        SetConverationText("letsgosanswer");
        ShowTalkButtons(false);
    }

    public void Exit() {
        currentTalkState = TalkState.INTRO;
        conversationUI.SetActive(false);
        talkUI.SetActive(true);
        isTalking = false;
        EnablePlayerMovement(true);
    }

    void EnablePlayerMovement(bool enable) {
        if (!enable) {
            player.GetComponent<Animator>().SetFloat("Forward", 0.0f);
            player.GetComponent<Animator>().SetFloat("Turn", 0.0f);
            player.GetComponent<Rigidbody>().velocity = Vector3.zero; 
        }
        player.GetComponent<ThirdPersonUserControl>().enabled = enable;
        player.GetComponent<ThirdPersonCharacter>().enabled = enable;
        cam.GetComponent<FreeLookCam>().enabled = enable;
        player.GetComponent<WeaponController>().EnableAttack(enable);
    }

    public void ConfirmAction(bool yes) {
        if (yes) {
            switch(currentTalkState) {
                case TalkState.REST:
                    currentTalkState = TalkState.INTRO;
                    SetConverationText("restansweryes");
                    StartCoroutine(DoRest());
                    break;
                case TalkState.LOGS:
                    currentTalkState = TalkState.INTRO;
                    if (missionController.currentLogs == 10) {
                        SetConverationText("logsansweryes");
                        hasLogs = true;
                        missionController.HideLogMission();
                    } else
                        SetConverationText("logsanswerno");
                    ShowTalkButtons(true);
                    break;
                case TalkState.TOOLS:
                    currentTalkState = TalkState.INTRO;
                    if (missionController.hasTools) {
                        SetConverationText("toolsansweryes");
                        hasTools = true;
                        missionController.HideToolsMission();
                    } else
                        SetConverationText("toolsanswerno");
                    ShowTalkButtons(true);
                    break;
                case TalkState.LETGO:
                    currentTalkState = TalkState.INTRO;
                    SetConverationText("letsgosansweryes");
                    StartCoroutine(EndGame());
                    break;
            }
        } else {
            currentTalkState = TalkState.INTRO;
            SetConverationText("noanswer");
            ShowTalkButtons(true);
        }
    }

    IEnumerator DoRest() {
        fadeImage.gameObject.SetActive(true);
        Time.timeScale = 0;
        gameManager.gameStarted = false;
        Color fadeColor = fadeImage.color;
        float opacity = 0;
        fadeColor.a = opacity;
        fadeImage.color = fadeColor;

        while (opacity <= 1) {
            opacity += 0.05f;
            fadeColor.a = opacity;
            fadeImage.color = fadeColor;
            yield return new WaitForSecondsRealtime(0.1f);
        }

        Exit();
        player.GetComponent<LifeManager>().ResetLife();
        player.GetComponent<WeaponController>().ResetArrows();
        enemiesController.ReloadEnemies();

        opacity = 1;
        fadeColor.a = opacity;
        fadeImage.color = fadeColor;

        while (opacity >= 0) {
            opacity -= 0.05f;
            fadeColor.a = opacity;
            fadeImage.color = fadeColor;
            yield return new WaitForSecondsRealtime(0.1f);
        }

        Time.timeScale = 1;
        gameManager.gameStarted = true;
        fadeImage.gameObject.SetActive(false);
    }

    IEnumerator EndGame() {
        fadeImage.gameObject.SetActive(true);
        Time.timeScale = 0;
        gameManager.gameStarted = false;
        Color fadeColor = fadeImage.color;
        float opacity = 0;
        fadeColor.a = opacity;
        fadeImage.color = fadeColor;

        while (opacity <= 1) {
            opacity += 0.05f;
            fadeColor.a = opacity;
            fadeImage.color = fadeColor;
            yield return new WaitForSecondsRealtime(0.1f);
        }

        string endText = FindObjectOfType<LanguageSystem>().currentDictionary["endgame"];
        char[] endTextArray = endText.ToCharArray();
        string text = "";


        for(int i = 0; i<endTextArray.Length; i++) {
            text += endTextArray[i];
            endGameText.text = text;
            yield return new WaitForSecondsRealtime(0.1f);
        }

        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
