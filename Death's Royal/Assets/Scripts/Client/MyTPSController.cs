using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class MyTPSController : ThirdPersonShooterController
{
    void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<MyTPController>();
        animator = GetComponent<Animator>();
        Crosshair = GameObject.Find("PlayerCanvas").transform.Find("Crosshair").gameObject;
        aimVirtualCamera = GameObject.Find("PlayerAimCamera").GetComponent<CinemachineVirtualCamera>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Raycast();
        Aim();
        Shoot();
        Reload();
    }

    #region 플레이어 동작
    private void Raycast()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999.0f, aimColliderLayerMask))
        {
            mouseWorldPosition = raycastHit.point;
            hitTransform = raycastHit.transform;
        }
    }

    private void Aim()
    {
        if (starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            WeaponManager.Instance.isAiming = true;
            Crosshair.SetActive(true);
            aimRigWeight = 1f;
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
            aimVirtualCamera.gameObject.SetActive(false);
            WeaponManager.Instance.isAiming = false;
            aimRigWeight = 0f;
            Crosshair.SetActive(false);
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
    #endregion
}
