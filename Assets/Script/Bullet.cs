using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    [SerializeField] private float castRadius = 0.05f;

    private Vector2 direction;
    private Rigidbody2D rb;
    private Collider2D ownCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ownCollider = GetComponent<Collider2D>();

        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }
    }

    public void Init(Vector2 shootDir, float bulletSpeed)
    {
        direction = shootDir.normalized;
        speed = bulletSpeed;
    }

    private void Start()
    {
        Destroy(gameObject, 5f); // หายเอง 5 วิ
    }

    private void FixedUpdate()
    {
        if (direction == Vector2.zero || speed <= 0f) return;

        float stepDistance = speed * Time.fixedDeltaTime;
        Vector2 currentPosition = rb != null ? rb.position : (Vector2)transform.position;

        // Sweep ahead so fast bullets cannot tunnel through thin colliders.
        RaycastHit2D[] hits = Physics2D.CircleCastAll(currentPosition, castRadius, direction, stepDistance);
        for (int i = 0; i < hits.Length; i++)
        {
            Collider2D hitCollider = hits[i].collider;
            if (hitCollider == null || hitCollider == ownCollider) continue;

            if (TryHandleHit(hitCollider))
                return;
        }

        Vector2 nextPosition = currentPosition + direction * stepDistance;
        if (rb != null)
            rb.MovePosition(nextPosition);
        else
            transform.position = nextPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TryHandleHit(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryHandleHit(collision.collider);
    }

    private bool TryHandleHit(Collider2D collision)
    {
        if (collision == null || collision == ownCollider) return false;
        if (collision.CompareTag("Player")) return false;

        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
            return true;
        }

        Destroy(gameObject);
        return true;
    }
}
