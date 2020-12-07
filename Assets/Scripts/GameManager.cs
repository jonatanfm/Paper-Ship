using UnityEngine;


public class GameManager : MonoBehaviour {

    [HideInInspector]
    public static bool gameRunning = true;
    [SerializeField]
    private MenuManager menuManager;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Time.timeScale = gameRunning ? 0 : 1;
            gameRunning = !gameRunning;
            menuManager.OpenMenu(!gameRunning);
        }

    }
}
