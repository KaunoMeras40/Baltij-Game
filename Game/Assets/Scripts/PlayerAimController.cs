using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;


public class PlayerAimController : MonoBehaviour
{
    public static PlayerAimController instance;

    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    public float aimSensitivity = 1f;
    [SerializeField] private float NormalSensitivity = 1f;
    [SerializeField] private GameObject Crosshair;
    [SerializeField] private LayerMask AimColliderMask = new LayerMask();

    public Transform aimPos;

    private Vector3 MouseWorldPosition;

    [HideInInspector] public Animator animator;

    public float DefaultWalkSpeed;
    public float DefaultSprintSpeed;
    public bool aimed { get; private set; }

    bool canAttack = true;

    private StarterAssetsInputs Inputs;
    private ThirdPersonController Charactercontroller;

    [SerializeField] private MultiAimConstraint BodyAim;
    [SerializeField] private MultiAimConstraint HeadAim;
    [SerializeField] private MultiAimConstraint RHandAim;

    [SerializeField] private TwoBoneIKConstraint LHandIK_Rifle;
    [SerializeField] private TwoBoneIKConstraint LHandIKAimed_Rifle;

    [SerializeField] private TwoBoneIKConstraint LHandIK_Pistol;
    [SerializeField] private TwoBoneIKConstraint LHandIKAimed_Pistol;

    [SerializeField] private AudioSource WeaponAudioSource;
    public Vector3 hitPos { private set; get; }

    public bool reloading = false;
    bool hipfire = false;

    public GameObject Magazine;
    public GameObject WeaponMagazine;

    public bool isPistol;

    public delegate void OnItemInteract();
    public OnItemInteract onItemInteractCallback;
    void Awake()
    {
        instance = this;
        Charactercontroller = GetComponent<ThirdPersonController>();
        Inputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 ScreenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        MouseWorldPosition = Vector3.zero;
        Ray cameraRay = Camera.main.ScreenPointToRay(ScreenCenter);
        if (Physics.Raycast(cameraRay, out RaycastHit raycasthit1, 999f, AimColliderMask))
        {
            MouseWorldPosition = raycasthit1.point;
            hitPos = raycasthit1.point;
            aimPos.position = Vector3.Lerp(aimPos.position, raycasthit1.point, 20f * Time.deltaTime);
        }

        if (Inputs.aim && canAttack)
        {

            BodyAim.weight = Mathf.Lerp(BodyAim.weight, 0.5f, 10f * Time.deltaTime);
            HeadAim.weight = Mathf.Lerp(HeadAim.weight, 1f, 10f * Time.deltaTime);
            RHandAim.weight = Mathf.Lerp(RHandAim.weight, 1f, 10f * Time.deltaTime);
            if(!reloading)
            {
                if (!isPistol)
                {
                    LHandIK_Rifle.weight = Mathf.Lerp(LHandIK_Rifle.weight, 0f, 10f * Time.deltaTime);
                    LHandIKAimed_Rifle.weight = Mathf.Lerp(LHandIKAimed_Rifle.weight, 1f, 10f * Time.deltaTime);

                    LHandIK_Pistol.weight = Mathf.Lerp(LHandIK_Pistol.weight, 0f, 10f * Time.deltaTime);
                    LHandIKAimed_Pistol.weight = Mathf.Lerp(LHandIKAimed_Pistol.weight, 0f, 10f * Time.deltaTime);

                    animator.SetLayerWeight(2, Mathf.Lerp(animator.GetLayerWeight(2), 0f, Time.deltaTime * 10f));
                    animator.SetLayerWeight(3, Mathf.Lerp(animator.GetLayerWeight(3), 0f, Time.deltaTime * 10f));

                    animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
                    animator.SetLayerWeight(0, Mathf.Lerp(animator.GetLayerWeight(0), 0f, Time.deltaTime * 10f));
                }
                else
                {
                    LHandIK_Rifle.weight = Mathf.Lerp(LHandIK_Rifle.weight, 0f, 10f * Time.deltaTime);
                    LHandIKAimed_Rifle.weight = Mathf.Lerp(LHandIKAimed_Rifle.weight, 0f, 10f * Time.deltaTime);

                    LHandIK_Pistol.weight = Mathf.Lerp(LHandIK_Pistol.weight, 0f, 10f * Time.deltaTime);
                    LHandIKAimed_Pistol.weight = Mathf.Lerp(LHandIKAimed_Pistol.weight, 1f, 10f * Time.deltaTime);

                    animator.SetLayerWeight(0, Mathf.Lerp(animator.GetLayerWeight(0), 0f, Time.deltaTime * 10f));
                    animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));

                    animator.SetLayerWeight(2, Mathf.Lerp(animator.GetLayerWeight(2), 1f, Time.deltaTime * 10f));
                    animator.SetLayerWeight(3, Mathf.Lerp(animator.GetLayerWeight(3), 1f, Time.deltaTime * 10f));
                }
            }


