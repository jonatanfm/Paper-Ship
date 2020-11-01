using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    const string STATE_WALK = "walk";
    const string STATE_RUN = "run";
    const string STATE_JUMP = "jump";

    private CharacterController controller;
    private Transform cam;

    [SerializeField]
    private Animator modelAnimator;

    [SerializeField]
    private float speed = 4f;
    [SerializeField]
    private float runningSpeed = 6f;
    [SerializeField]
    private float turnSmoothTime = 0.1f;
    [SerializeField]
    private float jumpHeight = 2f;
    [SerializeField]
    private float gravityValue = -9.81f;

    private float turnSmoothVelocity;

    private Vector3 playerVelocity;
    private bool groundedPlayer;


    private void Start() {
        controller = GetComponent<CharacterController>();
        cam = FindObjectOfType<Camera>().transform;
    }

    private void Update() {
        if (GameManager.gameRunning) {
            modelAnimator.speed = 1f;
            Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

            if (direction.magnitude >= 0.1f) {
                modelAnimator.SetBool(STATE_WALK, true);
                float currentSpeed = speed;
                if (Input.GetAxis("Run") != 0f) {
                    currentSpeed = runningSpeed;
                    modelAnimator.SetBool(STATE_RUN, true);
                }

                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
            } else {
                if (modelAnimator.GetBool(STATE_RUN))
                    modelAnimator.SetBool(STATE_RUN, false);
                if (modelAnimator.GetBool(STATE_WALK))
                    modelAnimator.SetBool(STATE_WALK, false);
            }

            if (Input.GetButtonDown("Jump") && groundedPlayer) {
                modelAnimator.SetBool(STATE_JUMP, true);
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -gravityValue);
            }

            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
        } 
    }


    void FixedUpdate() {
        if (GameManager.gameRunning) {
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0) {
                if (modelAnimator.GetBool(STATE_JUMP))
                    modelAnimator.SetBool(STATE_JUMP, false);
                playerVelocity.y = 0f;
            }

            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
        }
    }

    public void SwapAnimatorSpeed() {
        if (GameManager.gameRunning)
            modelAnimator.speed = 1f;
        else
            modelAnimator.speed = 0f;
    }
}
