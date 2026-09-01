using UnityEngine;
using UnityEngine.SceneManagement;

public class DingSceneController : MonoBehaviour
{
    [Header("场景跳转设置")]
    [Tooltip("将需要跳转的场景拖拽到这里")]
    public Object targetScene; // 通过UnityEngine.Object引用场景

    [Space]
    [Tooltip("点击间隔时间防止连点")]
    public float clickCooldown = 0.5f;

    private string targetSceneName; // 实际使用的场景名称
    private float lastClickTime;

    //void Start2()
    //{
    //    // 通过场景资源路径获取场景名称
    //    if (targetScene != null)
    //    {
    //        string scenePath = UnityEditor.AssetDatabase.GetAssetPath(targetScene);
    //        targetSceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
    //    }
    //}

    void OnMouseDown()
    {
        if (Time.time - lastClickTime < clickCooldown) return;

        lastClickTime = Time.time;

        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogError("目标场景设置错误！", this);
        }
    }
}