using UnityEngine;

public class DialogueImage : MonoBehaviour
{
    public DialogueManager manager;

    void OnMouseDown()
    {
        manager.GoToNextImage();
    }
}
