using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BattleState
{
    Start,
    PlayerTurn,
    EnemyTurn,
    Win,
    Lose
}

public class battle_manager : MonoBehaviour
{
    [Header("Battle State")]
    public Transform playerSpawnPoint;
    public Transform enemySpawnPoint;

    private data_battle_player playerData;
    private data_battle_character enemyData;

    private data_battle_unit playerUnit;
    private data_battle_unit enemyUnit;

    private BattleState currentState;

    [Header("Battle Units (Spawned)")]
    private GameObject playerGO;
    private GameObject enemyGO;
    private Animator playerAnimator;
    private Animator enemyAnimator;

    [Header("Attack Move Settings")]
    public float moveForwardDistance = 1f;   // 1/2 - 1 langkah maju
    public float moveDuration = 0.2f;
    public float attackAnimDuration = 0.5f;  // durasi nunggu animasi sebelum hit
    public float turnDelay = 1f;             // jeda antar turn

    [Header("UI")]
    public battle_ui_manager battleUI;
    public Button attackButton;
    public TMP_Text turnText;

    [Header("Result UI")]
    public GameObject resultPanel;
    public TMP_Text resultText;

    public BattleResultData victoryData;
    public BattleResultData defeatData;

    [System.Serializable]
    public class BattleResultData
    {
        public string text;
        public Color color;
    }

    private void Start()
    {
        InitializeBattle();
        attackButton.onClick.AddListener(OnAttackButton);
    }

    private void InitializeBattle()
    {
        resultPanel.SetActive(false);

        playerData = data_manager_player.Instance.CurrentPlayer;
        enemyData = data_manager_player.Instance.CurrentEnemy;

        if (playerData == null || enemyData == null)
        {
            Debug.LogError("Missing Battle Data!");
            return;
        }

        playerUnit = new data_battle_unit(playerData);
        enemyUnit = new data_battle_unit(enemyData);

        SpawnUnits();

        battleUI.SetupPlayer(playerData, playerUnit);
        battleUI.SetupEnemy(enemyData, enemyUnit);

        StartBattle();
    }

    private void SpawnUnits()
    {
        playerGO = Instantiate(playerData.modelPrefab,
            playerSpawnPoint.position,
            playerSpawnPoint.rotation);

        enemyGO = Instantiate(enemyData.modelPrefab,
            enemySpawnPoint.position,
            enemySpawnPoint.rotation);

        playerAnimator = playerGO.GetComponentInChildren<Animator>();
        enemyAnimator = enemyGO.GetComponentInChildren<Animator>();

        if (playerAnimator == null)
            Debug.LogWarning("Player Animator tidak ditemukan di child object!");

        if (enemyAnimator == null)
            Debug.LogWarning("Enemy Animator tidak ditemukan di child object!");
    }

    private void StartBattle()
    {
        ChangeState(BattleState.Start);
    }

    public void OnAttackButton()
    {
        if (currentState != BattleState.PlayerTurn)
            return;

        attackButton.interactable = false;
        StartCoroutine(HandlePlayerAttack());
    }

    private IEnumerator HandlePlayerAttack()
    {
        Vector3 startPos = playerSpawnPoint.position;
        Vector3 forwardPos = startPos + playerSpawnPoint.forward * moveForwardDistance;

        // Maju
        yield return MoveTo(playerGO.transform, startPos, forwardPos, moveDuration);

        // Play animasi attack
        if (playerAnimator != null)
            playerAnimator.SetTrigger("Attack");

        yield return new WaitForSeconds(attackAnimDuration);

        // Apply damage setelah animasi selesai
        enemyUnit.CurrentHP -= playerUnit.Attack;
        battleUI.UpdateEnemyHP(enemyUnit);

        // Mundur ke posisi awal
        yield return MoveTo(playerGO.transform, forwardPos, startPos, moveDuration);

        if (enemyUnit.CurrentHP <= 0)
        {
            ChangeState(BattleState.Win);
            yield break;
        }

        yield return new WaitForSeconds(turnDelay);

        ChangeState(BattleState.EnemyTurn);
    }

    private void ChangeState(BattleState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case BattleState.Start:
                HandleBattleStart();
                break;

            case BattleState.PlayerTurn:
                attackButton.interactable = true;
                turnText.text = "Player Turn";
                break;

            case BattleState.EnemyTurn:
                turnText.text = "Enemy Turn";
                StartCoroutine(HandleEnemyTurn());
                break;

            case BattleState.Win:
                HandleWin();
                break;

            case BattleState.Lose:
                HandleLose();
                break;
        }
    }

    private void HandleBattleStart()
    {
        if (playerUnit.Speed >= enemyUnit.Speed)
            ChangeState(BattleState.PlayerTurn);
        else
            ChangeState(BattleState.EnemyTurn);
    }

    private IEnumerator HandleEnemyTurn()
    {
        attackButton.interactable = false;

        Vector3 startPos = enemySpawnPoint.position;
        Vector3 forwardPos = startPos + enemySpawnPoint.forward * moveForwardDistance;

        yield return new WaitForSeconds(0.2f); // delay kecil sebelum mulai

        // Maju
        yield return MoveTo(enemyGO.transform, startPos, forwardPos, moveDuration);

        // Play animasi attack
        if (enemyAnimator != null)
            enemyAnimator.SetTrigger("Attack");

        yield return new WaitForSeconds(attackAnimDuration);

        // Apply damage
        playerUnit.CurrentHP -= enemyUnit.Attack;
        battleUI.UpdatePlayerHP(playerUnit);

        // Mundur
        yield return MoveTo(enemyGO.transform, forwardPos, startPos, moveDuration);

        if (playerUnit.CurrentHP <= 0)
        {
            ChangeState(BattleState.Lose);
            yield break;
        }

        yield return new WaitForSeconds(turnDelay);

        ChangeState(BattleState.PlayerTurn);
    }

    private IEnumerator MoveTo(Transform target, Vector3 from, Vector3 to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            target.position = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        target.position = to;
    }

    private void HandleWin()
    {
        attackButton.interactable = false;

        enemyAnimator.SetTrigger("Defeat");

        data_manager_player.Instance.DefeatedEnemies.Add(
            data_manager_player.Instance.CurrentEnemyID
        );

        player_statistics.Instance.AddKill(
            enemyData.typeID
        );

        quest_manager.Instance.AddProgress(
            QuestObjectiveType.Kill,
            enemyData.typeID
        );

        ShowResult(victoryData);
    }

    private void HandleLose()
    {
        attackButton.interactable = false;
        playerAnimator.SetTrigger("Defeat");
        ShowResult(defeatData);
    }

    private void ShowResult(BattleResultData data)
    {
        resultPanel.SetActive(true);
        resultText.text = data.text;
    }

    public void ReturnToExploration()
    {
        SceneManager.LoadScene("ExplorationScene");
    }
}