using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour {

    [SerializeField]
    private GameObject grabUI;
    [SerializeField]
    private TextUpdate grabUIMessage;

    public bool woodLog = false;

    bool playerIsNear = false;

    MissionController missionController;

    void Start() {
        missionController = FindObjectOfType<MissionController>();
    }

    void Update() {
        if (playerIsNear && Input.GetKeyDown(KeyCode.E)) {
            playerIsNear = false;
            grabUI.SetActive(false);
            GrabItem();
        }
    }

    void GrabItem() {
        if (woodLog)
            missionController.AddLog();
        else
            missionController.AddTools();

        GetComponent<SphereCollider>().enabled = false;
        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            playerIsNear = true;
            grabUIMessage.dictionaryKey = "grab";
            grabUIMessage.UpdateText();
            grabUI.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")) {
            playerIsNear = true;
            grabUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            playerIsNear = false;
            grabUI.SetActive(false);
        }
    }
}
