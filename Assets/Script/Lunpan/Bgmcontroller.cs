using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Bgmcontroller : MonoBehaviour
{
    public static Bgmcontroller Instance { get; private set; }

    [Header("背景音乐设置")]
    public AudioClip bgm1; // Scene6之前的背景音乐
    public AudioClip bgm2; // Scene6之后的背景音乐
    [Range(0f, 1f)] public float bgm1Volume = 0.2f; // BGM1音量
    [Range(0f, 1f)] public float bgm2Volume = 0.7f; // BGM2音量
    public float crossFadeDuration = 1.5f;

    private AudioSource primarySource;
    private AudioSource secondarySource;
    private bool usingPrimarySource = true;
    private bool hasReachedScene6 = false;

    private void Awake()
    {
        // 单例模式实现
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void InitializeAudioSources()
    {
        // 创建两个音频源实现无缝交叉淡入淡出
        primarySource = gameObject.AddComponent<AudioSource>();
        secondarySource = gameObject.AddComponent<AudioSource>();

        foreach (var source in new[] { primarySource, secondarySource })
        {
            source.loop = true;
            source.playOnAwake = false;
            source.volume = 0f;
        }

        // 初始播放bgm1
        primarySource.clip = bgm1;
        primarySource.Play();
        StartCoroutine(FadeIn(primarySource, bgm1Volume));
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        CheckCurrentScene(SceneManager.GetActiveScene());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckCurrentScene(scene);
    }

    private void CheckCurrentScene(Scene scene)
    {
        // 检查是否到达Scene6
        if (!hasReachedScene6 && scene.name == "Scene6")
        {
            hasReachedScene6 = true;
            SwitchToBGM2();
        }
    }

    private void SwitchToBGM2()
    {
        if (bgm2 == null) return;

        // 确定当前使用的源和备用源
        AudioSource activeSource = usingPrimarySource ? primarySource : secondarySource;
        AudioSource newSource = usingPrimarySource ? secondarySource : primarySource;

        // 设置新音乐并播放
        newSource.clip = bgm2;
        newSource.Play();

        // 交叉淡入淡出
        StartCoroutine(CrossFade(activeSource, newSource, bgm1Volume, bgm2Volume));

        // 切换使用标志
        usingPrimarySource = !usingPrimarySource;
    }

    private IEnumerator CrossFade(AudioSource fadeOutSource, AudioSource fadeInSource, float startVolumeOut, float startVolumeIn)
    {
        float elapsedTime = 0f;

        while (elapsedTime < crossFadeDuration)
        {
            float t = elapsedTime / crossFadeDuration;
            fadeOutSource.volume = Mathf.Lerp(startVolumeOut, 0f, t);
            fadeInSource.volume = Mathf.Lerp(0f, startVolumeIn, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeOutSource.volume = 0f;
        fadeInSource.volume = startVolumeIn;
        fadeOutSource.Stop();
    }

    private IEnumerator FadeIn(AudioSource source, float targetVolume)
    {
        float elapsedTime = 0f;

        while (elapsedTime < crossFadeDuration)
        {
            source.volume = Mathf.Lerp(0f, targetVolume, elapsedTime / crossFadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        source.volume = targetVolume;
    }

    // 设置BGM1音量
    public void SetBGM1Volume(float newVolume)
    {
        bgm1Volume = Mathf.Clamp01(newVolume);
        if (usingPrimarySource && primarySource.clip == bgm1)
        {
            primarySource.volume = bgm1Volume;
        }
        else if (!usingPrimarySource && secondarySource.clip == bgm1)
        {
            secondarySource.volume = bgm1Volume;
        }
    }

    // 设置BGM2音量
    public void SetBGM2Volume(float newVolume)
    {
        bgm2Volume = Mathf.Clamp01(newVolume);
        if (usingPrimarySource && primarySource.clip == bgm2)
        {
            primarySource.volume = bgm2Volume;
        }
        else if (!usingPrimarySource && secondarySource.clip == bgm2)
        {
            secondarySource.volume = bgm2Volume;
        }
    }

    // 获取当前播放的BGM类型
    public bool IsPlayingBGM1()
    {
        return (usingPrimarySource && primarySource.clip == bgm1) ||
               (!usingPrimarySource && secondarySource.clip == bgm1);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}