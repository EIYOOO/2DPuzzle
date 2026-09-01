using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private Dictionary<ItemName,bool> itemAvailableDic = new Dictionary<ItemName, bool>();
    private void OnEnable()
    {
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
        EventHandler.UpdateUIEvent += OnUpdateUIEvent;
    }
    private void OnDisable()
    {
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
        EventHandler.UpdateUIEvent -= OnUpdateUIEvent;
    }

    private void OnBeforeSceneUnloadEvent()
    {
        foreach (var item in FindObjectsOfType<Item>())
        {
            if (!itemAvailableDic.ContainsKey(item.itemName))
            {
                itemAvailableDic.Add(item.itemName, true);
            }
        }
    }
    private void OnAfterSceneLoadedEvent()
    {
        foreach (var item in FindObjectsOfType<Item>())
        {
            if (!itemAvailableDic.ContainsKey(item.itemName))
            {
                itemAvailableDic.Add(item.itemName, true);
            }
            else
            {
                item.gameObject.SetActive(itemAvailableDic[item.itemName]);
            }
        }
    }
    private void OnUpdateUIEvent(ItemDetails itemDetails,int index)
    {
        if (itemDetails != null)
        {
            itemAvailableDic[itemDetails.itemName] = false;
        }
    }
}