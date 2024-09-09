using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
    public enum UpgradeType { MAGAZINE, RELOAD, MOVESPEED, FIRERATE, END };
    public TextMeshProUGUI remainPoints;
    public int upgradePoint = 0;
    public int magazinePoint = 0;
    public int reloadPoint = 0;
    public int moveSpeedPoint = 0;
    public int fireRatePoint = 0;
    private int maxUpgradePoint = 4;
    private int maxPoint = 100;
    private int upgradeCost = 5;
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
        remainPoints.text = upgradePoint.ToString();
    }

    public void Upgrade_Magazine()
    {
        if (magazinePoint < maxUpgradePoint && upgradePoint >= upgradeCost)
        {
            upgradePoint -= upgradeCost;
            ++magazinePoint;
        }
    }

    public void Upgrade_ReloadSpeed()
    {
        if (reloadPoint < maxUpgradePoint && upgradePoint >= upgradeCost)
        {
            upgradePoint -= upgradeCost;
            ++reloadPoint;
        }
    }

    public void Upgrade_MoveSpeed()
    {
        if (moveSpeedPoint < maxUpgradePoint && upgradePoint >= upgradeCost)
        {
            upgradePoint -= upgradeCost;
            ++moveSpeedPoint;
        }
    }

    public void Upgrade_FireRate()
    {
        if (fireRatePoint < maxUpgradePoint && upgradePoint >= upgradeCost)
        {
            upgradePoint -= upgradeCost;
            ++fireRatePoint;
        }
    }
}
