using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    public Transform uiPos;
    public Transform playerInfo;
    public Image healthBar;
    public TextMeshProUGUI remainAmmotxt;
    public TextMeshProUGUI maxAmmotxt;
    public ThirdPersonShooterController tpsController;

    public float lerpSpeed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lerpSpeed = 3f * Time.deltaTime;
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
        maxAmmotxt.text = WeaponManager.Instance.CurrentFirearm.MaxAmmo.ToString();
        remainAmmotxt.text = WeaponManager.Instance.CurrentFirearm.RemainingAmmo.ToString();
    }
}
