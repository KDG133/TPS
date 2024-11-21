using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.Animations.Rigging;

public class ThirdPersonShooterController : MonoBehaviour
{
    public float Health = 100f;
    public float MaxHealth = 100f;

    [SerializeField] private Rig aimRig;
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private GameObject Crosshair;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;

    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;
    private Animator animator;
    private float aimRigWeight;
    private Vector3 mouseWorldPosition = Vector3.zero;
    private Transform hitTransform = null;
    private float reloadSpeed = 1.0f;
    private float reloadPlusRatio = 0.25f;

    public Vector3 playerMouseWorldPosition
    {
        get { return mouseWorldPosition; }
    }

    public Transform playerHitTransform
    {
        get { return hitTransform; }
    }

    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Raycast();
        Aim();
        Shoot();
        Reload();
    }

    private void Raycast()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999.0f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            hitTransform = raycastHit.transform;
        }
    }

    private void Aim()
    {
        if (starterAssetsInputs.aim)
        {
            WeaponManager.Instance.isAiming = true;
            Crosshair.SetActive(true);
            aimRigWeight = 1f;
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
        else
        {
            WeaponManager.Instance.isAiming = false;
            aimRigWeight = 0f;
            Crosshair.SetActive(false);
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }
        aimRig.weight = Mathf.Lerp(aimRig.weight, aimRigWeight, Time.deltaTime * 20f);
    }

    private void Shoot()
    {
        if (starterAssetsInputs.aim && starterAssetsInputs.shoot)
        {
            WeaponManager.Instance.CurrentFirearm.Shoot();
        }
    }

    private void Reload()
    {
        bool checkReload = WeaponManager.Instance.CurrentFirearm.MaxAmmo > WeaponManager.Instance.CurrentFirearm.RemainingAmmo;

        animator.SetFloat("ReloadSpeed", reloadSpeed + (reloadPlusRatio * Managers.Upgrade.reloadPoint));
        if (starterAssetsInputs.reload && checkReload && !WeaponManager.Instance.CurrentFirearm.Reloading)
        {
            animator.SetTrigger("Reload");
            WeaponManager.Instance.CurrentFirearm.Reload();
        }
    }

    private void End_Reload()
    {
        WeaponManager.Instance.CurrentFirearm.EndReload();
    }

    private void walkLeft()
    {
        if (thirdPersonController._applyspeed < 4f)
            Managers.Sound.PlaySound2D("robot_walk");
    }

    private void walkRight()
    {
        if (thirdPersonController._applyspeed < 4f)
            Managers.Sound.PlaySound2D("robot_walk");
    }

    private void runLeft()
    {
        if (thirdPersonController._applyspeed >= 4f)
            Managers.Sound.PlaySound2D("robot_walk");
    }

    private void runRight()
    {
        if (thirdPersonController._applyspeed >= 4f)
            Managers.Sound.PlaySound2D("robot_walk");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("MonsterCol"))
        {
            Debug.Log("Hit");
            Health -= 10f;
        }
    }
}
