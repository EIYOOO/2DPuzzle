using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("按钮设置")]
    public Button restartButton;
    public Button quitButton;
    public string restartSceneName = "Begin"; // 重新开始时要加载的场景名

    [Header("重置设置")]
    public bool resetPlayerPrefs = true; // 是否重置PlayerPrefs
    public bool resetStaticVariables = true; // 是否重置静态变量

    private void Start()
    {
        // 确保按钮不为空
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
    }

    // 重新开始游戏
    public void RestartGame()
    {
        // 重置静态变量状态（如果有）
        if (resetStaticVariables)
        {
            ResetAllStaticVariables();
        }

        // 清除玩家偏好设置（如果需要）
        if (resetPlayerPrefs)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        // 加载开始场景
        SceneManager.LoadScene(restartSceneName);

        // 如果需要淡入淡出效果，可以使用：
        // TransitionManager.Instance.Transition(SceneManager.GetActiveScene().name, restartSceneName);
    }

    // 退出游戏
    public void QuitGame()
    {
        // 在编辑器中停止播放
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 在发布版本中退出应用
        Application.Quit();
#endif
    }

    // 重置所有相关的静态变量
    private void ResetAllStaticVariables()
    {
        // 重置覆盖图片状态
        if (FindObjectOfType<ImageOverlayWithSize>() != null)
        {
            ImageOverlayWithSize.ResetGlobalCycleState();
        }

        // 可以在这里添加其他需要重置的静态变量
        // Example: OtherSystem.ResetStaticState();
    }
}