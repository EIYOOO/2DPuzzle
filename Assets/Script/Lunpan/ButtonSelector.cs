using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

[System.Serializable]
public class ButtonData
{
    public Button button;
    public bool isCorrectButton;
    public Sprite defaultSprite;
    public Sprite selectedSprite;
    [HideInInspector] public bool isSelected;
}

public class ButtonSelector : MonoBehaviour
{
    // 按钮设置
    public ButtonData[] buttons;
    public int requiredCorrectSelections = 5;

    // 反馈效果
    public AudioClip selectSound;
    public AudioClip correctSound;
    public AudioClip wrongSound;
    public AudioClip successSound;

    // 过渡效果设置
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 0.5f;
    public float successDelay = 1.0f; // 成功音效播放后的延迟
    public GameObject loadingScreen;
    public Image progressBar;

    private AudioSource audioSource;
    private bool isTransitioning = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        InitializeTransitionSystem();
        InitializeButtons();
    }

    private void InitializeTransitionSystem()
    {
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0;
            fadeCanvasGroup.gameObject.SetActive(false);
        }

        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }

    private void InitializeButtons()
    {
        foreach (ButtonData button in buttons)
        {
            button.button.onClick.AddListener(() => ToggleButton(button));
            button.button.image.sprite = button.defaultSprite;
            button.isSelected = false;
        }
    }

    private void ToggleButton(ButtonData buttonData)
    {
        if (isTransitioning) return;

        buttonData.isSelected = !buttonData.isSelected;
        buttonData.button.image.sprite = buttonData.isSelected ?
            buttonData.selectedSprite :
            buttonData.defaultSprite;

        PlayFeedbackSound(buttonData);
        CheckSelectionCondition();
    }

    private void PlayFeedbackSound(ButtonData buttonData)
    {
        if (buttonData.isSelected)
        {
            if (buttonData.isCorrectButton && correctSound != null)
            {
                audioSource.PlayOneShot(correctSound);
            }
            else if (!buttonData.isCorrectButton && wrongSound != null)
            {
                audioSource.PlayOneShot(wrongSound);
            }
        }
        else if (selectSound != null)
        {
            audioSource.PlayOneShot(selectSound);
        }
    }

    private void CheckSelectionCondition()
    {
        int correctSelected = 0;
        bool hasWrongSelection = false;

        foreach (ButtonData button in buttons)
        {
            if (button.isSelected)
            {
                if (button.isCorrectButton)
                {
                    correctSelected++;
                }
                else
                {
                    hasWrongSelection = true;
                }
            }
        }

        if (correctSelected == requiredCorrectSelections && !hasWrongSelection)
        {
            StartCoroutine(CompleteSelectionAndTransition());
        }
    }

    private IEnumerator CompleteSelectionAndTransition()
    {
        isTransitioning = true;

        // 播放成功音效
        if (successSound != null)
        {
            audioSource.PlayOneShot(successSound);
        }

        // 等待音效播放完成
        yield return new WaitForSeconds(successDelay);

        // 开始淡出效果
        yield return StartCoroutine(FadeOut());

        // 显示加载画面
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        // 强制跳转到Scene6 - 使用异步加载
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scene6");
        asyncLoad.allowSceneActivation = false;

        // 更新进度条
        while (!asyncLoad.isDone)
        {
            if (progressBar != null)
            {
                progressBar.fillAmount = asyncLoad.progress / 0.9f;
            }

            if (asyncLoad.progress >= 0.9f)
            {
                // 短暂等待确保加载完成
                yield return new WaitForSeconds(0.1f);

                // 执行淡入效果
                yield return StartCoroutine(FadeIn());

                // 激活场景
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
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
    }

    private IEnumerator FadeIn()
    {
        if (fadeCanvasGroup != null)
        {
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                fadeCanvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            fadeCanvasGroup.alpha = 0;
            fadeCanvasGroup.gameObject.SetActive(false);
        }
    }

    public void ResetAllButtons()
    {
        foreach (ButtonData button in buttons)
        {
            button.isSelected = false;
            button.button.image.sprite = button.defaultSprite;
        }
    }
}