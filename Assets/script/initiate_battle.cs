using UnityEngine;

public class initiate_battle : MonoBehaviour
{
    private void Start()
    {
        music_manager.Instance.PlayBattle();
    }
}
