using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ConditionalInteraction
{
    [Header("Condition")]
    public data_quest requiredQuest;
    public QuestStatus requiredStatus;

    [Header("Result")]
    public interaction_data interaction;
}

public class interactable_object : MonoBehaviour
{
    [Header("Conditional Dialogues")]
    [Tooltip("Dicek dari atas ke bawah, taruh kondisi paling spesifik di atas")]
    [SerializeField] private List<ConditionalInteraction> conditionalInteractions = new();

    [Header("Default / Fallback")]
    [SerializeField] private interaction_data defaultInteraction;

    [Header("UI")]
    [SerializeField] private GameObject interactSign;

    [Header("On Dialogue End (One Time)")]
    [SerializeField] private string interactionID;
    [SerializeField] private UnityEvent onDialogueEnd;

    private bool playerInRange;
    private bool waitingForDialogueEnd;

    private void Start()
    {
        if (interactSign != null)
            interactSign.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Space))
        {
            Interact();
        }
    }

    private interaction_data GetCurrentInteraction()
    {
        foreach (var entry in conditionalInteractions)
        {
            QuestStatus currentStatus = entry.requiredQuest != null
                ? quest_manager.Instance.GetStatus(entry.requiredQuest.questID)
                : QuestStatus.NotStarted;

            if (entry.requiredQuest == null ||
                currentStatus == entry.requiredStatus)
            {
                return entry.interaction;
            }
        }

        return defaultInteraction;
    }

    private void Interact()
    {
        interaction_data data = GetCurrentInteraction();
        if (data == null) return;

        simple_player_controller.Instance.RotateToFace(transform);

        interact_dialogue_manager.Instance.StartDialogue(data);

        HandleInteractionAction(data);

        onDialogueEnd?.Invoke();
    }

    private void HandleInteractionAction(interaction_data data)
    {
        switch (data.action)
        {
            case InteractionAction.StartQuest:

                if (data.quest != null)
                    quest_manager.Instance.StartQuest(data.quest);

                break;

            case InteractionAction.CompleteQuest:

                if (data.quest != null)
                    quest_manager.Instance.CompleteQuest(data.quest.questID);

                break;

            case InteractionAction.None:
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = true;

        if (interactSign != null)
            interactSign.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = false;

        if (interactSign != null)
            interactSign.SetActive(false);
    }
}