using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    //[SerializeField] private GameObject[] upgradeBtns;
    [SerializeField] private CanvasScaler canvasScaler;
    // Start is called before the first frame update
    void Start()
    {
        float wRatio = Screen.width / canvasScaler.referenceResolution.x;
        float hRatio = Screen.height / canvasScaler.referenceResolution.y;
        gameObject.GetComponent<RectTransform>();

        float ratio = wRatio * (1f - canvasScaler.matchWidthOrHeight) + hRatio * (canvasScaler.matchWidthOrHeight);

        float pixelWidth = gameObject.GetComponent<RectTransform>().rect.width * ratio;
        float pixelHeight = gameObject.GetComponent<RectTransform>().rect.height * ratio;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
