using UnityEngine;

public class Interact : MonoBehaviour
{
    SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnMouseEnter()
    {
        sprite.color = new Vector4(0.8f, 0.8f, 0.8f, 1);
    }
    private void OnMouseExit()
    {
        sprite.color = new Vector4(1, 1, 1, 1);
    }
}