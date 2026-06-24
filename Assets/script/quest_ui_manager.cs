using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class quest_ui_manager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject questPanel;
    [SerializeField] private TMP_Text questTitleText;
    [SerializeField] private TMP_Text questObjectivesText;

    private string trackedQuestID;

    private void OnEnable()
    {
        quest_manager.OnQuestStarted += HandleQuestStarted;
        quest_manager.OnQuestCompleted += HandleQuestCompleted;
        quest_manager.OnObjectiveProgress += HandleObjectiveProgress;
    }

    private void OnDisable()
    {
        quest_manager.OnQuestStarted -= HandleQuestStarted;
        quest_manager.OnQuestCompleted -= HandleQuestCompleted;
        quest_manager.OnObjectiveProgress -= HandleObjectiveProgress;
    }

    private void Start()
    {
        if (questPanel != null)
            questPanel.SetActive(false);

        RefreshFromExistingQuest();
    }

    private void RefreshFromExistingQuest()
    {
        if (quest_manager.Instance == null) return;

        var allActive = quest_manager.Instance.GetAllActiveQuests();
        if (allActive.Count > 0)
        {
            trackedQuestID = allActive[0].quest.questID;
            RefreshUI(allActive[0].quest);
        }
    }

    private void HandleQuestStarted(data_quest quest)
    {
        trackedQuestID = quest.questID;
        RefreshUI(quest);
    }

    private void HandleObjectiveProgress(data_quest quest, quest_objective_progress objective)
    {
        if (quest.questID != trackedQuestID) return;
        RefreshUI(quest);
    }

    private void HandleQuestCompleted(data_quest quest)
    {
        if (quest.questID != trackedQuestID) return;

        if (questPanel != null)
            questPanel.SetActive(false);

        trackedQuestID = null;
    }

    private void RefreshUI(data_quest quest)
    {
        if (questPanel != null)
            questPanel.SetActive(true);

        if (questTitleText != null)
            questTitleText.text = quest.questName;

        if (questObjectivesText != null)
            questObjectivesText.text = FormatDescription(quest);
    }

    private string FormatDescription(data_quest quest)
    {
        var runtime = quest_manager.Instance.GetActiveQuest(quest.questID);
        if (runtime == null || runtime.objectives.Count == 0)
            return quest.description;

        var obj = runtime.objectives[0];

        try
        {
            return string.Format(quest.description, obj.currentAmount, obj.requiredAmount);
        }
        catch (System.FormatException)
        {
            return quest.description;
        }
    }
}