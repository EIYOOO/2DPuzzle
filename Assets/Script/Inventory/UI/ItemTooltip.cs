using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image[] itemNameImage;
    private bool isPointerOverTooltip = false;

    public void UpdateItemName(ItemName itemName)
    {
        foreach (Transform child in transform)
        {
            if (child.name == itemName.ToString())
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOverTooltip = true; 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOverTooltip = false;
    }

    public bool IsPointerOverTooltip()
    {
        return isPointerOverTooltip;
    }
}
