using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour {

    public GameObject directional;
    public float totalSecondsTo16Hours = 90;
    public float startDayRotationX = 0;
    public float endDayRotationX = 270;
    private float elapsedTime = 0;

    void Start() {
        Reset();
    }

    // Update is called once per frame
    void Update() {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > 40) 
            Reset();
        

        float rot = (endDayRotationX - startDayRotationX) / totalSecondsTo16Hours;
        directional.transform.Rotate(-rot * Time.deltaTime, 0, 0);
    }

    private void Reset() {
        //totalSecondsTo16Hours--;
        elapsedTime = 0;
        directional.transform.rotation = Quaternion.Euler(
            startDayRotationX,
            directional.transform.rotation.eulerAngles.y,
            directional.transform.rotation.eulerAngles.z);
    }

}
