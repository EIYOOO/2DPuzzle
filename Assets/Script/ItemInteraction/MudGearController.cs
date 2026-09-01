using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MudGearController : MonoBehaviour
{
    [System.Serializable]
    public class CleanStage
    {
        public ItemName requiredTool;    // 该阶段需要的工具类型
        public Sprite stageSprite;      // 该阶段显示的齿轮图片
        public AudioClip toolUseSound;  // 使用该工具时的特定音效
    }

    [Header("清洁阶段设置")]
    [SerializeField] private CleanStage[] cleanStages;

    [Header("组件引用")]
    [SerializeField] private SpriteRenderer gearRenderer;
    [SerializeField] private AudioSource audioSource; // 用于播放音效的组件

    private int currentStage = 0;
    private bool isUsed;

    private const string CleanStageKey = "MudGearCleanStage";

    private void Start()
    {
        // 确保有AudioSource组件
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        LoadCleanState();
    }

    private void LoadCleanState()
    {
        int savedStage = PlayerPrefs.GetInt(CleanStageKey, 0);
        currentStage = savedStage;
        UpdateGearVisual();
    }

    private void UpdateGearVisual()
    {
        gearRenderer.sprite = cleanStages[currentStage].stageSprite;
    }

    private void SaveCleanState()
    {
        PlayerPrefs.SetInt(CleanStageKey, currentStage);
        PlayerPrefs.Save();
    }

    private void OnMouseDown()
    {
        CleanStage currentStageData = cleanStages[currentStage];
        ItemDetails currentItem = InventoryManager.Instance.GetCurrentItem();

        if (currentItem != null && currentItem.itemName == currentStageData.requiredTool)
        {
            isUsed = true;

            // 播放该工具对应的特定音效
            if (currentStageData.toolUseSound != null)
            {
                audioSource.PlayOneShot(currentStageData.toolUseSound);
            }

            AdvanceStage();
            EventHandler.CallItemUsedEvent(currentItem.itemName);
        }
        else
        {
            Debug.Log($"当前阶段需要 {currentStageData.requiredTool} 工具");
            // 这里不再播放通用的错误音效
        }
    }

    private void AdvanceStage()
    {
        currentStage = Mathf.Clamp(currentStage + 1, 0, cleanStages.Length - 1);
        UpdateGearVisual();
        isUsed = false;

        SaveCleanState();

        if (currentStage == cleanStages.Length - 1)
        {
            Debug.Log("清洁完成！");
            StartCoroutine(LoadScene2AndUnloadScene4());
        }
    }

    private IEnumerator LoadScene2AndUnloadScene4()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync("Scene2", LoadSceneMode.Additive);

        while (!loadOperation.isDone)
        {
            yield return null;
        }

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Scene2"));
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(currentScene);

        while (!unloadOperation.isDone)
        {
            yield return null;
        }

        Debug.Log("场景切换完成！");
    }
}