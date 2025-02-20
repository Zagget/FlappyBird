using UnityEngine;

public class PlayerController : Subject
{
    private Rigidbody2D rb;
    [SerializeField] private float upwardForce;

    private float halfScreenHeight;
    private float verticalBounds;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        halfScreenHeight = Camera.main.orthographicSize;
        verticalBounds = halfScreenHeight + 1.5f;
    }

    private void Update()
    {
        CheckOutOfBounds();
        if (Input.GetMouseButtonDown(0) || TouchInput())
        {
            Flap();
        }
    }

    private void CheckOutOfBounds()
    {
        if (transform.position.y > verticalBounds || transform.position.y < -verticalBounds)
        {
            NotifyObservers(Events.Die);
            Destroy(gameObject);
        }
    }

    private bool TouchInput()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                return true;
            }
        }
        return false;
    }

    private void Flap()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);

        rb.AddForce(Vector2.up * upwardForce, ForceMode2D.Impulse);

        NotifyObservers(Events.Jump);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pipe"))
        {
            NotifyObservers(Events.Die);
            Destroy(gameObject);
            return;
        }

        if (other.CompareTag("PassedPipe"))
        {
            NotifyObservers(Events.PassedPipe);
        }
    }
}