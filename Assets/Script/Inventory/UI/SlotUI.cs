using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image itemImage;
    public ItemTooltip tooltip;
    private ItemDetails currentItem;
    private bool isSelected;


    public void SetItem(ItemDetails itemDetails)
    {
        currentItem = itemDetails;
        this.gameObject.SetActive(true);
        itemImage.sprite = currentItem.itemSprite;
        itemImage.SetNativeSize();

        SetItemPosition(itemDetails.itemName);
    }

    private void SetItemPosition(ItemName itemName)
    {
        RectTransform rectTransform = itemImage.GetComponent<RectTransform>();
        Image image = itemImage.GetComponent<Image>();

        switch (itemName)
        {
            case ItemName.刷子:
                rectTransform.anchoredPosition = new Vector2(143, -253);
                image.raycastPadding = new Vector4(1179, 713, 1258, 635);
                break;
            case ItemName.刻刀:
                rectTransform.anchoredPosition = new Vector2(670, -253);
                image.raycastPadding = new Vector4(1014, 772, 1390, 696);
                break;
            case ItemName.洗牙器:
                rectTransform.anchoredPosition = new Vector2(-160, -242);
                image.raycastPadding = new Vector4(1270, 771, 1141, 696);
                break;
            case ItemName.锤子:
                rectTransform.anchoredPosition = new Vector2(460, -236);
                image.raycastPadding = new Vector4(1076, 713, 1326, 635);
                break;
            case ItemName.柠檬酸:
                rectTransform.anchoredPosition = new Vector2(-75, -144);
                image.raycastPadding = new Vector4(728,513,632,472);
                break;
            case ItemName.纯净水:
                rectTransform.anchoredPosition = new Vector2(30, -118);
                image.raycastPadding = new Vector4(653,513,653,472);
                break;
            case ItemName.编钟锤:
                rectTransform.anchoredPosition = new Vector2(-2142, -1606); // 示例坐标
                image.raycastPadding = new Vector4(-40691, -3942, 1100, 550);
                break;

            default:
                rectTransform.anchoredPosition = new Vector2(0, 0); 
                break;
        }
    }

    public void SetEmpty()
    {
        this.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isSelected = !isSelected;
        EventHandler.CallItemSelectedEvent(currentItem, isSelected);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentItem != null && this.gameObject.activeInHierarchy)
        {
            tooltip.transform.SetAsLastSibling(); 
            tooltip.gameObject.SetActive(true);
            tooltip.UpdateItemName(currentItem.itemName); 
            Debug.Log("Tooltip 激活: " + currentItem.itemName);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentItem != null && tooltip.gameObject.activeInHierarchy)
        {
            if (!tooltip.IsPointerOverTooltip())
            {
                Debug.Log("Tooltip 失活");
                tooltip.gameObject.SetActive(false);
            }
        }
    }

    private bool IsPointerOverSlotUI()
    {
        RectTransform rectTransform = this.GetComponent<RectTransform>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, null, out localPoint);

        return rectTransform.rect.Contains(localPoint);
    }
    
}
