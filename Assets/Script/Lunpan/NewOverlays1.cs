using UnityEngine;
using System.Collections;

public class NewOverlays1 : MonoBehaviour
{
    [Header("覆盖图片设置")]
    public Sprite[] overlaySprites;
    public Vector2 overlaySize = Vector2.one;
    [Tooltip("覆盖图片的位置偏移 (X: 水平, Y: 垂直)")]
    public Vector2 overlayOffset = Vector2.zero;
    [Tooltip("覆盖图片显示时间(秒)")]
    public float displayTime = 2f; // 新增：图片显示时间
    [Tooltip("是否启用淡出效果")]
    public bool useFadeOut = true; // 新增：是否使用淡出
    [Tooltip("淡出持续时间(秒)")]
    public float fadeDuration = 0.5f; // 新增：淡出持续时间

    [Header("调试")]
    [Tooltip("是否在场景视图中显示偏移量指引")]
    public bool showOffsetGizmo = true;
    [Tooltip("指引线颜色")]
    public Color gizmoColor = Color.green;

    private int currentIndex = 0;
    private GameObject currentOverlay;
    private Coroutine fadeCoroutine;

    void OnMouseDown()
    {
        if (overlaySprites.Length == 0) return;

        // 移除现有覆盖和可能正在运行的协程
        RemoveCurrentOverlay();

        // 创建新覆盖
        CreateNewOverlay();

        // 启动自动消失计时
        StartAutoHideTimer();
    }

    private void RemoveCurrentOverlay()
    {
        if (currentOverlay != null)
        {
            // 停止可能正在运行的淡出协程
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            Destroy(currentOverlay);
        }
    }

    private void CreateNewOverlay()
    {
        currentOverlay = new GameObject("OverlayImage");
        currentOverlay.transform.position = transform.position + (Vector3)overlayOffset;
        currentOverlay.transform.SetParent(transform);

        SpriteRenderer renderer = currentOverlay.AddComponent<SpriteRenderer>();
        renderer.sprite = overlaySprites[currentIndex];
        renderer.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;

        currentOverlay.transform.localScale = new Vector3(
            overlaySize.x,
            overlaySize.y,
            1f
        );

        // 更新索引
        currentIndex = (currentIndex + 1) % overlaySprites.Length;
    }

    private void StartAutoHideTimer()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(AutoHideRoutine());
    }

    private IEnumerator AutoHideRoutine()
    {
        // 等待显示时间
        yield return new WaitForSeconds(displayTime);

        // 淡出或直接消失
        if (useFadeOut && currentOverlay != null)
        {
            SpriteRenderer renderer = currentOverlay.GetComponent<SpriteRenderer>();
            float elapsedTime = 0f;
            Color originalColor = renderer.color;

            while (elapsedTime < fadeDuration && currentOverlay != null)
            {
                elapsedTime += Time.deltaTime;
                float newAlpha = Mathf.Lerp(originalColor.a, 0f, elapsedTime / fadeDuration);

                Color newColor = originalColor;
                newColor.a = newAlpha;
                renderer.color = newColor;

                yield return null;
            }
        }

        // 销毁覆盖对象
        if (currentOverlay != null)
        {
            Destroy(currentOverlay);
            currentOverlay = null;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!showOffsetGizmo) return;

        Gizmos.color = gizmoColor;
        Vector3 offsetPosition = transform.position + (Vector3)overlayOffset;
        Gizmos.DrawWireSphere(offsetPosition, 0.1f);
        Gizmos.DrawLine(transform.position, offsetPosition);
    }

    private void OnValidate()
    {
        if (currentOverlay != null)
        {
            currentOverlay.transform.position = transform.position + (Vector3)overlayOffset;
        }
    }
#endif
}