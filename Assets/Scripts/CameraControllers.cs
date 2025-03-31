using UnityEngine;

public class CameraControllers : MonoBehaviour
{
    [SerializeField] public Transform player;
    private Vector3 pos;

    private void Awake()
    {
        //if (!player)
        //{

        //    player = FindObjectOfType<UnitRoot>()?.transform;

        //    if (player == null)
        //    {
        //        Debug.LogError("UnitRoot не найден! Убедитесь, что объект с компонентом UnitRoot существует в сцене.");
        //    }
        //}
    }

    private void Update()
    {
        //if (player == null) return;

        //pos = player.position;
        //pos.z = -10f;
        //transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime);
    }
}