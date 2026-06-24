using UnityEngine;

public class data_enemy_cleaner : MonoBehaviour
{
    private void Start()
    {
        var enemies = FindObjectsOfType<batte_encounter_enemy>();

        foreach (var enemy in enemies)
        {
            if (data_manager_player.Instance.DefeatedEnemies.Contains(enemy.instanceID))
            {
                gameObject.SetActive(false);
            }
        }
    }
}