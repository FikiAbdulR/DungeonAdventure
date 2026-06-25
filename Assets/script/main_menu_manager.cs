using UnityEngine;
using UnityEngine.SceneManagement;

public class main_menu_manager : MonoBehaviour
{
    public void ResetData(string Scene)
    {
        Time.timeScale = 1f; // tambahkan ini
        game_reset_manager.Instance.ResetAndGoToMainMenu(Scene);
    }

    public void OpenScene(string Scene)
    {
        Time.timeScale = 1f; // tambahkan ini
        SceneManager.LoadScene(Scene);
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}