using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject explaneTxt;

    public void OnPointerEnter(PointerEventData eventData)
    {
        explaneTxt.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        explaneTxt.SetActive(false);
    }
}
