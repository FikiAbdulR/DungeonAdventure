using UnityEngine;
using UnityEngine.AI;

public class RoadBlock : MonoBehaviour
{
    [Header("Unique ID")]
    [Tooltip("Harus unik per roadblock di seluruh game")]
    [SerializeField] private string roadblockID;

    private NavMeshObstacle blockade;

    private void Awake()
    {
        blockade = GetComponent<NavMeshObstacle>();

        bool isDisabled = data_world_state_manager.Instance != null &&
                           data_world_state_manager.Instance.IsBlockadeDisabled(roadblockID);

        blockade.enabled = !isDisabled;
    }

    public void DisableBlockade()
    {
        blockade.enabled = false;
        data_world_state_manager.Instance.SetBlockadeDisabled(roadblockID);
    }

    public void EnableBlockade()
    {
        blockade.enabled = true;
        data_world_state_manager.Instance.SetBlockadeEnabled(roadblockID);
    }
}