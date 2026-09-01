using UnityEngine;
using System.Collections;

public class ImageOverlayWithSize : MonoBehaviour
{
    [Header("覆盖图片设置")]
    public Sprite[] overlaySprites;
    public Vector2 overlaySize = Vector2.one;
    [Tooltip("覆盖图片的位置偏移 (X: 水平, Y: 垂直)")]
    public Vector2 overlayOffset = Vector2.zero;
    [Tooltip("图片显示后自动消失的延迟时间(秒)")]
    public float autoDisappearDelay = 2f;

    [Header("调试")]
    [Tooltip("是否在场景视图中显示偏移量指引")]
    public bool showOffsetGizmo = true;
    [Tooltip("指引线颜色")]
    public Color gizmoColor = Color.green;

    private int currentIndex = 0;
    private GameObject currentOverlay;
    private static bool hasCompletedCycle = false; // 改为静态变量
    private Coroutine disappearCoroutine;

    // 定义完成事件
    public delegate void OverlayCycleCompletedHandler();
    public event OverlayCycleCompletedHandler OnCycleCompleted;

    void OnMouseDown()
    {
        if (hasCompletedCycle) return; // 如果已完成循环，直接返回

        ShowNextOverlay();
    }

    private void ShowNextOverlay()
    {
        if (overlaySprites.Length == 0) return;

        // 如果已经有协程在运行，先停止它
        if (disappearCoroutine != null)
        {
            StopCoroutine(disappearCoroutine);
            disappearCoroutine = null;
        }

        // 移除当前覆盖图片
        if (currentOverlay != null)
        {
            Destroy(currentOverlay);
        }

        // 创建新覆盖图片
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

        currentIndex++;
        if (currentIndex >= overlaySprites.Length)
        {
            currentIndex = 0;
            hasCompletedCycle = true; // 标记循环已完成

            // 触发完成事件
            OnCycleCompleted?.Invoke();
        }

        // 启动自动消失协程
        disappearCoroutine = StartCoroutine(AutoDisappearAfterDelay());
    }

    private IEnumerator AutoDisappearAfterDelay()
    {
        yield return new WaitForSeconds(autoDisappearDelay);

        if (currentOverlay != null)
        {
            Destroy(currentOverlay);
            currentOverlay = null;
        }
    }

    // 新增：重置静态状态的方法
    public static void ResetGlobalCycleState()
    {
        hasCompletedCycle = false;
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