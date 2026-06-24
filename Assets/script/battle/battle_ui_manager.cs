using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class battle_ui_manager : MonoBehaviour
{
    [Header("Player UI")]
    public TMP_Text playerNameText;
    public TMP_Text playerLevelText;
    public TMP_Text playerHpText;
    public Slider playerHpBar;

    [Header("Enemy UI")]
    public TMP_Text enemyNameText;
    public TMP_Text enemyHpText;
    public Slider enemyHpBar;

    public void SetupPlayer(data_battle_player playerData, data_battle_unit playerUnit)
    {
        playerNameText.text = playerData.playerName;
        playerLevelText.text = "Lv. " + playerData.level;

        UpdatePlayerHP(playerUnit);
    }

    public void SetupEnemy(data_battle_character enemyData, data_battle_unit enemyUnit)
    {
        enemyNameText.text = enemyData.characterName;

        UpdateEnemyHP(enemyUnit);
    }

    public void UpdatePlayerHP(data_battle_unit playerUnit)
    {
        playerHpBar.maxValue = playerUnit.MaxHP;
        playerHpBar.value = playerUnit.CurrentHP;

        playerHpText.text = $"{playerUnit.CurrentHP}/{playerUnit.MaxHP}";
    }

    public void UpdateEnemyHP(data_battle_unit enemyUnit)
    {
        enemyHpBar.maxValue = enemyUnit.MaxHP;
        enemyHpBar.value = enemyUnit.CurrentHP;

        enemyHpText.text = $"{enemyUnit.CurrentHP}/{enemyUnit.MaxHP}";
    }
}