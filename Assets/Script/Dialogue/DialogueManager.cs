using UnityEngine;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public GameObject[] dialogueImages;
    public Animator sceneAnimator;
    private int currentIndex = 0;

    void Start()
    {
        // 初始隐藏所有图片
        foreach (var img in dialogueImages)
        {
            img.SetActive(false);
        }

        // 启动延迟显示协程
        StartCoroutine(ShowFirstImageAfterDelay(1f));
    }

    // 新增协程方法
    IEnumerator ShowFirstImageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 等待1秒

        // 显示第一张图
        if (dialogueImages.Length > 0)
        {
            dialogueImages[0].SetActive(true);
        }
    }

    public void GoToNextImage()
    {
        // 隐藏当前图片
        dialogueImages[currentIndex].SetActive(false);
        currentIndex++;

        if (currentIndex < dialogueImages.Length)
        {
            // 显示下一张
            dialogueImages[currentIndex].SetActive(true);
        }
        else
        {
            // 所有图片播放完毕，触发动画
            sceneAnimator.SetTrigger("Start");
        }
    }
}