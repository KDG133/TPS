using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firearms : MonoBehaviour
{
    public enum gunType { AR, SMG, SG, END }
    [SerializeField] private ThirdPersonShooterController tpsController;
    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;
    [SerializeField] private GameObject muzzleLight;
    [SerializeField] private Transform spawnBulletPosition;
    [SerializeField] private TrailRenderer BulletTrail;
    [SerializeField] private gunType GunType;
    [SerializeField] private float fireRate;
    [SerializeField] private int maxAmmo = 0;
    [SerializeField] private int remainingAmmo = 0;
    private bool canFire = true;
    private Vector3 mouseWorldPosition = Vector3.zero;
    private Transform hitTransform = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hitTransform = tpsController.playerHitTransform;
        mouseWorldPosition = tpsController.playerMouseWorldPosition;
    }

    public void Shoot()
    {
        if (canFire)
        {
            muzzleLight.SetActive(true);
            StartCoroutine(Fire(hitTransform, mouseWorldPosition));
            StartCoroutine(SpawnTrail(mouseWorldPosition));
            CinemachineShake.Instance.ShakeCamera(.7f, 60.0f / fireRate);
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
        float timeToNextFire = 60f / fireRate;
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
            trail.transform.position = Vector3.Lerp(startPosition, mouseWorldPoint, time / 0.05f);
            time += Time.deltaTime;

            yield return null;
        }

        Destroy(trail.gameObject, trail.time);
    }
}
