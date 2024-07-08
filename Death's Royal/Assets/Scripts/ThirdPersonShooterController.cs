using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private GameObject Crosshair;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private float fireRate; //for test
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform bulletProjectile;
    [SerializeField] private Transform gunFire;
    [SerializeField] private Transform spawnBulletPosition;

    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;
    private Animator animator;
    [SerializeField] private bool canFire = true;

    private void Awake()
    {
        //aimVirtualCamera = GameObject.Find("PlayerAimCamera").GetComponent<CinemachineVirtualCamera>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999.0f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }

        if (starterAssetsInputs.aim)
        {
            Crosshair.SetActive(true);
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
            Crosshair.SetActive(false);
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }

        if (starterAssetsInputs.shoot)
        {
            if (canFire)
            {
                Vector3 aimDirection = (mouseWorldPosition - spawnBulletPosition.position).normalized;
                StartCoroutine(Fire(aimDirection));
            }
        }
    }

    IEnumerator Fire(Vector3 aimDir)
    {
        canFire = false;
        Instantiate(bulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        //Instantiate(gunFire, spawnBulletPosition.position, Quaternion.LookRotation(aimDirection, Vector3.up));
        StartCoroutine(FireRateHandler());
        yield return null;
    }

    IEnumerator FireRateHandler()
    {
        float timeToNextFire = 60 / fireRate;
        CinemachineShake.Instance.ShakeCamera(.7f, timeToNextFire);
        yield return new WaitForSeconds(timeToNextFire);
        canFire = true;
    }
}
