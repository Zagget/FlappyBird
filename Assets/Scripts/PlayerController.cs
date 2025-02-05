using UnityEngine;

public class PlayerController : Subject
{
    [SerializeField] private float upwardForce;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || TouchInput())
        {
            Flap();
        }
    }

    private bool TouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pipe"))
        {
            Debug.Log("Player Died");
            NotifyObservers(Events.Die);
            Destroy(gameObject);
            return;
        }

        if (other.CompareTag("PassedPipe"))
        {
            Debug.Log("Player passed a pipe");
            NotifyObservers(Events.PassedPipe);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (gameObject == null) return;

        if (other.CompareTag("Bounds"))
        {
            Debug.Log("Player got outside bounds");
            NotifyObservers(Events.Die);
            Destroy(gameObject);
        }
    }
}