using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections; // 添加这个命名空间引用以使用IEnumerator

public class EndingTransition : MonoBehaviour
{
    [Header("视频设置")]
    public VideoPlayer[] videoPlayers; // 场景中的所有视频播放器
    public string nextSceneName = "Scene8"; // 要跳转的下一个场景名

    [Header("过渡设置")]
    public float fadeDuration = 1.0f; // 淡入淡出持续时间
    public CanvasGroup fadeCanvasGroup; // 用于淡入淡出的CanvasGroup

    private bool allVideosFinished = false;
    private bool isTransitioning = false;

    private void Start()
    {
        // 初始化淡入淡出效果
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0;
            fadeCanvasGroup.gameObject.SetActive(false);
        }

        // 设置所有视频播放器的完成事件
        foreach (var videoPlayer in videoPlayers)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
        }

        // 如果没有视频，直接过渡
        if (videoPlayers.Length == 0)
        {
            StartCoroutine(TransitionToNextScene());
        }
    }

    private void OnVideoFinished(VideoPlayer source)
    {
        // 检查是否所有视频都播放完成
        foreach (var videoPlayer in videoPlayers)
        {
            if (videoPlayer.isPlaying)
            {
                return; // 还有视频在播放
            }
        }

        // 所有视频播放完成
        if (!allVideosFinished && !isTransitioning)
        {
            allVideosFinished = true;
            StartCoroutine(TransitionToNextScene());
        }
    }

    private IEnumerator TransitionToNextScene()
    {
        isTransitioning = true;

        // 淡出效果
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.gameObject.SetActive(true);
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                fadeCanvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            fadeCanvasGroup.alpha = 1;
        }

        // 异步加载下一个场景
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
        asyncLoad.allowSceneActivation = false;

        // 等待场景加载
        while (!asyncLoad.isDone)
        {
            // 当加载进度达到90%时，允许场景切换
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    // 编辑器调试
    private void OnValidate()
    {
        // 自动获取所有视频播放器（可选）
        if (videoPlayers == null || videoPlayers.Length == 0)
        {
            videoPlayers = FindObjectsOfType<VideoPlayer>();
        }
    }
}