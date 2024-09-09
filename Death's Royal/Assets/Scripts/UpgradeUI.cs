using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    public UpgradeManager.UpgradeType upgradeType;
    public Sprite change;
    [SerializeField] private Image[] images;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (upgradeType)
        {
            case UpgradeManager.UpgradeType.MAGAZINE:
                for (int i = 0; i < UpgradeManager.Instance.magazinePoint; i++)
                    images[i].sprite = change;
                return;
            case UpgradeManager.UpgradeType.RELOAD:
                for (int i = 0; i < UpgradeManager.Instance.reloadPoint; i++)
                    images[i].sprite = change;
                return;
            case UpgradeManager.UpgradeType.MOVESPEED:
                for (int i = 0; i < UpgradeManager.Instance.moveSpeedPoint; i++)
                    images[i].sprite = change;
                return;
            case UpgradeManager.UpgradeType.FIRERATE:
                for (int i = 0; i < UpgradeManager.Instance.fireRatePoint; i++)
                    images[i].sprite = change;
                return;
        }     
    }
}