            aimed = true;
            Crosshair.SetActive(true);
            aimVirtualCamera.gameObject.SetActive(true);
            Charactercontroller.ChangeSens(aimSensitivity);
            Charactercontroller.SetAimed(true);

            Vector3 WorldAim = MouseWorldPosition;
            WorldAim.y = transform.position.y;
            Vector3 aimDirection = (WorldAim - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
        else
        {
           BodyAim.weight = Mathf.Lerp(BodyAim.weight, 0f, 10f * Time.deltaTime);
           HeadAim.weight = Mathf.Lerp(HeadAim.weight, 0f, 10f * Time.deltaTime);
           RHandAim.weight = Mathf.Lerp(RHandAim.weight, 0f, 10f * Time.deltaTime);
            if (!reloading)
            {
                if (!isPistol)
                {
                    LHandIK_Rifle.weight = Mathf.Lerp(LHandIK_Rifle.weight, 1f, 10f * Time.deltaTime);
                    LHandIKAimed_Rifle.weight = Mathf.Lerp(LHandIKAimed_Rifle.weight, 0f, 10f * Time.deltaTime);

                    LHandIK_Pistol.weight = Mathf.Lerp(LHandIK_Pistol.weight, 0f, 10f * Time.deltaTime);
                    LHandIKAimed_Pistol.weight = Mathf.Lerp(LHandIKAimed_Pistol.weight, 0f, 10f * Time.deltaTime);

                    animator.SetLayerWeight(2, Mathf.Lerp(animator.GetLayerWeight(2), 0f, Time.deltaTime * 10f));
                    animator.SetLayerWeight(3, Mathf.Lerp(animator.GetLayerWeight(3), 0f, Time.deltaTime * 10f));

                    animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
                    animator.SetLayerWeight(0, Mathf.Lerp(animator.GetLayerWeight(0), 1f, Time.deltaTime * 10f));
                }
                else
                {
                    LHandIK_Rifle.weight = Mathf.Lerp(LHandIK_Rifle.weight, 0f, 10f * Time.deltaTime);
                    LHandIKAimed_Rifle.weight = Mathf.Lerp(LHandIKAimed_Rifle.weight, 0f, 10f * Time.deltaTime);

                    LHandIK_Pistol.weight = Mathf.Lerp(LHandIK_Pistol.weight, 1f, 10f * Time.deltaTime);
                    LHandIKAimed_Pistol.weight = Mathf.Lerp(LHandIKAimed_Pistol.weight, 0f, 10f * Time.deltaTime);

                    animator.SetLayerWeight(0, Mathf.Lerp(animator.GetLayerWeight(0), 0f, Time.deltaTime * 10f));
                    animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));

                    animator.SetLayerWeight(2, Mathf.Lerp(animator.GetLayerWeight(2), 1f, Time.deltaTime * 10f));
                    animator.SetLayerWeight(3, Mathf.Lerp(animator.GetLayerWeight(3), 0f, Time.deltaTime * 10f));
                }
            }

