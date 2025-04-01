using UnityEngine;
using Unity.Netcode;

public class Weapon : NetworkBehaviour
{
    public static Weapon Instance { get; private set; }
    private SpriteRenderer spriteRenderer;
    public Sprite weaponSprite;
    [SerializeField] public string typeWeapon;

    
    private NetworkVariable<bool> isPickedUp = new NetworkVariable<bool>(false);

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsServer) return;  

        if (UnitRoot.Instance == null) return;
        if (collision.gameObject == UnitRoot.Instance.gameObject)
        {
  
            if (typeWeapon == "handWeapon")
            {
                UnitRoot.Instance.leftHandWithWeapon.sprite = weaponSprite;
            }
            if (typeWeapon == "shieldWeapon")
            {
                UnitRoot.Instance.rightHandWithShield.sprite = weaponSprite;
            }

            isPickedUp.Value = true;

            PickupWeaponClientRpc();

            spriteRenderer.enabled = false;
            Destroy(gameObject);
        }
    }


    [ClientRpc]
    private void PickupWeaponClientRpc()
    {
        if (!IsOwner) return;  
        spriteRenderer.enabled = false;  
    }
}