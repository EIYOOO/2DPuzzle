using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleRender : MonoBehaviour
{
    [Header("State Sprites")]
    [SerializeField] private Sprite damagedSprite;
    [SerializeField] private Sprite initialStateSprite;
    [SerializeField] private ItemName requiredTool;

    [Header("Components")]
    [SerializeField] private SpriteRenderer gearRenderer;
    [SerializeField] private Collider2D itemCollider;
    [SerializeField] private GameObject holeWithChimeHammer; // 包含编钟锤的洞口
    [SerializeField] private GameObject emptyHole;          // 空洞口

    private int currentStage = 0;
    private const string CleanStageKey = "CrackCleanStage";
    private const string HammerCollectedKey = "HammerCollected";

    private void Start()
    {
        LoadCleanState();
        UpdateVisualState();
        UpdateHoleState();
    }

    private void LoadCleanState()
    {
        currentStage = PlayerPrefs.GetInt(CleanStageKey, 0);
    }

    private void SaveCleanState()
    {
        PlayerPrefs.SetInt(CleanStageKey, currentStage);
        PlayerPrefs.Save();
    }

    private void UpdateVisualState()
    {
        gearRenderer.sprite = currentStage == 0 ? initialStateSprite : damagedSprite;
        itemCollider.enabled = currentStage == 0;
    }

    private void UpdateHoleState()
    {
        bool hammerCollected = PlayerPrefs.GetInt(HammerCollectedKey, 0) == 1;

        holeWithChimeHammer.SetActive(currentStage == 1 && !hammerCollected);
        emptyHole.SetActive(currentStage == 1 && hammerCollected);
    }

    private void OnMouseDown()
    {
        // 如果已经是破损状态或正在显示洞口时点击
        if (currentStage == 1) return;

        var currentItem = GetCurrentSelectedItem();

        if (currentItem != null && currentItem.itemName == requiredTool)
        {
            currentStage = 1;
            UpdateVisualState();
            UpdateHoleState();
            SaveCleanState();
            EventHandler.CallItemUsedEvent(currentItem.itemName);
        }
    }

    // 洞口编钟锤的点击事件（需要给洞口对象添加Collider2D）
    public void OnHoleClick()
    {
        if (!holeWithChimeHammer.activeSelf) return;

        // 添加编钟锤到背包
        //                           EventHandler.CallItemAddedEvent(ItemName.编钟锤);

        // 更新洞口状态
        PlayerPrefs.SetInt(HammerCollectedKey, 1);
        PlayerPrefs.Save();

        UpdateHoleState();
    }

    private ItemDetails GetCurrentSelectedItem()
    {
        if (InventoryManager.Instance.GetItemList().Count == 0) return null;
        int currentIndex = InventoryManager.Instance.CurrentIndex;
        return InventoryManager.Instance.itemData.GetItemDetails(
            InventoryManager.Instance.GetItemList()[currentIndex]
        );
    }

    // 重置状态（如果需要）
    public void ResetHoleState()
    {
        PlayerPrefs.DeleteKey(HammerCollectedKey);
        UpdateHoleState();
    }
}
