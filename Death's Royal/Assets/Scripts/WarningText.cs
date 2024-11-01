using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WarningText : MonoBehaviour
{
    private TextMeshProUGUI warningText;
    private bool isWarning = false;
    private float disapearTime = 0f;
    private float applyTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        warningText = GetComponent<TextMeshProUGUI>();
        disapearTime = applyTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (isWarning)
        {
            disapearTime -= Time.unscaledDeltaTime;
            warningText.alpha = disapearTime;
        }

        if (warningText.alpha <= 0)
        {
            isWarning = false;
            disapearTime = applyTime;
        }
    }

    public void ActiveWarning()
    {
        isWarning = true;
    }
}
