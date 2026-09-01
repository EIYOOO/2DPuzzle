using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : Singleton<TransitionManager>
{
    public string startScene;
    
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration;
    private bool isFade;

    private void Start()
    {
        StartCoroutine(TransitionToScene(string.Empty, startScene));
        
        PlayerPrefs.DeleteKey("MudGearCleanStage");
        Debug.Log("已清洁MudGearCleanStage");
        PlayerPrefs.DeleteKey("CrackCleanStage");
        Debug.Log("已清洁CrackCleanStage");

        PlayerPrefs.DeleteKey("HoleState");
        Debug.Log("洞状态已重置！");

        PlayerPrefs.Save();
        Debug.Log("新游戏已开始，清洁状态已重置！");
    }

    public void Transition(string from, string to)
    {
        if (!isFade)
        {
            StartCoroutine(TransitionToScene(from, to));
        }
    }

    private IEnumerator TransitionToScene(string from, string to)
    {
        yield return Fade(1);

        if (from != string.Empty)
        {
            EventHandler.CallBeforeSceneUnloadEvent();
            yield return SceneManager.UnloadSceneAsync(from);
        }

        yield return SceneManager.LoadSceneAsync(to, LoadSceneMode.Additive);

        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newScene);

        LoadHoleStateInNewScene();

        EventHandler.CallAfterSceneLoadedEvent();

        yield return Fade(0);
    }

    private void LoadHoleStateInNewScene()
    {
        CrackRender crackRender = FindObjectOfType<CrackRender>();
        if (crackRender != null)
        {
            crackRender.LoadHoleState();
            Debug.Log("洞状态已加载！");
        }
        else
        {
            Debug.LogWarning("未找到 CrackRender 组件，洞状态未加载！");
        }
    }
    
    private IEnumerator Fade(float targetAlpha)
    {
        isFade = true;
        fadeCanvasGroup.blocksRaycasts = true;
        
        float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / fadeDuration;

        while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
        {
            fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }
        
        fadeCanvasGroup.blocksRaycasts = false;
        isFade = false;
    }
}