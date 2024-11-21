using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeManager
{
    public enum UpgradeType { MAGAZINE, RELOAD, MOVESPEED, FIRERATE, END };
    public TextMeshProUGUI remainPoints;
    public WarningText warningText;
    public int upgradePoint = 0;
    public int magazinePoint = 0;
    public int reloadPoint = 0;
    public int moveSpeedPoint = 0;
    public int fireRatePoint = 0;
    private int maxUpgradePoint = 4;
    private int maxPoint = 100;
    private int upgradeCost = 5;
    // Start is called before the first frame update
    public void Init()
    {
        remainPoints = GameObject.Find("RemainPoint").GetComponent<TextMeshProUGUI>();
        warningText = GameObject.Find("PlayerCanvas").transform.Find("ShopUI").transform.Find("Point Warning").GetComponent<WarningText>();
    }

    // Update is called once per frame
    public void Update()
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
            Managers.Sound.PlaySound2D("ui_upgrade");
            upgradePoint -= upgradeCost;
            ++magazinePoint;
        }
        else
        {
            Managers.Sound.PlaySound2D("ui_denied");
            warningText.ActiveWarning();
        }
    }

    public void Upgrade_ReloadSpeed()
    {
        if (reloadPoint < maxUpgradePoint && upgradePoint >= upgradeCost)
        {
            Managers.Sound.PlaySound2D("ui_upgrade");
            upgradePoint -= upgradeCost;
            ++reloadPoint;
        }
        else
        {
            Managers.Sound.PlaySound2D("ui_denied");
            warningText.ActiveWarning();
        }
    }

    public void Upgrade_MoveSpeed()
    {
        if (moveSpeedPoint < maxUpgradePoint && upgradePoint >= upgradeCost)
        {
            Managers.Sound.PlaySound2D("ui_upgrade");
            upgradePoint -= upgradeCost;
            ++moveSpeedPoint;
        }
        else
        {
            Managers.Sound.PlaySound2D("ui_denied");
            warningText.ActiveWarning();
        }
    }

    public void Upgrade_FireRate()
    {
        if (fireRatePoint < maxUpgradePoint && upgradePoint >= upgradeCost)
        {
            Managers.Sound.PlaySound2D("ui_upgrade");
            upgradePoint -= upgradeCost;
            ++fireRatePoint;
        }
        else
        {
            Managers.Sound.PlaySound2D("ui_denied");
            warningText.ActiveWarning();
        }
    }
}
