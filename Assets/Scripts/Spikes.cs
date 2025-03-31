using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] public float spikeDamage;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( UnitRoot.Instance == null) return;
        if (collision.gameObject == UnitRoot.Instance.gameObject)
        {
            UnitRoot.Instance.GetDamage(spikeDamage);

        }
    }

    void Update()
    {
        
    }
}
