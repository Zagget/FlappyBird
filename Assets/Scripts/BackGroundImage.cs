using UnityEngine;

public class BackgroundImage : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    float singleTextureWidth;

    void Start()
    {
        SetupTexture();

    }

    public void SetScrollDirection(bool scrollLeft)
    {
        if (scrollLeft) moveSpeed = -moveSpeed;
    }

    void SetupTexture()
    {
        float scale = transform.localScale.x;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        singleTextureWidth = sprite.texture.width / sprite.pixelsPerUnit * scale;
    }

    public void Scroll()
    {
        float delta = moveSpeed * Time.deltaTime;
        transform.position += new Vector3(delta, 0, 0);
    }

    public void CheckReset()
    {
        if (Mathf.Abs(transform.position.x) > singleTextureWidth)
        {
            transform.position = new Vector3(0.0f, transform.position.y, transform.position.z);
        }
    }
}