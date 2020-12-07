using System;
using UnityEngine;

public class FreeLookCam : MonoBehaviour {

    private Transform m_Cam;
    private Transform m_Pivot;
    private Vector3 m_LastTargetPosition;

    [SerializeField] protected Transform m_Target;
    [SerializeField] private float m_MoveSpeed = 1f;                      // How fast the rig will move to keep up with the target's position.
    [Range(1f, 10f)] [SerializeField] private float m_TurnSpeed = 1.5f;   // How fast the rig will rotate from user input.
    public float m_TiltMax = 75f;                       // The maximum value of the x axis rotation of the pivot.
    public float m_TiltMin = 45f;                       // The minimum value of the x axis rotation of the pivot.
    [SerializeField] private bool m_LockCursor = false;                   // Whether the cursor should be hidden and locked.

    private float maxCamZoom = -5f;
    private float minCamZoom = -1.5f;

    [Range(-5f, -1.5f)] [SerializeField] private float m_CameraZoom = -3f;

    private float m_LookAngle;                    // The rig's y axis rotation.
    public float m_TiltAngle;                    // The pivot's x axis rotation.
    private const float k_LookDistance = 100f;    // How far in front of the pivot the character's look target is.
	private Vector3 m_PivotEulers;
	private Quaternion m_PivotTargetRot;
	private Quaternion m_TransformTargetRot;

    private Vector3 lastCamPosition;

    private void Awake() {
        m_Cam = GetComponentInChildren<Camera>().transform;

        m_Pivot = m_Cam.parent;

        // Lock or unlock the cursor.
        Cursor.lockState = m_LockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !m_LockCursor;

        m_PivotEulers = m_Pivot.rotation.eulerAngles;

        m_PivotTargetRot = m_Pivot.transform.localRotation;
        m_TransformTargetRot = transform.localRotation;
    }

    private void Update() {
        if (Time.timeScale > float.Epsilon) {
            HandleZoom();
            HandleRotationMovement();
        }
    }
    
    private void OnDisable() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public Vector3 GetCamPosition() {
        return new Vector3(0.3f, 0.48f, m_CameraZoom);
    }

    private void FixedUpdate() {
        if (m_Target == null) return;
        // Move the rig towards target position.
        transform.position = Vector3.Lerp(transform.position, m_Target.position, Time.deltaTime*m_MoveSpeed);
    }

    private void HandleZoom() {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0) {
            Vector3 pos = m_Cam.localPosition;
            pos.z = Mathf.Clamp(pos.z + scroll, maxCamZoom, minCamZoom);
            m_Cam.localPosition = pos;
        }
    }

    private void HandleRotationMovement() {
        // Read the user input
        var x = Input.GetAxis("Mouse X");
        var y = Input.GetAxis("Mouse Y");

        // Adjust the look angle by an amount proportional to the turn speed and horizontal input.
        m_LookAngle += x*m_TurnSpeed;

        // Rotate the rig (the root object) around Y axis only:
        m_TransformTargetRot = Quaternion.Euler(0f, m_LookAngle, 0f);

        m_TiltAngle -= y*m_TurnSpeed;
        m_TiltAngle = Mathf.Clamp(m_TiltAngle, -m_TiltMin, m_TiltMax);
        
 
        // Tilt input around X is applied to the pivot (the child of this object)
		m_PivotTargetRot = Quaternion.Euler(m_TiltAngle, m_PivotEulers.y , m_PivotEulers.z);


		m_Pivot.localRotation = m_PivotTargetRot;
		transform.localRotation = m_TransformTargetRot;
    }
}

