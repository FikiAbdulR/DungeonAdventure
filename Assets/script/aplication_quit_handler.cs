using UnityEngine;

public class aplication_quit_handler : MonoBehaviour
{
    void OnApplicationQuit()
    {
        if (!Application.isEditor)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }

}
