using UnityEngine;


public class GameManager : MonoBehaviour {

    [HideInInspector]
    public static bool gameRunning = true;

    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private GameObject cvCam;
    [SerializeField]
    private MenuManager menuManager;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            gameRunning = !gameRunning;
            player.SwapAnimatorSpeed();
            cvCam.SetActive(gameRunning);
            menuManager.OpenMenu(!gameRunning);
        }

    }
}
