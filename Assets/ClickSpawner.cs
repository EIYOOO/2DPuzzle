using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(SpriteRenderer))]
public class ClickStoryController : MonoBehaviour
{
    [Header("Story Settings")]
    public Sprite[] storySprites;
    public Sprite finalSprite;  // 最后点击结束的图片

    [Header("Video Settings")]
    public VideoPlayer videoPlayer;
    public GameObject videoScreen;

    private SpriteRenderer spriteRenderer;
    private int currentIndex = 0;
    private bool isFinalStage = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        InitializeVideoSystem();
    }

    void InitializeVideoSystem()
    {
        if (videoPlayer != null)
        {
            videoPlayer.gameObject.SetActive(false);
   
        }

        if (videoScreen != null)
            videoScreen.SetActive(false);
    }

    void OnMouseDown()
    {
        if (isFinalStage)
        {
            QuitApplication();
            return;
        }

        if (currentIndex < storySprites.Length - 1)
        {
            // 普通图片切换
            currentIndex++;
            spriteRenderer.sprite = storySprites[currentIndex];
        }
        else if (currentIndex == storySprites.Length - 1)
        {
            // 触发视频播放
            StartVideo();
            currentIndex++; // 进入视频阶段
        }
    }

    void StartVideo()
    {
        if (videoPlayer == null || videoScreen == null) return;

        gameObject.SetActive(false);
        videoScreen.SetActive(true);
        videoPlayer.gameObject.SetActive(true);
        videoPlayer.Play();
    }

  

    void QuitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    
}