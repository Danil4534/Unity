using UnityEngine;

public class Weapon : MonoBehaviour
{
    public static Weapon Instance { get; private set; }
    private SpriteRenderer spriteRenderer;
    public Sprite weaponSprite;
    [SerializeField] public string typeWeapon;

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
        if ( UnitRoot.Instance == null) return;
        if (collision.gameObject == UnitRoot.Instance.gameObject)
        {
          if(typeWeapon == "handWeapon")
            {
                UnitRoot.Instance.leftHandWithWeapon.sprite = weaponSprite;
                
            }  
           if(typeWeapon == "shieldWeapon")
            {
                UnitRoot.Instance.rightHandWithShield.sprite = weaponSprite;
            }
           
            spriteRenderer.enabled = false;
            Destroy(gameObject);
        }
    }
    void Update()
    {
        
    }
}
