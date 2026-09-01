using UnityEngine;

public class BellController : MonoBehaviour
{
    public int bellID;
    public AudioClip bellSound;
    public ParticleSystem hitEffect;

    private AudioSource audioSource;
    private const ItemName requiredTool = ItemName.编钟锤;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnMouseDown()
    {
        // 前置工具检测
        if (!IsHoldingCorrectTool())
            return;

        PlayBellSound();
        SequenceManager.Instance.AddBellToSequence(bellID);
        PlayHitEffect();
    }

    bool IsHoldingCorrectTool()
    {
        // 与InventoryManager联动的检测
        ItemDetails currentItem = InventoryManager.Instance.GetCurrentItem();
        return currentItem != null && currentItem.itemName == requiredTool;
    }

    void PlayBellSound()
    {
        if (bellSound != null && audioSource != null)
            audioSource.PlayOneShot(bellSound);
    }

    void PlayHitEffect()
    {
        if (hitEffect != null)
            hitEffect.Play();
    }
}