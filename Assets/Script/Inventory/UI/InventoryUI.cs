using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Button leftButton, rightButton;
    public SlotUI slotUI;
    public int currentIndex;

    private int previousIndex = -1;

    private static bool _hasInitializedButtons = false;

    private void Start()
    {
        if (!_hasInitializedButtons)
        {
            _hasInitializedButtons = true;
            leftButton.interactable = false;
            rightButton.interactable = false;
        }
    }
    private void OnEnable()
    {
        EventHandler.UpdateUIEvent += OnUpdateUIEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
    }

    private void OnDisable()
    {
        EventHandler.UpdateUIEvent -= OnUpdateUIEvent;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
    }

    private void OnBeforeSceneUnloadEvent()
    {
        PlayerPrefs.SetInt("CurrentItemIndex", currentIndex);
        PlayerPrefs.Save();
    }

    private void OnAfterSceneLoadedEvent()
    {
        int savedIndex = PlayerPrefs.GetInt("CurrentItemIndex", -1);

        var itemList = InventoryManager.Instance.GetItemList();
        if (itemList.Count == 0)
        {
            leftButton.interactable = false;
            rightButton.interactable = false;
        }
        else
        {
            if (savedIndex >= 0 && savedIndex < itemList.Count)
            {
                currentIndex = savedIndex;
            }
            else
            {
                currentIndex = 0;
            }
            EventHandler.CallChangeItemEvent(currentIndex);
        }
    }

    private void OnUpdateUIEvent(ItemDetails itemDetails, int index)
    {
        if (itemDetails == null)
        {
            slotUI.SetEmpty();
            currentIndex = -1;
            leftButton.interactable = false;
            rightButton.interactable = false;
        }
        else
        {
            // ------------ 这里修复！！！------------
            // 永远以传进来的 index 为准
            currentIndex = index;

            slotUI.SetItem(itemDetails);
            UpdateButtonState();
        }
    }

    private void UpdateButtonState()
    {
        var itemList = InventoryManager.Instance.GetItemList();
        int total = itemList.Count;

        // ------------ 这里修复！！！------------
        // 只有 1 个物品 → 全部禁用
        if (total == 1)
        {
            leftButton.interactable = false;
            rightButton.interactable = false;
            return;
        }

        if (currentIndex < 0 || total == 0)
        {
            leftButton.interactable = false;
            rightButton.interactable = false;
        }
        else if (currentIndex <= 0)
        {
            leftButton.interactable = false;
            rightButton.interactable = true;
        }
        else if (currentIndex >= total - 1)
        {
            leftButton.interactable = true;
            rightButton.interactable = false;
        }
        else
        {
            leftButton.interactable = true;
            rightButton.interactable = true;
        }
    }

    public void SwitchItem(int amount)
    {
        var itemList = InventoryManager.Instance.GetItemList();
        int total = itemList.Count;

        if (total <= 0) return;

        // ------------ 这里修复！！！------------
        previousIndex = currentIndex;

        int newIndex = currentIndex + amount;
        newIndex = Mathf.Clamp(newIndex, 0, total - 1);

        currentIndex = newIndex;
        EventHandler.CallChangeItemEvent(currentIndex);
    }

    public void ReturnToPreviousItem()
    {
        if (previousIndex >= 0 && previousIndex != currentIndex)
        {
            EventHandler.CallChangeItemEvent(previousIndex);
            currentIndex = previousIndex;
        }
    }

    public void SetEmpty()
    {
        this.gameObject.SetActive(false);
    }
}