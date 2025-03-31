using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] public float damage;
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float knockbackUpwardForce = 2f;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    public Animator animator;
    public GameObject canvasPrefab;
    private GameObject canvasInstance;

    public static Enemy Instance { get; private set; }

    private Vector3 direction;
    private Rigidbody2D rb;
    private bool facingRight = true;
    private float damageCooldown = 1f;
    private float lastDamageTime;
    private bool isDead = false; 

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (canvasPrefab != null)
        {
            canvasInstance = Instantiate(canvasPrefab, transform);
            canvasInstance.transform.localPosition = new Vector3(0, 2f, 0);
            hearts = canvasInstance.GetComponentsInChildren<Image>();
            UpdateHealthUI();
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isDead && UnitRoot.Instance != null) 
        {
            MoveTowardsHero();
        }
    }

    private void MoveTowardsHero()
    {
        if (isDead) return;
        Vector3 heroPosition = UnitRoot.Instance.transform.position;
        direction = (heroPosition - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        animator.SetBool("1_Move", true);
        CheckFlip(heroPosition.x);
    }

    private void CheckFlip(float heroX)
    {
        bool shouldFaceRight = heroX > transform.position.x;
        if (shouldFaceRight == facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(facingRight ? 1 : -1, transform.localScale.y, transform.localScale.z);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isDead) return;
        if (isDead || UnitRoot.Instance == null) return;

        if (collision.gameObject == UnitRoot.Instance.gameObject)
        {
            animator.SetBool("2_Attack", true);
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                UnitRoot.Instance.GetDamage(damage);
                TakeDamage(UnitRoot.Instance.GetHeroDamage(), collision.transform.position);
                lastDamageTime = Time.time;
            }
        }
        
        else if (collision.contacts[0].normal.y < -0.7f)
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    public void TakeDamage(float damage, Vector3 heroPosition)
    {
        if (hearts.Length > 0)
        {
            int lastIndex = hearts.Length - 1;
            hearts[lastIndex].enabled = false;
            Image[] newHearts = new Image[lastIndex];
            for (int i = 0; i < lastIndex; i++)
            {
                newHearts[i] = hearts[i];
            }
            hearts = newHearts;
        }

        ApplyKnockback(heroPosition);

        if (hearts.Length == 0) 
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true; 
        animator.SetBool("4_Death", true);
        damage = 0;
        speed = 0; 
        rb.linearVelocity = Vector2.zero; 

        if (canvasInstance != null)
        {
            Destroy(canvasInstance);
        }
        StartCoroutine(DestroyEnemyAfterDelay(1f));
    }

    private IEnumerator DestroyEnemyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void ApplyKnockback(Vector3 heroPosition)
    {
        if (rb == null || isDead) return; 

        Vector2 knockbackDirection = (transform.position - heroPosition).normalized;
        knockbackDirection.y = 0.5f;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(knockbackDirection * knockbackForce + Vector2.up * knockbackUpwardForce, ForceMode2D.Impulse);
    }

    private void UpdateHealthUI()
    {
        if (hearts == null || hearts.Length == 0)
        {
            return;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = fullHeart;
            hearts[i].enabled = true;
        }
    }
}
