using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    MyPlayer _myPlayer;
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
        StartCoroutine("FindmyPlayer");
    }

    // Update is called once per frame
    void Update()
    {
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
        maxAmmotxt.text = WeaponManager.Instance.CurrentFirearm.MaxAmmo.ToString();
        remainAmmotxt.text = WeaponManager.Instance.CurrentFirearm.RemainingAmmo.ToString();
    }

    void FollowUI()
    {
        uiPos = _myPlayer.transform.GetChild(1).transform;
    }

    IEnumerator FindmyPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);

            _myPlayer = GameObject.Find("MyPlayer").GetComponent<MyPlayer>();
            tpsController = _myPlayer.GetComponent<ThirdPersonShooterController>();
            
        }
    }
}
