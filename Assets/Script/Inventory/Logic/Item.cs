using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Timeline.Actions;
using UnityEngine;

public class Item : MonoBehaviour
{
   public ItemName itemName;
   public bool canClickCollection;
   
   [SerializeField] private GameObject holeSetActive;

   public void ItemClicked()
   {
      InventoryManager.Instance.AddItem(itemName);
      
      this.gameObject.SetActive(false);
   }
}
