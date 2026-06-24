using UnityEngine;

public class initiate_explore : MonoBehaviour
{
    private void Start()
    {
        music_manager.Instance.PlayExploration();
    }
}
