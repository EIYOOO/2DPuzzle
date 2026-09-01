using UnityEngine;

public class SpriteClickToClose : MonoBehaviour
{
    void OnMouseDown()
    {
        // 直接关闭自身或父物体
        gameObject.SetActive(false);
    }
}
