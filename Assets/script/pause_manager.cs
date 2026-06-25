using UnityEngine;

public class pause_manager : MonoBehaviour
{
    public static pause_manager Instance;

    public GameObject pauseMenuUI;
    public bool IsPaused { get; private set; } = false;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }

    public void TogglePause()
    {
        if (IsPaused) ResumeGame();
        else PauseGame();
    }
}