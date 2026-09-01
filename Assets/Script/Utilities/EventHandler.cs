using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventHandler
{
    public static event Action<ItemDetails, int> UpdateUIEvent;
    public static void CallUpdateUIEvent(ItemDetails itemDetails, int index)
    {
        UpdateUIEvent?.Invoke(itemDetails, index);
    }
    
    
    public static event Action BeforeSceneUnloadEvent;
    public static void CallBeforeSceneUnloadEvent()
    {
        BeforeSceneUnloadEvent?.Invoke();
    }
   
    
    public static event Action AfterSceneLoadedEvent;
    public static void CallAfterSceneLoadedEvent()
    {
        AfterSceneLoadedEvent?.Invoke();
    }
    
    
    public static event Action<ItemDetails,bool> ItemSelectedEvent;
    public static void CallItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        ItemSelectedEvent?.Invoke(itemDetails, isSelected);
    }
    
    
    public static event Action<ItemName> ItemUsedEvent;
    public static void CallItemUsedEvent(ItemName itemName)
    {
        ItemUsedEvent?.Invoke(itemName);
    }
    
    
    public static event Action<int> ChangeItemEvent;
    public static void CallChangeItemEvent(int index)
    {
        ChangeItemEvent?.Invoke(index);
    }
    
    public static event Action<ItemName> ItemAddedEvent;

    public static void CallItemAddedEvent(ItemName itemName)
    {
        ItemAddedEvent?.Invoke(itemName);
    }
    
}
