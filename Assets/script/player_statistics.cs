using System.Collections.Generic;
using UnityEngine;

public class player_statistics : MonoBehaviour
{
    public static player_statistics Instance;

    private Dictionary<string, int> killCounts =
        new Dictionary<string, int>();

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

    public void AddKill(string enemyTypeID)
    {
        if (!killCounts.ContainsKey(enemyTypeID))
            killCounts.Add(enemyTypeID, 0);

        killCounts[enemyTypeID]++;

        Debug.Log($"Kill Recorded: {enemyTypeID} = {killCounts[enemyTypeID]}");
    }

    public int GetKillCount(string enemyTypeID)
    {
        if (killCounts.TryGetValue(enemyTypeID, out int count))
            return count;

        return 0;
    }

    public void ResetData()
    {
        killCounts.Clear();
    }
}