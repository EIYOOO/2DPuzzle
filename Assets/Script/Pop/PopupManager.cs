using UnityEngine;

public class PopupManager : MonoBehaviour
{
    // 单例模式（确保全局唯一）
    public static PopupManager Instance;

    private GameObject currentPopup; // 当前打开的弹窗

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 打开弹窗（自动关闭旧弹窗）
    public void OpenPopup(GameObject popup)
    {
        // 如果已有弹窗，先关闭
        if (currentPopup != null)
        {
            CloseCurrentPopup();
        }

        // 打开新弹窗
        currentPopup = popup.gameObject;
        currentPopup.SetActive(true);
    }

    // 关闭当前弹窗
    public void CloseCurrentPopup()
    {
        if (currentPopup != null)
        {
            currentPopup.SetActive(false);
            currentPopup = null;
        }
    }
}