using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChanger : MonoBehaviour
{
    [SerializeField] protected Firearms[] firearms;
    [SerializeField] protected Firearms currentFirearm = null;
    protected GunType gunType;
    public bool isAiming {  get; set; }

    public Firearms CurrentFirearm
    {
        get { return currentFirearm; }
    }

    public GunType CurrentGunType
    {
        get { return gunType; }
        set { gunType = value; }
    }

    // Start is called before the first frame update
    private void Start()
    {
        gunType = GunType.Ar;
        currentFirearm = firearms[0];
    }

    // Update is called once per frame
    private void Update()
    {
        switch (gunType)
        {
            case GunType.Ar:
                currentFirearm = firearms[0];
                firearms[0].gameObject.SetActive(true);
                firearms[1].gameObject.SetActive(false);
                firearms[2].gameObject.SetActive(false);
                break;
            case GunType.Sg:
                currentFirearm = firearms[1];
                firearms[1].gameObject.SetActive(true);
                firearms[0].gameObject.SetActive(false);
                firearms[2].gameObject.SetActive(false);
                break;
            case GunType.Smg:
                currentFirearm = firearms[2];
                firearms[2].gameObject.SetActive(true);
                firearms[0].gameObject.SetActive(false);
                firearms[1].gameObject.SetActive(false);
                break;
        }
    }
}
