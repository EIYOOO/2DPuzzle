using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SequenceManager : MonoBehaviour
{
    public static SequenceManager Instance;

    [Header("音序设置")]
    public int[] correctSequence;
    private List<int> inputSequence = new List<int>();

    [Header("工具检测")]
    public ItemName requiredTool = ItemName.编钟锤;

    [Header("音效设置")]
    public AudioClip correctSound;
    public AudioClip wrongSound;
    public AudioClip successSound;
    private AudioSource audioSource;

    [Header("事件")]
    public UnityEvent onUnlockSuccess;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void AddBellToSequence(int bellID)
    {
        // 静默工具检测
        if (!IsHoldingCorrectTool())
            return;

        inputSequence.Add(bellID);
        CheckSequence();
    }

    private bool IsHoldingCorrectTool()
    {
        // 与SlotUI联动的检测逻辑
        ItemDetails currentItem = InventoryManager.Instance.GetCurrentItem();

        // 双重空值保护
        if (currentItem == null)
        {
            Debug.Log("当前未持有任何工具");
            return false;
        }

        // 精确匹配工具类型
        bool isValid = currentItem.itemName == requiredTool;
        if (!isValid)
        {
            Debug.Log($"禁止使用 {currentItem.itemName} 互动，需要 {requiredTool}");
        }
        return isValid;
    }

    void CheckSequence()
    {
        int currentStep = inputSequence.Count - 1;

        // 序列验证
        if (inputSequence[currentStep] != correctSequence[currentStep])
        {
            HandleWrongSequence();
            return;
        }

        // 最终验证
        if (inputSequence.Count == correctSequence.Length)
        {
            HandleSuccess();
        }
        else
        {
            PlaySound(correctSound);
        }
    }

    void HandleWrongSequence()
    {
        PlaySound(wrongSound);
        ResetSequence();
        Debug.Log("音序错误，已重置");
    }

    void HandleSuccess()
    {
        PlaySound(successSound);
        onUnlockSuccess.Invoke();
        ResetSequence();
        Debug.Log("谜题解锁成功！");
    }

    void ResetSequence()
    {
        inputSequence.Clear();
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
            audioSource.PlayOneShot(clip);
    }
}