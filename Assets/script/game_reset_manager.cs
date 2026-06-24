using UnityEngine;
using UnityEngine.SceneManagement;

public class game_reset_manager : MonoBehaviour
{
    public static game_reset_manager Instance;

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

    public void ResetAndGoToMainMenu(string mainMenuSceneName = "MainMenu")
    {
        ResetAllData();
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void ResetAllData()
    {
        if (quest_manager.Instance != null)
            quest_manager.Instance.ResetData();

        if (data_manager_player.Instance != null)
            data_manager_player.Instance.ResetData();

        if (data_world_state_manager.Instance != null)
            data_world_state_manager.Instance.ResetData();

        Debug.Log("Semua data game telah direset.");
    }
}