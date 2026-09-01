using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public int CurrentIndex { get; private set; } = 0;
    public itemDataList_SO itemData;
    [SerializeField] public List<ItemName> itemList = new List<ItemName>();

    public List<ItemName> GetItemList()
    {
        return itemList;
    }

    private void OnEnable()
    {
        EventHandler.ItemUsedEvent += OnItemUsedEvent;
        EventHandler.ChangeItemEvent += OnChangeItemEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {
        EventHandler.ItemUsedEvent -= OnItemUsedEvent;
        EventHandler.ChangeItemEvent -= OnChangeItemEvent;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
    }

    private void OnItemUsedEvent(ItemName itemName)
    {
        var index = GetItemIndex(itemName);
        if (index >= 0)
        {
            itemList.RemoveAt(index); 

            if (itemList.Count == 0)
            {
                EventHandler.CallUpdateUIEvent(null, -1); 
            }
            else
            {
                int newIndex = Mathf.Min(index, itemList.Count - 1); 
                ItemDetails newItem = itemData.GetItemDetails(itemList[newIndex]);

                EventHandler.CallUpdateUIEvent(newItem, newIndex);

                CurrentIndex = newIndex; 
            }
        }
    }

    private void OnChangeItemEvent(int index)
    {
        if (index >= 0 && index < itemList.Count)
        {
            CurrentIndex = index;
            ItemDetails item = itemData.GetItemDetails(itemList[index]);
            EventHandler.CallUpdateUIEvent(item, index);
        }
    }

    public ItemDetails GetCurrentItem()
    {
        if (itemList.Count == 0 || CurrentIndex < 0 || CurrentIndex >= itemList.Count)
        {
            return null; 
        }

        return itemData.GetItemDetails(itemList[CurrentIndex]);
    }

    private void OnAfterSceneLoadedEvent()
    {
        int savedIndex = PlayerPrefs.GetInt("CurrentItemIndex", -1);
        if (savedIndex >= 0 && savedIndex < itemList.Count)
        {
            EventHandler.CallChangeItemEvent(savedIndex); 
        }
        else
        {
            EventHandler.CallChangeItemEvent(0); 
        }
    }

    public void AddItem(ItemName itemName)
    {
        if (!itemList.Contains(itemName))
        {
            itemList.Add(itemName);
            EventHandler.CallUpdateUIEvent(itemData.GetItemDetails(itemName), itemList.Count - 1);

            EventHandler.CallItemAddedEvent(itemName);
        }
    }

    private int GetItemIndex(ItemName itemName)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i] == itemName)
            {
                return i;
            }
        }
        return -1;
    }
}