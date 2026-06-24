using UnityEngine;
using UnityEngine.Events;

public class quest_location_trigger : MonoBehaviour
{
    [Header("Quest Target")]
    [SerializeField] private string locationID; // harus sama dengan typeID di quest objective

    [Header("Settings")]
    [SerializeField] private bool disableAfterTrigger = true;
    [SerializeField] private bool destroyAfterTrigger = false;

    [Header("On Reached")]
    [SerializeField] private UnityEvent onLocationReached;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;

        hasTriggered = true;

        quest_manager.Instance.AddProgress(QuestObjectiveType.ReachLocation, locationID);
        Debug.Log($"Location reached: {locationID}");

        onLocationReached?.Invoke();

        if (destroyAfterTrigger)
        {
            Destroy(gameObject);
        }
        else if (disableAfterTrigger)
        {
            GetComponent<Collider>().enabled = false;
        }
    }
}