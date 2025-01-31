using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] bool scrollLeft;

    float singleTextureWidth;

    void Start()
    {
        SetupTexture();
        if (scrollLeft) moveSpeed = -moveSpeed;
    }

    void SetupTexture()
    {
        // gameobjects scale s
        float scale = transform.localScale.x;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        singleTextureWidth = sprite.texture.width / sprite.pixelsPerUnit * scale;
    }

    void Update()
    {
        Scroll();
        CheckReset();
    }


    void Scroll()
    {
        float delta = moveSpeed * Time.deltaTime;
        transform.position += new Vector3(delta, 0, 0);
    }

    void CheckReset()
    {
        if ((Mathf.Abs(transform.position.x) - singleTextureWidth) > 0)
        {
            transform.position = new Vector3(0.0f, transform.position.y, transform.position.z);
        }
    }
}
