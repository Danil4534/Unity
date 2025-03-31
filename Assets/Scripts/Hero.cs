using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using Unity.Netcode;

public class UnitRoot : NetworkBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] public float lives;
    [SerializeField] public float jumpForce;
    [SerializeField] public float damage;
    [SerializeField] private LayerMask groundLayer;

    [Header("Health UI")]
    [SerializeField] public Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] public GameObject weaponPrint;

    public bool isPaused = false;
    private bool isDead = false;
    private bool facingRight = true;
    public float heartsToDisplay;

    private Dictionary<string, KeyCode> keyBindings = new Dictionary<string, KeyCode>();
    public static UnitRoot Instance;

    [Header("Player Prefab")]
    [SerializeField] private GameObject playerPrefab;

    public SpriteRenderer leftHandWithWeapon;
    public SpriteRenderer rightHandWithShield;
    public Animator animator;
    public Rigidbody2D rb;
    AudioManager audioManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //else
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();

        LoadKeyBindings();
        animator.SetBool("4_Death", false);

        if (NetworkManager.Singleton)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
    }

    private void Start()
    {
        if (IsServer) // Сервер создает персонажей для всех игроков
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            SpawnPlayerForClientServerRpc(OwnerClientId);  // Спавн персонажа только для текущего клиента
        }
    }


    private void OnClientConnected(ulong clientId)
    {
        if (IsServer && clientId != NetworkManager.Singleton.LocalClientId) // Создаём персонажа только для новых клиентов
        {
            Debug.Log($"Client connected: {clientId}");
            SpawnPlayerForClientServerRpc(clientId);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnPlayerForClientServerRpc(ulong clientId)
    {
      
        if (clientId != NetworkManager.Singleton.LocalClientId && !IsPlayerSpawned(clientId))
        {
            Debug.Log($"Spawning player for client {clientId}");

          
            GameObject player = Instantiate(playerPrefab);
            player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        }
    }
    private bool IsPlayerSpawned(ulong clientId)
    {
        
        return NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(clientId) != null;
    }

    public void LoadKeyBindings()
    {
        keyBindings["MoveLeft"] = ParseKeyCode(PlayerPrefs.GetString("MoveLeftKey", "A"));
        keyBindings["MoveRight"] = ParseKeyCode(PlayerPrefs.GetString("MoveRightKey", "D"));
        keyBindings["Jump"] = ParseKeyCode(PlayerPrefs.GetString("JumpKey", "Space"));
        keyBindings["Attack"] = ParseKeyCode(PlayerPrefs.GetString("AttackKey", "Mouse0"));
    }

    private KeyCode ParseKeyCode(string key)
    {
        if (Enum.TryParse(key.ToUpper(), out KeyCode result))
        {
            return result;
        }
        return KeyCode.A;
    }

    private void Update()
    {
        if (isPaused || isDead || !IsOwner) return;

        if (Input.GetKey(keyBindings["MoveLeft"]))
        {
            MoveServerRpc(-1);
        }
        else if (Input.GetKey(keyBindings["MoveRight"]))
        {
            MoveServerRpc(1);
        }
        else
        {
            animator.SetBool("1_Move", false);
        }

        if (Input.GetKeyDown(keyBindings["Jump"]))
        {
            JumpServerRpc();
        }

        if (Input.GetKeyDown(keyBindings["Attack"]))
        {
            AttackServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void MoveServerRpc(int direction)
    {
        MoveClientRpc(direction);
    }

    [ClientRpc]
    private void MoveClientRpc(int direction)
    {
        if (isDead) return;
        transform.position += new Vector3(direction * speed * Time.deltaTime, 0, 0);
        animator.SetBool("1_Move", true);
        if (direction < 0 && facingRight) Flip();
        else if (direction > 0 && !facingRight) Flip();
    }

    [ServerRpc]
    private void JumpServerRpc()
    {
        JumpClientRpc();
    }

    [ClientRpc]
    private void JumpClientRpc()
    {
        if (isDead) return;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    [ServerRpc]
    private void AttackServerRpc()
    {
        AttackClientRpc();
    }

    [ClientRpc]
    private void AttackClientRpc()
    {
        weaponPrint.SetActive(true);
        animator.SetBool("2_Attack", true);
    }

    public void GetDamage(float enemyDamage)
    {
        if (isDead) return;
        lives -= enemyDamage;
        UpdateHealthUI();
        damageText.text = "-" + enemyDamage.ToString();
        StartCoroutine(FadeOutDamageText());

        if (lives <= 0)
        {
            isDead = true;
            animator.SetBool("4_Death", true);
            audioManager?.PlaySFX(audioManager.dead);
            rb.simulated = false;
            isPaused = true;
            StartCoroutine(DefeatGame(1));
        }
    }

    private IEnumerator DefeatGame(int level)
    {
        yield return new WaitForSeconds(1f);
        GetComponent<Collider2D>().enabled = false;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) spriteRenderer.enabled = false;
        PlayerPrefs.SetString("LevelStatus" + level, "defeat");
        SceneManager.LoadScene("Defeat");
    }

    private IEnumerator FadeOutDamageText()
    {
        Color color = damageText.color;
        float duration = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, elapsedTime / duration);
            damageText.color = color;
            yield return null;
        }

        damageText.text = "";
        color.a = 1;
        damageText.color = color;
    }

    public void UpdateHealthUI()
    {
        heartsToDisplay = Mathf.Clamp(lives / 20, 0, hearts.Length);
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = i < heartsToDisplay ? fullHeart : emptyHeart;
            hearts[i].enabled = i < hearts.Length;
        }
    }

    public float GetHeroDamage()
    {
        return damage;
    }

    public void Move(int direction)
    {
        if (isDead) return;

        Vector3 moveDir = new Vector3(direction, 0, 0);
        transform.position = Vector3.MoveTowards(transform.position, transform.position + moveDir, speed * Time.deltaTime);
        animator.SetBool("1_Move", true);
        weaponPrint.SetActive(false);
        if (direction < 0 && facingRight)
        {
            Flip();
        }
        else if (direction > 0 && !facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
