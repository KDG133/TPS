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

    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;
    [SerializeField] private Rig aimRig;
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private GameObject Crosshair;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private float fireRate;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private GameObject muzzleLight;
    [SerializeField] private Transform spawnBulletPosition;
    [SerializeField] private TrailRenderer BulletTrail;

    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;
    private WeaponManager weaponManager;

    [SerializeField] private Firearms firearms;

    private Animator animator;
    private float aimRigWeight;
    private Vector3 mouseWorldPosition = Vector3.zero;
    private Transform hitTransform = null;
    [SerializeField] private bool canFire = true;

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
        weaponManager = GetComponent<WeaponManager>();
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
    }

    private void Shoot()
    {
        if (starterAssetsInputs.shoot)
        {
            firearms.Shoot();
            //if (canFire)
            //{
            //    muzzleLight.SetActive(true);
            //    StartCoroutine(Fire(hitTransform, mouseWorldPosition));
            //    StartCoroutine(SpawnTrail(mouseWorldPosition));
            //    CinemachineShake.Instance.ShakeCamera(.7f, 60.0f / fireRate);
            //}
        }
    }

    private void Reload()
    {
        if (starterAssetsInputs.reload)
        {
            animator.SetTrigger("Reload");
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
        yield return new WaitForSeconds(timeToNextFire);
        canFire = true;
        muzzleLight.SetActive(false);
    }

    IEnumerator SpawnTrail(Vector3 mouseWorldPoint)
    {
        TrailRenderer trail = Instantiate(BulletTrail, spawnBulletPosition.position, Quaternion.identity);

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
            Health -= 10f;
        }
    }
}
