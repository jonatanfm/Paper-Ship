using System;
using UnityEngine;

public class ThirdPersonUserControl : MonoBehaviour {

    private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.

        
    private void Start() {
        m_Cam = Camera.main.transform;
        m_Character = GetComponent<ThirdPersonCharacter>();
    }


    private void Update() {
        if (!m_Jump)
            m_Jump = Input.GetButtonDown("Jump");
            
    }


    private void FixedUpdate() { 
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool crouch = Input.GetKey(KeyCode.C);
        bool run = Input.GetKey(KeyCode.LeftShift);

        m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
        m_Move = (v * m_CamForward + h * m_Cam.right);

        m_Character.Move(m_Move, crouch, m_Jump, run);
        m_Jump = false;
    }
}

