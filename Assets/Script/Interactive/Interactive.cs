using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    public ItemName requireItem;
    public bool isDone;

    public void CheckItem(ItemName itemName)
    {
        if (itemName == requireItem && !isDone)
        {
            isDone = true;
            //使用、移除物品
            OnclickedAction();
            EventHandler.CallItemUsedEvent(itemName);
        }
    }
        
    /// <summary>
    /// 默认正确物品执行
    /// </summary>
    protected virtual void OnclickedAction()
    {
        //相同逻辑
    }

    public virtual void EmptyClicked()
    {
        Debug.Log("空点");
    }
}
