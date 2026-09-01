using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour
{
    // 这个函数会在物体被点击时触发
    private void OnMouseDown()
    {
        // 加载场景编号为6的场景
        SceneManager.LoadScene(8, LoadSceneMode.Additive); // 使用场景编号加载，并添加到当前场景
    }
}
