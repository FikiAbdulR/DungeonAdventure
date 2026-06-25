using UnityEngine;

public class app_settings_manager : MonoBehaviour
{
    public static app_settings_manager Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        ApplyFrameRateSettings();
    }

    void ApplyFrameRateSettings()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 24;
    }
}