using UnityEngine;

public class generic_hint : MonoBehaviour
{
    [SerializeField] private CanvasGroup hintGroup;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField]
    private KeyCode[] dismissKeys = {
        KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D
    };

    private bool hasTriggered = false;
    private float fadeTimer = 0f;

    private void Start()
    {
        hintGroup.alpha = 1f;
        hintGroup.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!hasTriggered && AnyDismissKeyPressed())
        {
            hasTriggered = true;
            fadeTimer = 0f;
        }

        if (hasTriggered)
        {
            fadeTimer += Time.deltaTime;
            float t = fadeTimer / fadeDuration;
            hintGroup.alpha = Mathf.Lerp(1f, 0f, t);

            if (t >= 1f)
            {
                hintGroup.gameObject.SetActive(false);
                enabled = false;
            }
        }
    }

    private bool AnyDismissKeyPressed()
    {
        foreach (var key in dismissKeys)
        {
            if (Input.GetKeyDown(key))
                return true;
        }
        return false;
    }
}