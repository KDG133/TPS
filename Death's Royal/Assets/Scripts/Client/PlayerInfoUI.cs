using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    private MyTPController _myPlayer;
    private MyTPSController tpsController;
    private MyWeaponChanger myWeaponchanger;
    public Transform uiPos;
    public Transform playerInfo;
    public Image healthBar;
    public TextMeshProUGUI remainAmmotxt;
    public TextMeshProUGUI maxAmmotxt;

    public float lerpSpeed;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        FindMyPlayer();
        lerpSpeed = 3f * Time.deltaTime;
        FollowUI();
        HealthBarFill();
        ColorChange();
        AmmoInfo();
    }

    private void LateUpdate()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(uiPos.position);
        screenPos.x += 150f;
        playerInfo.position = screenPos;
    }

    void HealthBarFill()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, tpsController.Health / tpsController.MaxHealth, lerpSpeed);
    }

    void ColorChange()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, tpsController.Health / tpsController.MaxHealth);
        healthBar.color = healthColor;
    }

    void AmmoInfo()
    {
        maxAmmotxt.text = myWeaponchanger.CurrentFirearm.MaxAmmo.ToString();
        remainAmmotxt.text = myWeaponchanger.CurrentFirearm.RemainingAmmo.ToString();
    }

    void FollowUI()
    {
        uiPos = _myPlayer.transform.GetChild(1).transform;
    }

    void FindMyPlayer()
    {
        while (_myPlayer == null || tpsController == null)
        {
            _myPlayer = GameObject.Find("MyPlayer").GetComponent<MyTPController>();
            myWeaponchanger = GameObject.Find("MyPlayer").GetComponent<MyWeaponChanger>();
            tpsController = _myPlayer.GetComponent<MyTPSController>();
        }
    }
}
