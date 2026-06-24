using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class batte_encounter_enemy : MonoBehaviour
{
    public data_battle_character enemyData;
    public string instanceID; // enemy_01
    public string typeID;     // enemy

    private void Awake()
    {
        Debug.Log("SPAWN ENEMY: " + instanceID);
    }

    private IEnumerator Start()
    {
        yield return null;

        if (data_manager_player.Instance.DefeatedEnemies.Contains(instanceID))
        {
            gameObject.SetActive(false);
            yield break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (data_manager_player.Instance.DefeatedEnemies.Contains(instanceID))
            return; // BLOCK TOTAL

        battle_holder_player player =
            other.GetComponent<battle_holder_player>();

        if (player == null)
            return;

        data_manager_player.Instance.CurrentPlayer = player.playerData;
        data_manager_player.Instance.CurrentEnemy = enemyData;
        data_manager_player.Instance.CurrentEnemyID = instanceID;

        data_world_state_manager.Instance.PlayerPosition = player.transform.position;

        Debug.Log("Battle Start Enemy ID: " + instanceID);

        SceneManager.LoadScene("BattleScene");
    }
}