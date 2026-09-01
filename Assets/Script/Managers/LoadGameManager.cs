using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadGame : MonoBehaviour
{
    public void LoadingGame()
    {
        DestroyAllPersistentObjects();
        // 卸载场景 1，加载场景 2 并添加 Persistent 场景
        SceneManager.UnloadSceneAsync(1);
        //SceneManager.LoadScene(2);
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
        
    }
    
    private void DestroyAllPersistentObjects()
    {
        // 销毁所有不销毁的对象
        GameObject[] persistentObjects = GameObject.FindGameObjectsWithTag("Persistent");
        foreach (GameObject obj in persistentObjects)
        {
            Destroy(obj);
        }
    }
}