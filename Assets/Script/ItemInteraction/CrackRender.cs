using UnityEngine;

public class CrackRender : MonoBehaviour
{
    [Header("状态图像")]
    [SerializeField] private Sprite damagedSprite;  
    [SerializeField] private Sprite initialStateSprite;
    [SerializeField] private ItemName requiredTool;    

    [Header("组件")]
    [SerializeField] private SpriteRenderer gearRenderer;  
    [SerializeField] private Collider2D itemCollider;      
    [SerializeField] private GameObject childObjectToActivate1;
    [SerializeField] private GameObject childObjectToActivate2; 
    [SerializeField] private GameObject holeSetActive;

    [Header("音效")]
    [SerializeField] private AudioClip repairSound;        // 修理/破坏音效
    [SerializeField] private float soundVolume = 1f;      // 音效音量

    private int currentStage = 0;  
    private const string CleanStageKey = "CrackCleanStage";
    private const string HoleStateKey = "HoleState";
    private AudioSource audioSource; // 音频源组件
    private void Start()
    {
        // 获取或添加AudioSource组件
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        LoadCleanState();
        LoadHoleState();
        UpdateVisualState();

        CheckAndUpdateChildObjects();

        EventHandler.ItemAddedEvent += OnItemAdded;
    }

    private void OnDestroy()
    {
        EventHandler.ItemAddedEvent -= OnItemAdded;
    }

    private void OnItemAdded(ItemName itemName)
    {
        CheckAndUpdateChildObjects();
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

    public void LoadHoleState()
    {
        bool isHoleActive = PlayerPrefs.GetInt(HoleStateKey, 0) == 1;

        if (holeSetActive != null)
        {
            holeSetActive.SetActive(isHoleActive);
        }
    }

    private void SaveHoleState(bool isActive)
    {
        PlayerPrefs.SetInt(HoleStateKey, isActive ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void UpdateVisualState()
    {
        gearRenderer.sprite = currentStage == 0 ? initialStateSprite : damagedSprite;
        itemCollider.enabled = currentStage == 0;

        childObjectToActivate1.SetActive(currentStage == 1);
        childObjectToActivate2.SetActive(currentStage == 1);
    }

    private void OnMouseDown()
    {
        if (currentStage == 1) return;

        var currentItem = GetCurrentSelectedItem();

        if (currentItem != null && currentItem.itemName == requiredTool)
        {
            currentStage = 1;
            UpdateVisualState();
            SaveCleanState();
            EventHandler.CallItemUsedEvent(currentItem.itemName);

            // 播放修理/破坏音效
            if (repairSound != null)
            {
                audioSource.PlayOneShot(repairSound, soundVolume);
            }

            CheckAndActivateHole();
            CheckAndUpdateChildObjects();
        }
    }

    private ItemDetails GetCurrentSelectedItem()
    {
        if (InventoryManager.Instance.GetItemList().Count == 0) return null;
        int currentIndex = InventoryManager.Instance.CurrentIndex;
        return InventoryManager.Instance.itemData.GetItemDetails(
            InventoryManager.Instance.GetItemList()[currentIndex]
        );
    }

    private void CheckAndActivateHole()
    {
        bool hasLemonAcid = InventoryManager.Instance.itemList.Contains(ItemName.柠檬酸);
        bool hasPurifiedWater = InventoryManager.Instance.itemList.Contains(ItemName.纯净水);

        if (hasLemonAcid && hasPurifiedWater)
        {
            if (holeSetActive != null)
            {
                holeSetActive.SetActive(true);
                SaveHoleState(true);
                Debug.Log("洞已激活！");
            }
        }
    }

    private void CheckAndUpdateChildObjects()
    {
        if (childObjectToActivate1 != null)
        {
            ItemName childItemName = GetItemNameFromGameObject(childObjectToActivate1);
            if (childItemName != ItemName.None && InventoryManager.Instance.itemList.Contains(childItemName))
            {
                childObjectToActivate1.SetActive(false);
            }
        }

        if (childObjectToActivate2 != null)
        {
            ItemName childItemName = GetItemNameFromGameObject(childObjectToActivate2);
            if (childItemName != ItemName.None && InventoryManager.Instance.itemList.Contains(childItemName))
            {
                childObjectToActivate2.SetActive(false);
            }
        }

        bool hasLemonAcid = InventoryManager.Instance.itemList.Contains(ItemName.柠檬酸);
        bool hasPurifiedWater = InventoryManager.Instance.itemList.Contains(ItemName.编钟锤);

        if (hasLemonAcid && hasPurifiedWater && holeSetActive != null)
        {
            holeSetActive.SetActive(true);
            SaveHoleState(true);
        }
    }

    private ItemName GetItemNameFromGameObject(GameObject obj)
    {
        Item itemComponent = obj.GetComponent<Item>();
        if (itemComponent != null)
        {
            return itemComponent.itemName;
        }
        return ItemName.None;
    }

    public void ResetHoleState()
    {
        if (holeSetActive != null)
        {
            holeSetActive.SetActive(false);
            SaveHoleState(false);
        }
    }
}