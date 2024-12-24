using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.Animations.Rigging;
using Unity.VisualScripting;

public class ThirdPersonShooterController : ObjectController
{
    public float Health = 100f;
    public float MaxHealth = 100f;

    [SerializeField] protected Rig aimRig;
    [SerializeField] protected CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] protected GameObject Crosshair;
    [SerializeField] protected float normalSensitivity;
    [SerializeField] protected float aimSensitivity;
    [SerializeField] protected LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] protected Transform _aimSpot;

    protected StarterAssetsInputs starterAssetsInputs;
    protected ThirdPersonController thirdPersonController;
    protected WeaponChanger weaponChanger;
    protected Animator animator;
    protected float aimRigWeight;
    protected Vector3 mouseWorldPosition = Vector3.zero;
    protected Transform hitTransform = null;
    protected float reloadSpeed = 1.0f;
    protected float reloadPlusRatio = 0.25f;
    protected bool isAim = false;
    protected bool isReload = false;
    protected bool isShot = false;
    protected bool pervShoot = false;

    public bool playerAim
    {
        get { return isAim; }
        set { isAim = value; }
    }

    public bool playerReload
    {
        get { return isReload; }
        set { isReload = value; }
    }

    public bool playerShot
    {
        get { return isShot; }
        set { isShot = value; }
    }

    public Transform aimSpot
    {
        get { return _aimSpot; }
    }

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
        //starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        //thirdPersonController = GetComponent<ThirdPersonController>();
        weaponChanger = GetComponent<WeaponChanger>();
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
        mouseWorldPosition = _aimSpot.position;
        //hitTransform = raycastHit.transform;
    }

    private void Aim()
    {
        if (isAim)
        {
            aimRigWeight = 1f;
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
        }
        else
        {
            aimRigWeight = 0f;
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }
        aimRig.weight = Mathf.Lerp(aimRig.weight, aimRigWeight, Time.deltaTime * 20f);
    }

    private void Shoot()
    {
        if (isShot)
        {
            weaponChanger.CurrentFirearm.Shoot();
        }
    }

    private void Reload()
    {
        animator.SetFloat("ReloadSpeed", reloadSpeed /*+ (reloadPlusRatio * Managers.Upgrade.reloadPoint)*/);
        if (isReload)
        {
            isReload = false;
            animator.SetTrigger("Reload");
        }
    }

    private void End_Reload()
    {
        weaponChanger.CurrentFirearm.EndReload();
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
