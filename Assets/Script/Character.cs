using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Character : MonoBehaviour
{
    //public Animator anim;
    protected Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Color originalColor;
    public DeathUIManager deathUIManager;   // assign in Inspector

    [SerializeField] private int health;
    [SerializeField] private int maxHealth = 100;

    public int Health
    {
        get { return health; }
        set
        {
            health = Mathf.Clamp(value, 0, maxHealth);

            if (isPlayer && GameManager.Instance != null)
                GameManager.Instance.SetPlayerHealth(health);

            // Update health bar if assigned
            if (uiHealthBarrrrrr != null)
                uiHealthBarrrrrr.UpdateHealthBar(health);

        }
    }

    [SerializeField] private bool isPlayer = false; // set in inspector

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
            originalColor = sprite.color;   // save starting color
    }

    [SerializeField] private HealthBar uiHealthBarrrrrr;


    public bool IsDead()
    {
        if (health <= 0)
        {
            if (isPlayer)
            {
                Player playerScript = GetComponent<Player>();
                if (playerScript != null)
                {
                    playerScript.Die(); // ??????? Player ???????????? Analytics ????????
                }

                if (deathUIManager != null)
                    deathUIManager.ShowDeathUI();
            }
            else // ????????????
            {
            }

            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySFX(isPlayer ? "PlayerDead" : "EnemyDead");
            }

            Destroy(this.gameObject); // ????????????????????????????????????????
            return true;
        }
        else return false;
    }
    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(isPlayer ? "PlayerHit" : "EnemyHit");
        }

        StartCoroutine(FlashRed());
        IsDead();
    }

    IEnumerator FlashRed()
    {
        if (sprite != null)
        {
            sprite.color = Color.red;               // flash red
            yield return new WaitForSeconds(0.1f);
            sprite.color = originalColor;           // restore original color
        }
    }
    public void Init(int newHealth)
    {
        if (!isPlayer)
            maxHealth = Mathf.Max(1, newHealth);

        Health = newHealth;
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        if (uiHealthBarrrrrr != null)
        {
            uiHealthBarrrrrr.SetMaxHealth(maxHealth);
            uiHealthBarrrrrr.UpdateHealthBar(health);
        }


    }
}
