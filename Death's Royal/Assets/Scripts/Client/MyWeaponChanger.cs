using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyWeaponChanger : WeaponChanger
{
    // Start is called before the first frame update
    private void Start()
    {
        gunType = GunType.Ar;
        currentFirearm = firearms[0];
    }

    // Update is called once per frame
    private void Update()
    {
        if (!currentFirearm.Reloading && !isAiming)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                gunType = GunType.Ar;
                currentFirearm = firearms[0];
                firearms[0].gameObject.SetActive(true);
                firearms[1].gameObject.SetActive(false);
                firearms[2].gameObject.SetActive(false);
                SendWeaponState();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                gunType = GunType.Sg;
                currentFirearm = firearms[1];
                firearms[1].gameObject.SetActive(true);
                firearms[0].gameObject.SetActive(false);
                firearms[2].gameObject.SetActive(false);
                SendWeaponState();
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                gunType = GunType.Smg;
                currentFirearm = firearms[2];
                firearms[2].gameObject.SetActive(true);
                firearms[0].gameObject.SetActive(false);
                firearms[1].gameObject.SetActive(false);
                SendWeaponState();
            }
        }
    }

    private void SendWeaponState()
    {
        C_Weaponchange weaponchangePacket = new C_Weaponchange();
        weaponchangePacket.GunType = gunType;
        Managers.Network.Send(weaponchangePacket);
    }
}
