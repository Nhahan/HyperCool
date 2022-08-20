using UnityEngine;
using UnityEngine.UI;
 
[RequireComponent(typeof(Image))]
public class UISpritesAnimation : MonoBehaviour
{
    private Image image;
    private new SpriteRenderer renderer;

    private void Start()
    {
        image = GetComponent<Image>();
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Update ()
    {
        image.sprite = renderer.sprite;
    }
}