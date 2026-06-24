using UnityEngine;

public class hero_initializer : MonoBehaviour
{
    [SerializeField] private data_battle_player playerTemplate;

    private void Start()
    {
        data_manager_player.Instance.CurrentPlayer = playerTemplate;

        Debug.Log("Player Loaded: " + playerTemplate.playerName);
    }
}