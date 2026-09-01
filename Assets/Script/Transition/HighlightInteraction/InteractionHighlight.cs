using UnityEngine;

public class InteractionHighlight : MonoBehaviour
{
    private SpriteRenderer sprite;
    public Color hoverColor = new Color(1f, 0.92f, 0.016f, 1f); // ÁÁ»ÆÉ«

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnMouseEnter()
    {
        sprite.color = hoverColor;
    }

    private void OnMouseExit()
    {
        sprite.color = Color.white;
    }
}

//public class Apple : MonoBehaviour
//{
//    SpriteRenderer sprite;

//    private void Start()
//{
//    sprite = GetComponent<SpriteRenderer>();
//}

//private void OnMouseEnter()
//{
//    sprite.coLor = new Vector4(0.8f, 0.8f, 0.8f, 1);
//}
//private void OnMouseExit()
//{
//    sprite.color = new Vector4(1, 1, 1, 1);
//}
//}