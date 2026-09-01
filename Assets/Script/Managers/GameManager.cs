using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("Begin", LoadSceneMode.Additive);
    }

    public void StartNewGame()
    {
        PlayerPrefs.DeleteKey("MudGearCleanStage"); // 删除之前保存的清洁状态
        PlayerPrefs.Save(); // 确保保存
        Debug.Log("新游戏已开始，清洁状态已重置！");
    }
}