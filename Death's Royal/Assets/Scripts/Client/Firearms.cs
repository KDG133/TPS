using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firearms : MonoBehaviour
{
    public enum gunType { AR, SMG, SG, END }
    [SerializeField] private MyTPSController tpsController;
    [SerializeField] private Transform vfxHitRed;
    [SerializeField] private Transform vfxHitYellow;
    [SerializeField] private GameObject muzzleLight;
    [SerializeField] private Transform spawnBulletPosition;
    [SerializeField] private TrailRenderer BulletTrail;
    [SerializeField] private gunType GunType;

    [SerializeField] private float Firerate;
    private float firerateRatio = 0.125f;
    [SerializeField] private float applyFirerate;

    [SerializeField] private int maxAmmo = 0;
    private float AmmoRatio = 0.25f;
    private int applyMaxAmmo;
    [SerializeField] private int remainingAmmo = 0;

    [SerializeField] private int shotgunPellets = 0;
    [SerializeField] private float spreadAngle = 0;
    [SerializeField] private float range = 0;
    [SerializeField] private float camIntensity = 0.7f;

    private bool canReload = true;
    private bool canFire = true;
    private bool reloading = false;
    private Vector3 mouseWorldPosition = Vector3.zero;
    private Transform hitTransform = null;

    public float MaxAmmo
    {
        get { return applyMaxAmmo; }
    }
    public float RemainingAmmo
    {
        get { return remainingAmmo; }
    }
    public bool Reloading
    {
        get { return reloading; }
    }

    // Start is called before the first frame update
    void Start()
    {
        remainingAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        FindTPSController();
        applyMaxAmmo = (int)(maxAmmo * (1.0f + (Managers.Upgrade.magazinePoint * AmmoRatio)));
        applyFirerate = Firerate * (1.0f + (Managers.Upgrade.fireRatePoint * firerateRatio));
        hitTransform = tpsController.playerHitTransform;
        mouseWorldPosition = tpsController.playerMouseWorldPosition;
    }

    public void Shoot()
    {
        if (canFire && remainingAmmo > 0 && !reloading)
        {
            switch (GunType)
            {
                case gunType.AR:
                    Managers.Sound.PlaySound2D("ar");
                    break;
                case gunType.SG:
                    Managers.Sound.PlaySound2D("shotgun");
                    break;
                case gunType.SMG:
                    Managers.Sound.PlaySound2D("mp5");
                    break;
            }

            if (GunType == gunType.SG)
            {
                muzzleLight.SetActive(true);
                StartCoroutine(ShotGunFire(mouseWorldPosition));
                CinemachineShake.Instance.ShakeCamera(camIntensity, 60.0f / applyFirerate);
            }
            else
            {
                muzzleLight.SetActive(true);
                StartCoroutine(Fire(hitTransform, mouseWorldPosition));
                StartCoroutine(SpawnTrail(mouseWorldPosition));
                CinemachineShake.Instance.ShakeCamera(camIntensity, 60.0f / applyFirerate);
            }
        }              
    }

    public void Reload()
    {
        reloading = true;
        if (canReload)
        {
            Managers.Sound.PlaySound2D("Reload");
            canReload = false;
        }
    }

    //Animation Event Function
    public void EndReload()
    {
        reloading = false;
        canReload = true;
        remainingAmmo = applyMaxAmmo;
    }

    void FindTPSController()
    {
        while (tpsController == null)
        {
            tpsController = GameObject.Find("MyPlayer").GetComponent<MyTPSController>();
        }
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
                GameObject effectHit = Managers.Effect.GetValue("vfxHitRed");
                effectHit.transform.position = mouseWorldPoint;
            }
            else
            {               
                GameObject effectHit = Managers.Effect.GetValue("vfxHitYellow");
                effectHit.transform.position = mouseWorldPoint;
            }
        }
        StartCoroutine(FirerateHandler());
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
                    GameObject effectHit = Managers.Effect.GetValue("vfxHitRed");
                    effectHit.transform.position = hit.point;
                }
                else
                {
                    GameObject effectHit = Managers.Effect.GetValue("vfxHitYellow");
                    effectHit.transform.position = hit.point;
                }
            }
            StartCoroutine(SpawnTrail(hit.point));
        }       

        StartCoroutine(FirerateHandler());
        yield return null;
    }

    IEnumerator FirerateHandler()
    {
        float timeToNextFire = 60f / applyFirerate;
        yield return new WaitForSeconds(timeToNextFire);
        canFire = true;
        muzzleLight.SetActive(false);
    }

    IEnumerator SpawnTrail(Vector3 mouseWorldPoint)
    {
        GameObject trail = BulletManager.Instance.GetQueue();
        trail.transform.position = spawnBulletPosition.position;

        float time = 0;
        float timeToNextFire = 60 / applyFirerate;
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
