// 弹窗物体上的脚本（如 ClickPopup.cs）
using UnityEngine;

public class ClickPopup : MonoBehaviour
{
    public GameObject popupImage; // 弹窗预制体或物体

    void OnMouseDown()
    {
        // 通过管理器打开弹窗
        PopupManager.Instance.OpenPopup(popupImage);
    }
}