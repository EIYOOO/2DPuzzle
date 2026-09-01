// InteractionManager.cs
using UnityEngine;
using System.Collections.Generic;

public class InteractionManager : MonoBehaviour
{
    [Header("基础设置")]
    [Tooltip("需要高亮的物体标签")]
    public string interactTag = "Interactable";
    [Tooltip("高亮颜色")]
    public Color highlightColor = new Color(1f, 0.92f, 0.016f, 1f); // 亮黄色
    [Tooltip("检测层级（优化性能）")]
    public LayerMask interactLayer;

    [Header("运行时状态")]
    [SerializeField] private List<SpriteRenderer> interactableRenderers = new List<SpriteRenderer>();
    private Dictionary<SpriteRenderer, Color> originalColors = new Dictionary<SpriteRenderer, Color>();
    private SpriteRenderer lastHovered;

    public static InteractionManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSystem();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeSystem()
    {
        // 清空旧数据
        interactableRenderers.Clear();
        originalColors.Clear();

        // 自动查找所有带标签的物体
        GameObject[] interactables = GameObject.FindGameObjectsWithTag(interactTag);
        foreach (GameObject obj in interactables)
        {
            CacheRenderer(obj);
        }
    }

    void Update()
    {
        CheckMouseInteraction();
    }

    void CheckMouseInteraction()
    {
        // 将鼠标位置转换为世界坐标
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // 2D场景z轴归零

        // 使用射线检测
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0, interactLayer);

        if (hit.collider != null)
        {
            HandleHover(hit.collider.gameObject);
        }
        else
        {
            ClearHover();
        }
    }

    void HandleHover(GameObject target)
    {
        SpriteRenderer currentRenderer = target.GetComponent<SpriteRenderer>();

        // 如果检测到新物体
        if (currentRenderer != null && currentRenderer != lastHovered)
        {
            // 还原上一个物体的颜色
            if (lastHovered != null && originalColors.ContainsKey(lastHovered))
            {
                lastHovered.color = originalColors[lastHovered];
            }

            // 存储并设置新物体颜色
            if (interactableRenderers.Contains(currentRenderer))
            {
                lastHovered = currentRenderer;
                currentRenderer.color = highlightColor;
            }
        }
    }

    void ClearHover()
    {
        if (lastHovered != null)
        {
            lastHovered.color = originalColors[lastHovered];
            lastHovered = null;
        }
    }

    void CacheRenderer(GameObject obj)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            interactableRenderers.Add(sr);
            originalColors[sr] = sr.color; // 存储原始颜色
        }
    }

    // 外部调用方法：动态添加可交互物体
    public void RegisterInteractable(GameObject newObj)
    {
        if (newObj.CompareTag(interactTag))
        {
            CacheRenderer(newObj);
        }
    }

    // 外部调用方法：移除交互物体
    public void UnregisterInteractable(GameObject removedObj)
    {
        SpriteRenderer sr = removedObj.GetComponent<SpriteRenderer>();
        if (sr != null && interactableRenderers.Contains(sr))
        {
            interactableRenderers.Remove(sr);
            originalColors.Remove(sr);
        }
    }
}