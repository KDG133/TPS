using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    public Transform uiPos;
    public Transform test;
    public Image healthBar;
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
    }

    private void LateUpdate()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(uiPos.position);
        screenPos.x += 150f;
        test.position = screenPos;
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
}
