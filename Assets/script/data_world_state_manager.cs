using System.Collections.Generic;
using UnityEngine;

public class data_world_state_manager : MonoBehaviour
{
    public static data_world_state_manager Instance;

    public Vector3 PlayerPosition;
    [Header("Default Spawn")]
    [SerializeField] private Vector3 defaultSpawnPosition = new Vector3(0f, 0f, 0f);

    public HashSet<string> CompletedCutscenes =
        new HashSet<string>();
    public HashSet<string> DefeatedEnemies =
        new HashSet<string>();
    public HashSet<string> DisabledBlockades =
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

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerPosition = player.transform.position;
        }
    }

    // ===== Roadblock =====

    public void SetBlockadeDisabled(string id)
    {
        DisabledBlockades.Add(id);
    }

    public void SetBlockadeEnabled(string id)
    {
        DisabledBlockades.Remove(id);
    }

    public bool IsBlockadeDisabled(string id)
    {
        return DisabledBlockades.Contains(id);
    }

    public void ResetData()
    {
        CompletedCutscenes.Clear();
        DefeatedEnemies.Clear();
        DisabledBlockades.Clear();
        PlayerPosition = defaultSpawnPosition;
    }
}