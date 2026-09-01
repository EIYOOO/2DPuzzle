using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick2 : MonoBehaviour
{
    // 这个函数会在物体被点击时触发
    private void OnMouseDown()
    {
        SceneManager.UnloadSceneAsync(8); 
    }
}
