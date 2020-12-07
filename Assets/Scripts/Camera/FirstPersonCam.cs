using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCam : MonoBehaviour {

    private Transform m_Cam;
    [SerializeField] protected Transform m_Target;

    [HideInInspector]
    public Vector3 camPosition = new Vector3(1f, 0.48f, -2f);

    private void Awake() {
        m_Cam = GetComponentInChildren<Camera>().transform;
    }

    void Update() {
        var characterRotation = m_Cam.rotation;
        characterRotation.x = 0;
        characterRotation.z = 0;

        m_Target.rotation = characterRotation;
    }
}