            aimed = false;
            Crosshair.SetActive(false);
            aimVirtualCamera.gameObject.SetActive(false);
            Charactercontroller.SetAimed(false);
            Charactercontroller.ChangeSens(NormalSensitivity);
        }

        if (reloading == true)
        {
            LHandIK_Rifle.weight = Mathf.Lerp(LHandIK_Rifle.weight, 0f, 10f * Time.deltaTime);
            LHandIKAimed_Rifle.weight = Mathf.Lerp(LHandIKAimed_Rifle.weight, 0f, 10f * Time.deltaTime);
            LHandIK_Pistol.weight = Mathf.Lerp(LHandIK_Pistol.weight, 0f, 10f * Time.deltaTime);
            LHandIKAimed_Pistol.weight = Mathf.Lerp(LHandIKAimed_Pistol.weight, 0f, 10f * Time.deltaTime);

            if (!isPistol)
            {
                animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
            }
            else
            {
                animator.SetLayerWeight(2, Mathf.Lerp(animator.GetLayerWeight(2), 0f, Time.deltaTime * 10f));
                animator.SetLayerWeight(3, Mathf.Lerp(animator.GetLayerWeight(3), 1f, Time.deltaTime * 10f));
            }
        }
        if (hipfire)
        {
            RHandAim.weight = 1f;
            if(!isPistol)
            {
                LHandIK_Rifle.weight = 0f;
                LHandIKAimed_Rifle.weight = 1f;
                animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
            }
            else
            {
                LHandIK_Pistol.weight = 0f;
                LHandIKAimed_Pistol.weight = 1f;
                animator.SetLayerWeight(3, Mathf.Lerp(animator.GetLayerWeight(3), 1f, Time.deltaTime * 10f));
            }
            Vector3 WorldAim = MouseWorldPosition;
            WorldAim.y = transform.position.y;
            Vector3 aimDirection = (WorldAim - transform.position).normalized;
            transform.forward = aimDirection;
        }

        if (Inputs.interact)
        {
            if (onItemInteractCallback != null)
                onItemInteractCallback.Invoke();

            Inputs.interact = false;
        }
        if(isPistol)
        {
            animator.SetBool("Pistol", true);
            animator.SetBool("Rifle", false);
        }
        else
        {
            animator.SetBool("Pistol", false);
            animator.SetBool("Rifle", true);
        }

    }

    public void SwitchWeapon(bool pistol)
    {
        isPistol = pistol;
    }

    public void DisableAttacking()
    {
        canAttack = false;
        aimVirtualCamera.gameObject.SetActive(false);
        Charactercontroller.SetRotateOnMove(true);
        Charactercontroller.SetAimed(false);
        Charactercontroller.ChangeSens(NormalSensitivity);
        animator.SetLayerWeight(2, Mathf.Lerp(animator.GetLayerWeight(2), 0f, Time.deltaTime * 10f));
    }

    public void Shake(float intensity, float time, float fr)
    {
        GameObject[] ShakeObjects = GameObject.FindGameObjectsWithTag("Shake");
        foreach (GameObject obj in ShakeObjects)
        {
            if (obj.GetComponent<CameraShake>() != null)
            {
                obj.GetComponent<CameraShake>().SetCameraShake(intensity, time, fr);
            }
        }
    }

    public void AimToTarget()
    {
        hipfire = true;
        BodyAim.weight = 1f;
        HeadAim.weight = 1f;
        Invoke("StopAimToTarget", 0.7f);
    }

    void StopAimToTarget()
    {
        hipfire = false;
    }

    private void MagazineOut()
    {
        WeaponManager manager = GetComponentInChildren<WeaponManager>();
        if (manager)
        {
            manager.MagazineOut();
        }
    }

    private void Load()
    {
        WeaponManager manager = GetComponentInChildren<WeaponManager>();
        if (manager)
        {
            manager.Load();
        }
    }

    private void LoadEnd()
    {
        WeaponManager manager = GetComponentInChildren<WeaponManager>();
        if (manager)
        {
            manager.LoadEnd();
        }
    }

    private void ReloadEnd()
    {
        reloading = false;
    }

}
