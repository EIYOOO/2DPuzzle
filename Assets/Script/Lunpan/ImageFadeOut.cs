using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AdvancedImageFade : MonoBehaviour
{
    [Header("图片设置")]
    public Sprite newSprite;          // 第二秒要切换的新图片
    public float firstPhaseDuration = 2f; // 原图片显示时间(秒)
    public float secondPhaseDuration = 2f; // 新图片显示时间(秒)
    public float fadeDuration = 0.5f; // 淡出持续时间(秒)

    private SpriteRenderer spriteRenderer;
    private Image uiImage;
    private Sprite originalSprite;
    private Color originalColor;

    void Start()
    {
        // 获取渲染组件并保存原始状态
        spriteRenderer = GetComponent<SpriteRenderer>();
        uiImage = GetComponent<Image>();

        if (spriteRenderer != null)
        {
            originalSprite = spriteRenderer.sprite;
            originalColor = spriteRenderer.color;
        }
        else if (uiImage != null)
        {
            originalSprite = uiImage.sprite;
            originalColor = uiImage.color;
        }

        // 启动图片切换协程
        StartCoroutine(ImageTransitionRoutine());
    }

    IEnumerator ImageTransitionRoutine()
    {
        // 第一阶段：显示原图片
        yield return new WaitForSeconds(firstPhaseDuration);

        // 第二阶段：立即切换为新图片
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = newSprite;
        }
        else if (uiImage != null)
        {
            uiImage.sprite = newSprite;
        }

        // 第三阶段：新图片显示一段时间后淡出
        yield return new WaitForSeconds(secondPhaseDuration);

        // 淡出效果
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(originalColor.a, 0f, elapsedTime / fadeDuration);

            if (spriteRenderer != null)
            {
                Color newColor = spriteRenderer.color;
                newColor.a = newAlpha;
                spriteRenderer.color = newColor;
            }
            else if (uiImage != null)
            {
                Color newColor = uiImage.color;
                newColor.a = newAlpha;
                uiImage.color = newColor;
            }

            yield return null;
        }

        // 淡出完成后禁用对象
        gameObject.SetActive(false);
    }

    // 编辑器调试信息
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        string info = $"图片切换时序:\n" +
                     $"0-{firstPhaseDuration}s: 原图\n" +
                     $"{firstPhaseDuration}-{firstPhaseDuration + secondPhaseDuration}s: 新图\n" +
                     $"之后淡出{fadeDuration}s";
        UnityEditor.Handles.Label(transform.position, info, style);
    }
#endif
}