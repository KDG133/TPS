using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firearms : MonoBehaviour
{
    public enum gunType { AR, SMG, SG, END }
    [SerializeField] private ThirdPersonShooterController tpsController;
    [SerializeField] private Transform vfxHitRed;
    [SerializeField] private Transform vfxHitYellow;
    [SerializeField] private GameObject muzzleLight;
    [SerializeField] private Transform spawnBulletPosition;
    [SerializeField] private TrailRenderer BulletTrail;
    [SerializeField] private gunType GunType;
    [SerializeField] private float fireRate;
    [SerializeField] private int maxAmmo = 0;
    [SerializeField] private int remainingAmmo = 0;
    [SerializeField] private float camIntensity = 0;

    [SerializeField] private int shotgunPellets = 0;
    [SerializeField] private float spreadAngle = 0;
    [SerializeField] private float range = 0;
    private bool canFire = true;
    private bool reloading = false;
    private Vector3 mouseWorldPosition = Vector3.zero;
    private Transform hitTransform = null;

    public float MaxAmmo
    {
        get { return maxAmmo; }
    }
    public float RemainingAmmo
    {
        get { return remainingAmmo; }
    }
    // Start is called before the first frame update
    void Start()
    {
        remainingAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        hitTransform = tpsController.playerHitTransform;
        mouseWorldPosition = tpsController.playerMouseWorldPosition;
    }

    public void Shoot()
    {
        if (canFire && remainingAmmo > 0 && !reloading)
        {
            if (GunType == gunType.SG)
            {
                muzzleLight.SetActive(true);
                StartCoroutine(ShotGunFire(mouseWorldPosition));
                CinemachineShake.Instance.ShakeCamera(.7f, 60.0f / fireRate);
            }
            else
            {
                muzzleLight.SetActive(true);
                StartCoroutine(Fire(hitTransform, mouseWorldPosition));
                StartCoroutine(SpawnTrail(mouseWorldPosition));
                CinemachineShake.Instance.ShakeCamera(.7f, 60.0f / fireRate);
            }
        }              
    }

    public void Reload()
    {
        reloading = true;
    }

    //Animation Event Function
    public void EndReload()
    {
        reloading = false;
        remainingAmmo = maxAmmo;
    }

    IEnumerator Fire(Transform hitTransform, Vector3 mouseWorldPoint)
    {
        remainingAmmo -= 1;
        canFire = false;
        if (hitTransform != null)
        {
            BulletTarget target = hitTransform.GetComponent<BulletTarget>();
            if (target != null)
            {
                target.Hit(1.0f);
                Instantiate(vfxHitRed, mouseWorldPoint, Quaternion.identity);
            }
            else
            {
                Instantiate(vfxHitYellow, mouseWorldPoint, Quaternion.identity);
            }
        }
        StartCoroutine(FireRateHandler());
        yield return null;
    }

    IEnumerator ShotGunFire(Vector3 mouseWorldPoint)
    {
        remainingAmmo -= 1;
        canFire = false;

        for(int i = 0; i < shotgunPellets; i++) {
            Vector3 aimDirection = (mouseWorldPoint - spawnBulletPosition.position).normalized;
            Vector3 randomDirection = aimDirection + new Vector3(Random.Range(-spreadAngle, spreadAngle),
                                                                 Random.Range(-spreadAngle, spreadAngle),
                                                                 0);
            randomDirection.Normalize();

            RaycastHit hit;
            if (Physics.Raycast(spawnBulletPosition.position, randomDirection, out hit, range))
            {
                BulletTarget target = hit.transform.GetComponent<BulletTarget>();
                if (target != null)
                {
                    target.Hit(1.0f);
                    Instantiate(vfxHitRed, hit.point, Quaternion.identity);
                }
                else
                {
                    Instantiate(vfxHitYellow, hit.point, Quaternion.identity);
                }
            }
            StartCoroutine(SpawnTrail(hit.point));
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
        GameObject trail = BulletManager.Instance.GetQueue();
        trail.transform.position = spawnBulletPosition.position;

        float time = 0;
        float timeToNextFire = 60 / fireRate;
        Vector3 startPosition = trail.transform.position;

        while (time < timeToNextFire)
        {
            trail.transform.position = Vector3.Lerp(startPosition, mouseWorldPoint, time / 0.1f);
            time += Time.deltaTime;

            yield return null;
        }

        BulletManager.Instance.insertQueue(trail);
    }
}
