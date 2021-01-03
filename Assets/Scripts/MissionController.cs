using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionController : MonoBehaviour {
    [SerializeField]
    TextUpdate mission1, mission2;

    public int currentLogs = 0;
    int logsToGet = 10;

    public bool hasTools = false;

    void Start() {
        UpdateLogsMission();
        UpdateToolsMission();
    }

    void Update() {
        
    }

    void UpdateLogsMission() {
        mission1.AddConcatString(" (" + currentLogs + "/" + logsToGet + ")");
    }

    void UpdateToolsMission() {
        mission2.AddConcatString(" (" + (hasTools ? 1 : 0) + "/1)");
    }
    
    public void AddLog() {
        currentLogs++;
        UpdateLogsMission();
    }

    public void AddTools() {
        hasTools = true;
        UpdateToolsMission();
    }

}
