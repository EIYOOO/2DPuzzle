using UnityEngine;

public class GameObjectManager : MonoBehaviour
{
    // 单例实例
    public static GameObjectManager Instance;

    // 机关状态属性
    public bool IsPuzzle1Completed { get; private set; }

    // 事件系统
    public event System.Action OnPuzzle1Completed;

    // 持久化存储键
    private const string Puzzle1CompleteKey = "Puzzle1Complete";

    void Awake()
    {
        // 单例模式初始化
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 跨场景保留
        }
        else
        {
            Destroy(gameObject); // 防止重复实例
        }
    }

    void Start()
    {
        // 加载存档状态
        LoadPuzzleStates();
    }

    /// <summary>
    /// 标记机关1完成
    /// </summary>
    public void CompletePuzzle1()
    {
        if (!IsPuzzle1Completed)
        {
            IsPuzzle1Completed = true;
            PlayerPrefs.SetInt(Puzzle1CompleteKey, 1);
            PlayerPrefs.Save();
            GameObjectManager.Instance.IsPuzzle1Completed = true;
            // 触发事件
            OnPuzzle1Completed?.Invoke();
            Debug.Log("机关1已完成！");
        }
    }

    /// <summary>
    /// 加载所有机关状态
    /// </summary>
    private void LoadPuzzleStates()
    {
        IsPuzzle1Completed = PlayerPrefs.GetInt(Puzzle1CompleteKey, 0) == 1;
    }

    /// <summary>
    /// 重置所有进度（调试用）
    /// </summary>
    public void ResetAllProgress()
    {
        IsPuzzle1Completed = false;
        PlayerPrefs.DeleteKey(Puzzle1CompleteKey);
        PlayerPrefs.Save();

        // 触发事件更新场景对象
        OnPuzzle1Completed?.Invoke();
        Debug.Log("所有进度已重置");
    }

    /// <summary>
    /// 安全销毁时解除事件绑定
    /// </summary>
    void OnDestroy()
    {
        if (Instance == this)
        {
            // 清理所有事件监听者
            OnPuzzle1Completed = null;
        }
    }
}