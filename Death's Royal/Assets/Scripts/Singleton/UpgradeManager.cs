using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
    public int upgradePoint = 0;
    public int magazinePoint = 0;
    public int reloadPoint = 0;
    public int moveSpeedPoint = 0;
    public int fireRatePoint = 0;
    private int maxUpgradePoint = 4;
    private int maxPoint = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(upgradePoint >= maxPoint)
            upgradePoint = maxPoint;

        ThirdPersonController._movespeedPoint = moveSpeedPoint;
    }

    public void Upgrade_Magazine()
    {
        if (magazinePoint < maxUpgradePoint)
        {
            ++magazinePoint;
        }
    }

    public void Upgrade_ReloadSpeed()
    {
        if (reloadPoint < maxUpgradePoint)
        {
            ++reloadPoint;
        }
    }

    public void Upgrade_MoveSpeed()
    {
        if (moveSpeedPoint < maxUpgradePoint)
        {
            ++moveSpeedPoint;
        }
    }

    public void Upgrade_FireRate()
    {
        if (fireRatePoint < maxUpgradePoint)
        {
            ++fireRatePoint;
        }
    }
}
