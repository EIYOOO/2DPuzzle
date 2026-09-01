using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoSceneChange : MonoBehaviour
{
    public VideoPlayer videoPlayer; // 拖拽你的VideoPlayer组件到这里
    public string sceneName = "Scene7"; // 要跳转的场景名称

    void Start()
    {
        // 确保VideoPlayer不为空
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        // 添加视频播放结束事件监听
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // 视频播放结束时加载新场景
        SceneManager.LoadScene(sceneName);
    }

    void OnDestroy()
    {
        // 移除事件监听以防止内存泄漏
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
    }
}
