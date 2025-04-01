using UnityEngine;

public class CameraControllers : MonoBehaviour
{
    [SerializeField] public Transform player;
    private Vector3 pos;
    private void Start()
    {
        FindLocalPlayer(); 
    }
    private void FindLocalPlayer()
    {
        var players = FindObjectsOfType<UnitRoot>();
        foreach (var p in players)
        {
            if (p.IsOwner) 
            {
                SetTarget(p.transform);
                break;
            }
        }
    }
    public void SetTarget(Transform newTarget)
    {
        player = newTarget;
    }
    private void Awake()
    {
        if (!player)
        {
            player = FindObjectOfType<UnitRoot>()?.transform;
        }
    }

    private void Update()
    {
        if (player == null) return;

        pos = player.position;
        pos.z = -10f;
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime);
    }
}