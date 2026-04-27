using UnityEngine;

public class Enemy : Character
{
    public int damageHit = 10;
    private Transform player;
    public float speed = 2f;
    public float followRange = 6f;

    public float avoidRadius = 0.8f;
    public float avoidStrength = 2f;
    public float contactDamageCooldown = 0.5f;

    private Vector2 moveDirection;
    private float nextDamageTime;

    public int DamageHit
    {
        get
        {
            return damageHit;
        }
        set
        {
            damageHit = value;
        }
    }

    private void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
            player = playerObj.transform;

    }

    private void Update()
    {
        Enemymove();
    }

    private void FixedUpdate()
    {
        if (player == null || rb == null) return;

        Vector2 nextPosition = rb.position + moveDirection * speed * Time.fixedDeltaTime;
        rb.MovePosition(nextPosition);
    }

    public void Enemymove() // enemy เดินหาผู้เล่น
    {
        if (player == null) return;

        float distanceToPlayerSqr = ((Vector2)player.position - (Vector2)transform.position).sqrMagnitude;
        float followRangeSqr = followRange * followRange;
        if (distanceToPlayerSqr > followRangeSqr)
        {
            moveDirection = Vector2.zero;
            return;
        }

        Vector2 directionToPlayer = ((Vector2)player.position - (Vector2)transform.position).normalized;
        Vector2 separation = SeparateFromOthers();
        moveDirection = (directionToPlayer + separation).normalized;

    }

    Vector2 SeparateFromOthers()
    {
        Vector2 totalPush = Vector2.zero;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, avoidRadius);

        foreach (var hit in hits)
        {
            if (hit.gameObject != gameObject && hit.CompareTag("Enemy"))
            {
                Vector2 push = (Vector2)(transform.position - hit.transform.position).normalized;
                totalPush += push;
            }
        }

        return totalPush * avoidStrength;
    }

    private void TryDamagePlayer(Component other)
    {
        if (Time.time < nextDamageTime) return;

        if (other.CompareTag("Player"))
        {
            Character player = other.GetComponent<Character>();
            if (player != null)
            {
                player.TakeDamage(damageHit);
                nextDamageTime = Time.time + contactDamageCooldown;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TryDamagePlayer(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        TryDamagePlayer(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryDamagePlayer(collision.collider);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryDamagePlayer(collision.collider);
    }


}
