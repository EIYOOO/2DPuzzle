using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class CursorManager : MonoBehaviour
{
    private VideoPlayer goToVideo;
    private GameObject goToGameObject;
    private Item item;

    private ItemName currentItem;

    private Vector3 mouseWorldPosition =>
        Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

    private bool canClick;

    private void OnEnable()
    {
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
        EventHandler.ItemUsedEvent += OnItemUsedEvent;
    }
    private void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
        EventHandler.ItemUsedEvent -= OnItemUsedEvent;
    }

    private void Update()
    {
        canClick = ObjectAtMousePosition();

        /*if (hand.gameObject.activeInHierarchy)
        {
            hand.position = Input.mousePosition;
        }*/

        if (canClick && Input.GetMouseButtonDown(0))
        {
            ClickAction(ObjectAtMousePosition().gameObject);
        }
    }


    private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        //holdItem = isSelected;
        if (isSelected)
        {
            currentItem = itemDetails.itemName;
        }
        /*if (holdItem != hand.gameObject.activeSelf)
        {
            hand.gameObject.SetActive(holdItem);
        }*/
    }

    private void OnItemUsedEvent(ItemName obj)
    {
        currentItem = ItemName.None;
        //holdItem = false;
        //hand.gameObject.SetActive(false);
    }

    private void ClickAction(GameObject clickedObject)
    {
        switch (clickedObject.tag)
        {
            case "Teleport":
                var teleport = clickedObject.GetComponent<Teleport>();
                teleport?.TeleportToScene();
                break;
            case "Video":
                var videos = clickedObject.gameObject.GetComponentsInChildren<VideoPlayer>();
                string currentObjectName = clickedObject.name;

                if (videos != null && videos.Length > 0)
                {
                    StartCoroutine(PlayVideosSequentially(videos, clickedObject, currentObjectName));
                }
                break;
            case "Item":
                item = clickedObject.GetComponent<Item>();
                if (item != null)
                {
                    item.canClickCollection = true;
                    Debug.Log($"已选中 Item: {item.name} (可点击 Collection)");
                }
                else
                {
                    Debug.LogError($"对象 '{clickedObject.name}' 标记为 Item，但未挂载 Item 组件！");
                }
                break;

            case "Collection":
                if (item == null)
                {
                    Debug.LogWarning("Item未初始化！请先点击带有'Item'标签的对象.");
                }
                else if (!item.canClickCollection)
                {
                    Debug.LogWarning("Item未激活可收集状态！请确认已正确点击Item.");
                }
                else
                {
                    item.ItemClicked();
                    item.canClickCollection = false;
                    item = null;
                }
                break;
        }
    }

    /// <summary>
    /// 检测碰撞体
    /// </summary>
    /// <returns></returns>
    private Collider2D ObjectAtMousePosition()
    {
        return Physics2D.OverlapPoint(mouseWorldPosition);
    }

    /// <summary>
    /// 顺序播放
    /// </summary>
    /// <param name="videos"></param>
    /// <param name="clickedObject"></param>
    /// <param name="currentObjectName"></param>
    /// <returns></returns>
    private IEnumerator PlayVideosSequentially(VideoPlayer[] videos, GameObject clickedObject, string currentObjectName)
    {
        //if (currentObjectName == "GoTo")
        //{
        //    yield return new WaitForSeconds(5.6f);
        //}

        foreach (var video in videos)
        {
            bool videoFinished = false;

            video.loopPointReached += (VideoPlayer vp) => { videoFinished = true; };
            if (currentObjectName == "Curtain")
            {
                goToGameObject.SetActive(false);
            }
            video.Play();

            if (currentObjectName == "GoTo")
            {
                goToVideo = video;
                goToGameObject = clickedObject;
                ActivateInactiveObject("Curtain");
            }

            yield return new WaitUntil(() => videoFinished);

            video.loopPointReached -= (VideoPlayer vp) => { videoFinished = true; };
        }

        StartCoroutine(DeactivateAfterVideo(clickedObject, currentObjectName));
    }

    /// <summary>
    /// 等待失活激活
    /// </summary>
    /// <returns></returns>
    private IEnumerator DeactivateAfterVideo(GameObject objectToDeactivate, string currentObjectName)
    {
        Debug.Log("当前物体名称: '" + currentObjectName + "'");

        yield return new WaitForSeconds(0.1f);
        objectToDeactivate.SetActive(false);

        Debug.Log("准备激活");
        switch (currentObjectName)
        {
            case "Close":
                ActivateInactiveObject("Open");
                break;

            case "Open":
                ActivateInactiveObject("GoTo");
                break;

            case "GoTo":
                ActivateInactiveObject("CloseCurtain");
                break;

            case "Curtain":
                // 叠加加载 Scene1，保留 Persistent 场景
                SceneManager.LoadScene("Scene1", LoadSceneMode.Additive);
                break;

            default:
                ActivateInactiveObject("");
                Debug.LogWarning("没有匹配的物体名称！");
                break;
        }
    }

    /// <summary>
    /// 找物体激活
    /// </summary>
    /// <param name="objectName"></param>
    private void ActivateInactiveObject(string objectName)
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>(true);
        bool found = false;

        Debug.Log("当前场景中的所有物体名称:");

        foreach (GameObject obj in allObjects)
        {
            Debug.Log(obj.name);

            if (obj.name == objectName)
            {
                obj.SetActive(true);
                Debug.Log(objectName + "已激活");
                found = true;
                break;
            }
        }

        if (!found)
        {
            Debug.LogWarning("没有找到名称为 " + objectName + "的物体");
        }
    }
}
