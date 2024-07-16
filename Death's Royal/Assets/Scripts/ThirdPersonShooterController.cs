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
    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;
    [SerializeField] private Rig aimRig;
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private GameObject Crosshair;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private float fireRate; //for test
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private GameObject muzzleLight;
    [SerializeField] private Transform spawnBulletPosition;
    [SerializeField] private TrailRenderer BulletTrail;

    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;
    private Animator animator;
    private float aimRigWeight;
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
        Transform hitTransform = null;
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999.0f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            hitTransform = raycastHit.transform;
        }

        if (starterAssetsInputs.aim)
        {
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
            aimRigWeight = 0f;
            Crosshair.SetActive(false);
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }
        aimRig.weight = Mathf.Lerp(aimRig.weight, aimRigWeight, Time.deltaTime * 20f);

        if (starterAssetsInputs.shoot)
        {
            if (canFire)
            {
                muzzleLight.SetActive(true);
                TrailRenderer trail = Instantiate(BulletTrail, spawnBulletPosition.position, Quaternion.identity);
                //Vector3 aimDirection = (mouseWorldPosition - spawnBulletPosition.position).normalized;
                StartCoroutine(Fire(hitTransform, mouseWorldPosition));
                StartCoroutine(SpawnTrail(trail, mouseWorldPosition));
            }
        }
    }

    IEnumerator Fire(Transform hitTransform, Vector3 mouseWorldPoint)
    {
        canFire = false;
        if (hitTransform != null)
        {
            if (hitTransform.GetComponent<BulletTarget>() != null)
            {
                Instantiate(vfxHitGreen, mouseWorldPoint, Quaternion.identity);
            }
            else
            {
                Instantiate(vfxHitRed, mouseWorldPoint, Quaternion.identity);
            }
        }
        StartCoroutine(FireRateHandler());
        yield return null;
    }

    IEnumerator FireRateHandler()
    {
        float timeToNextFire = 60 / fireRate;
        CinemachineShake.Instance.ShakeCamera(.7f, timeToNextFire);
        yield return new WaitForSeconds(timeToNextFire);
        canFire = true;
        muzzleLight.SetActive(false);
    }

    IEnumerator SpawnTrail(TrailRenderer trail, Vector3 mouseWorldPoint)
    {
        float time = 0;
        float timeToNextFire = 60 / fireRate;
        Vector3 startPosition = trail.transform.position;

        while (time < timeToNextFire)
        {
            trail.transform.position = Vector3.Lerp(startPosition, mouseWorldPoint, time / timeToNextFire);
            time += Time.deltaTime;

            yield return null;
        }

        Destroy(trail.gameObject, trail.time);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("MonsterCol"))
        {
            Debug.Log("Hit");
        }
    }
}
