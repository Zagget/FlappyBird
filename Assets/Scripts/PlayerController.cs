using UnityEngine;

public class PlayerController : Subject
{
    [SerializeField] private float upwardForce;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Flap();
        }
    }

    private void Flap()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);

        rb.AddForce(Vector2.up * upwardForce, ForceMode2D.Impulse);

        NotifyObservers(PlayerAction.Jump);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pipe"))
        {
            Debug.Log("Player Died");
            NotifyObservers(PlayerAction.Die);
            Destroy(gameObject);
            return;
        }

        if (other.CompareTag("PassedPipe"))
        {
            Debug.Log("Player passed a pipe");
            NotifyObservers(PlayerAction.PassedPipe);
        }
    }
}