using UnityEngine;
using UnityEngine.UI;

public class ImageOverlayController : MonoBehaviour
{
    [Header("覆盖图片设置")]
    public Sprite[] overlaySprites;  // 所有可替换的覆盖图片
    public Canvas overlayCanvas;     // 覆盖层Canvas

    [Header("尺寸设置")]
    public Vector2 overlaySize = new Vector2(200, 200); // 覆盖图尺寸
    public bool maintainAspectRatio = true; // 是否保持原图比例

    private int currentIndex = 0;
    private Image currentOverlayImage;

    void Start()
    {
        // 确保有Button组件
        Button button = GetComponent<Button>();
        if (button == null) button = gameObject.AddComponent<Button>();

        button.onClick.AddListener(ChangeOverlayImage);

        // 初始化覆盖Canvas
        if (overlayCanvas == null)
        {
            CreateOverlayCanvas();
        }
    }

    void CreateOverlayCanvas()
    {
        GameObject canvasObj = new GameObject("OverlayCanvas");
        overlayCanvas = canvasObj.AddComponent<Canvas>();
        overlayCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        overlayCanvas.sortingOrder = 1; // 比主UI高

        canvasObj.AddComponent<GraphicRaycaster>();
    }

    public void ChangeOverlayImage()
    {
        if (overlaySprites.Length == 0) return;

        // 创建或更新覆盖图片
        if (currentOverlayImage == null)
        {
            CreateNewOverlay();
        }
        else
        {
            UpdateOverlayImage();
        }

        // 循环索引
        currentIndex = (currentIndex + 1) % overlaySprites.Length;
    }

    void CreateNewOverlay()
    {
        GameObject overlayObj = new GameObject("OverlayImage");
        overlayObj.transform.SetParent(overlayCanvas.transform, false);

        currentOverlayImage = overlayObj.AddComponent<Image>();
        currentOverlayImage.sprite = overlaySprites[currentIndex];

        // 设置尺寸和位置
        RectTransform rt = overlayObj.GetComponent<RectTransform>();
        rt.sizeDelta = CalculateSize(overlaySprites[currentIndex]);

        // 居中显示（可根据需要调整）
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
    }

    void UpdateOverlayImage()
    {
        currentOverlayImage.sprite = overlaySprites[currentIndex];
        currentOverlayImage.GetComponent<RectTransform>().sizeDelta =
            CalculateSize(overlaySprites[currentIndex]);
    }

    Vector2 CalculateSize(Sprite sprite)
    {
        if (!maintainAspectRatio) return overlaySize;

        // 保持原图比例计算尺寸
        float spriteAspect = sprite.rect.width / sprite.rect.height;
        if (spriteAspect > 1)
        {
            return new Vector2(overlaySize.x, overlaySize.x / spriteAspect);
        }
        else
        {
            return new Vector2(overlaySize.y * spriteAspect, overlaySize.y);
        }
    }
}