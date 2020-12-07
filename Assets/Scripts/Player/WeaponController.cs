using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponController : MonoBehaviour {

    private Queue<GameObject> instantiatedArrows;
    const int MAX_ARROWS_INSTANTIATED = 10; 

    const int NO_WEAPON_IDX = -1;
    const int SWORD_IDX = 0;
    const int BOW_IDX = 1;
    [SerializeField] GameObject[] weapons;

    [SerializeField] GameObject aimUI;

    [SerializeField] Transform neck;
    [SerializeField] Transform rightHand;
    [SerializeField] Transform leftHand;

    [SerializeField] CameraManager camManager;

    [SerializeField] GameObject arrowPrefab;

    private GameObject currentArrow;
    Animator m_Animator;
    bool canChangeWeapon = true;
    bool aiming = false;
    float drawTime = 1f;
    bool inDraw = false;

    int activeWeaponIdx = NO_WEAPON_IDX;

    int arrows = 50;

    [SerializeField]
    TMP_Text arrowsUI;


    Vector3[] weaponBackPositions, weaponHandPositions;
    Quaternion[] weaponBackRotations, weaponHandRotations;
    string[] animationTriggers = new string[2] { "EquipSword", "EquipBow" };
    int[] animationLayers = new int[2] { 1, 2 };

    // Start is called before the first frame update
    void Start() {
        instantiatedArrows = new Queue<GameObject>();
        m_Animator = GetComponent<Animator>();
        weapons[SWORD_IDX].transform.parent = neck.transform;
        weapons[BOW_IDX].transform.parent = neck.transform;

        weaponBackPositions = new Vector3[2] { weapons[SWORD_IDX].transform.localPosition, weapons[BOW_IDX].transform.localPosition };
        weaponHandPositions = new Vector3[2] { new Vector3(-0.04f, 0.087f,0.038f), new Vector3(0f, 0.07f, 0.023f) };
        weaponBackRotations = new Quaternion[2] { weapons[SWORD_IDX].transform.localRotation, weapons[BOW_IDX].transform.localRotation };
        weaponHandRotations = new Quaternion[2] { new Quaternion(0.2f, -0.2f, 0.7f, 0.6f), new Quaternion(0, 0f, 0.7f, 0.7f) };

        arrowsUI.text = arrows.ToString();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1) && canChangeWeapon)
            StartCoroutine(DrawWeapon(rightHand, SWORD_IDX));
        else if (Input.GetKeyDown(KeyCode.Alpha2) && canChangeWeapon)
            StartCoroutine(DrawWeapon(leftHand, BOW_IDX));
        else if (Input.GetMouseButtonDown(0) && canChangeWeapon && activeWeaponIdx != NO_WEAPON_IDX)
            Attack();
        else if (Input.GetMouseButtonDown(1) && canChangeWeapon && activeWeaponIdx == BOW_IDX)
            StartAim(true);
        else if (aiming && Input.GetMouseButtonUp(1))
            StartAim(false);
        else if (aiming && !inDraw && Input.GetMouseButtonDown(0))
            StartCoroutine(StartAimShoot(true));
        else if (aiming && Input.GetMouseButtonUp(0))
            StartCoroutine(StartAimShoot(false));


        if (Input.GetKeyDown(KeyCode.Q))
            Debug.Break();
    }


    IEnumerator StartAimShoot(bool draw) {
        if (draw) {
            m_Animator.SetTrigger("Draw");
            inDraw = true;
            yield return new WaitForSeconds(0.5f);
            inDraw = false;
            CreateArrow();
        } else {
            while (inDraw) {
                yield return null;
            }
            m_Animator.SetTrigger("Shoot");
            inDraw = false;
            ThroughArrow(50f, false);
            yield return null;
        }
    }

    private void ThroughArrow(float force, bool useGround) {
        if (currentArrow != null) {
            arrows--;
            arrowsUI.text = arrows.ToString();
            currentArrow.GetComponent<Arrow>().Shoot(force, useGround);
            instantiatedArrows.Enqueue(currentArrow);
            if (instantiatedArrows.Count > MAX_ARROWS_INSTANTIATED) {
                var arrow = instantiatedArrows.Dequeue();
                Destroy(arrow);
            }
            currentArrow = null;
        }
    }

    private void CreateArrow() {
        GameObject arrow = Instantiate(arrowPrefab, weapons[BOW_IDX].transform, false);
        arrow.transform.Translate(new Vector3(-0.32f, 0f, 0.03f));
        arrow.transform.Rotate(new Vector3(0f, 0f, 90f));
        currentArrow = arrow;
    }

    void StartAim(bool aim) {
        canChangeWeapon = !aim;
        aiming = aim;
        aimUI.SetActive(aim);
        m_Animator.SetBool("Aim", aim);
        camManager.SwitchCamera();
        if (!aim && currentArrow != null) {
            Destroy(currentArrow);
            currentArrow = null;
        }
    }

    void Attack() {
        if (activeWeaponIdx == SWORD_IDX)
            StartCoroutine(AttackSword());
        else
            StartCoroutine(AttackSimpleBow());
    }

    IEnumerator AttackSimpleBow() {
        canChangeWeapon = false;
        m_Animator.SetTrigger("SimpleBowAttack");
        yield return new WaitForSeconds(0.5f);
        CreateArrow();
        yield return new WaitForSeconds(0.1f);
        ThroughArrow(10f, true);
        canChangeWeapon = true;
    }

    IEnumerator AttackSword() {
        canChangeWeapon = false;
        m_Animator.SetBool("AttackSword", true);
        weapons[SWORD_IDX].GetComponent<Weapon>().ActivateWeapon();
        yield return new WaitForSeconds(1.3f);
        m_Animator.SetBool("AttackSword", false);

        canChangeWeapon = true;
    }

    IEnumerator DrawWeapon(Transform hand, int getWeapon) {
        m_Animator.SetTrigger(animationTriggers[getWeapon]);
        if (activeWeaponIdx != NO_WEAPON_IDX && activeWeaponIdx != getWeapon)
            m_Animator.SetTrigger(animationTriggers[activeWeaponIdx]);
        
        if (activeWeaponIdx != getWeapon)
            StartCoroutine(ChangeLayerWeight(animationLayers[getWeapon], true));
        if (activeWeaponIdx != NO_WEAPON_IDX)
            StartCoroutine(ChangeLayerWeight(animationLayers[activeWeaponIdx], false));

        canChangeWeapon = false;
        yield return new WaitForSeconds(drawTime / 2);
        if (getWeapon == activeWeaponIdx) {
            RemoveWeapon();
            activeWeaponIdx = NO_WEAPON_IDX;
        } else {
            if (activeWeaponIdx != NO_WEAPON_IDX)
                RemoveWeapon();
            activeWeaponIdx = getWeapon;
            AddWeapon(hand);
        }
        yield return new WaitForSeconds(drawTime / 2);
        canChangeWeapon = true;
    }

    private void RemoveWeapon() {
        m_Animator.SetLayerWeight(animationLayers[activeWeaponIdx], 0);
        weapons[activeWeaponIdx].transform.parent = neck.transform;
        weapons[activeWeaponIdx].transform.localPosition = weaponBackPositions[activeWeaponIdx];
        weapons[activeWeaponIdx].transform.localRotation = weaponBackRotations[activeWeaponIdx];
    }

    private void AddWeapon(Transform hand) {
        m_Animator.SetLayerWeight(animationLayers[activeWeaponIdx], 1);
        weapons[activeWeaponIdx].transform.parent = hand.transform;
        weapons[activeWeaponIdx].transform.localPosition = weaponHandPositions[activeWeaponIdx];
        weapons[activeWeaponIdx].transform.localRotation = weaponHandRotations[activeWeaponIdx];
    }

    IEnumerator ChangeLayerWeight(int layerIdx, bool enableLayer) {
        float time = drawTime;

        while (time > float.Epsilon) {
            yield return null;
            time -= Time.deltaTime;
            float currWeight = Mathf.Clamp(time / drawTime, 0f, 1f);
            m_Animator.SetLayerWeight(layerIdx, enableLayer ? (1f-currWeight) : currWeight);
        }

        m_Animator.SetLayerWeight(layerIdx, enableLayer ? 1f : 0f);
    }
}
