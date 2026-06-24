using UnityEngine;
using System.Collections.Generic;

public class data_manager_player : MonoBehaviour
{
    public static data_manager_player Instance;

    [Header("Player (SO)")]
    public data_battle_player CurrentPlayer;

    [Header("Enemy")]
    public data_battle_character CurrentEnemy;
    public string CurrentEnemyID;

    public HashSet<string> DefeatedEnemies =
        new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetData()
    {
        CurrentEnemy = null;
        CurrentEnemyID = null;
        DefeatedEnemies.Clear();

        Debug.Log("Player Data Manager: data direset");
    }
}