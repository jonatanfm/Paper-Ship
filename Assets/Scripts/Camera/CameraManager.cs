using System;
using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    private Transform m_Cam;
    private FreeLookCam thirdPersonCamera;
    private ProtectCameraFromWallClip protectCameraFromWallClip;
    private FirstPersonCam fpsCam;

    protected Vector3 thirdPersonCameraPos;
    private float transitionTime = 0.5f;

    private bool thirdCamActive = true;
    Coroutine zoomCoroutine;
    float movingTime = 0;

    float prevMaxtilt;

    protected void Awake() {
        thirdPersonCamera = GetComponent<FreeLookCam>();
        fpsCam = GetComponent<FirstPersonCam>();
        protectCameraFromWallClip = GetComponent<ProtectCameraFromWallClip>();
        m_Cam = GetComponentInChildren<Camera>().transform;
        prevMaxtilt = thirdPersonCamera.m_TiltMax;
    }

    public void SwitchCamera() {
        if (thirdCamActive)
            EnableFPSCam();
        else
            EnableThirdPersonCam();
    }

    void EnableFPSCam() {
        fpsCam.enabled = true;
        //protectCameraFromWallClip.enabled = false;
        thirdCamActive = false;
        thirdPersonCamera.m_TiltMax = thirdPersonCamera.m_TiltMin;
        StartTransitionCoroutine(fpsCam.camPosition, false);
    }

    void EnableThirdPersonCam() {
        fpsCam.enabled = false;
        thirdCamActive = true;
        thirdPersonCamera.m_TiltMax = prevMaxtilt;
        StartTransitionCoroutine(thirdPersonCamera.GetCamPosition(), true);
    }

    private void StartTransitionCoroutine(Vector3 camPosition, bool enable) {
        if (zoomCoroutine != null)
            StopCoroutine(zoomCoroutine);
        movingTime = transitionTime - movingTime;
        if (movingTime == transitionTime)
            movingTime = 0;
        zoomCoroutine = StartCoroutine(Transition(camPosition, enable));
    }

    IEnumerator Transition(Vector3 newPosition, bool enableProtect) {
        yield return null;
        Vector3 startingPos = m_Cam.transform.localPosition;
        while (movingTime < transitionTime) {
            movingTime += Time.deltaTime;
            m_Cam.transform.localPosition = Vector3.Lerp(startingPos, newPosition, movingTime/transitionTime);
            yield return null;
        }
        m_Cam.transform.localPosition = newPosition;
        movingTime = 0;
        //if (enableProtect)
        //    protectCameraFromWallClip.enabled = true;
    }

}
