using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class quest_manager : MonoBehaviour
{
    public static quest_manager Instance;

    private Dictionary<string, quest_runtime_data> activeQuests = new Dictionary<string, quest_runtime_data>();
    private HashSet<string> completedQuests = new HashSet<string>();

    public static event Action<data_quest> OnQuestStarted;
    public static event Action<data_quest> OnQuestCompleted;
    public static event Action<data_quest, quest_objective_progress> OnObjectiveProgress;
    public List<quest_runtime_data> GetAllActiveQuests()
    {
        return activeQuests.Values.ToList();
    }

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

    public void StartQuest(data_quest quest)
    {
        if (quest == null) return;
        if (activeQuests.ContainsKey(quest.questID)) return;
        if (completedQuests.Contains(quest.questID)) return;

        var runtimeQuest = new quest_runtime_data(quest);

        if (quest.retroactiveProgress)
        {
            foreach (var obj in runtimeQuest.objectives)
            {
                switch (obj.type)
                {
                    case QuestObjectiveType.Kill:

                        obj.currentAmount = Mathf.Min(
                            player_statistics.Instance.GetKillCount(obj.typeID),
                            obj.requiredAmount
                        );

                        Debug.Log(
                            $"Retroactive Progress: {obj.typeID} = {obj.currentAmount}/{obj.requiredAmount}"
                        );

                        break;
                }
            }
        }

        activeQuests.Add(
            quest.questID,
            runtimeQuest
        );

        OnQuestStarted?.Invoke(quest);

        CheckQuestCompletion(runtimeQuest);

        Debug.Log("Quest Started: " + quest.questName);
    }

    public void AddProgress(QuestObjectiveType type, string targetID, int amount = 1)
    {
        foreach (var quest in activeQuests.Values.ToList())
        {
            bool changed = false;

            foreach (var obj in quest.objectives)
            {
                if (obj.type != type) continue;
                if (obj.typeID != targetID) continue;
                if (obj.currentAmount >= obj.requiredAmount) continue;

                obj.currentAmount = Mathf.Min(obj.currentAmount + amount, obj.requiredAmount);
                changed = true;

                Debug.Log($"{quest.quest.questName} : {obj.currentAmount}/{obj.requiredAmount}");
                OnObjectiveProgress?.Invoke(quest.quest, obj);
            }

            if (changed)
                CheckQuestCompletion(quest);
        }
    }

    private void CheckQuestCompletion(quest_runtime_data quest)
    {
        bool complete = quest.objectives.All(o => o.currentAmount >= o.requiredAmount);

        if (complete)
        {
            Debug.Log("QUEST COMPLETE TRIGGERED: " + quest.quest.questName);
            CompleteQuest(quest.quest.questID);
        }
    }

    public void CompleteQuest(string questID)
    {
        if (!activeQuests.TryGetValue(questID, out var quest)) return;

        quest.status = QuestStatus.Completed;
        completedQuests.Add(questID);
        activeQuests.Remove(questID);

        OnQuestCompleted?.Invoke(quest.quest);
        Debug.Log("Quest Completed: " + quest.quest.questName);
    }

    public bool HasQuest(string questID) => activeQuests.ContainsKey(questID);

    public bool IsQuestCompleted(string questID) => completedQuests.Contains(questID);

    public QuestStatus GetStatus(string questID)
    {
        if (completedQuests.Contains(questID)) return QuestStatus.Completed;
        if (activeQuests.ContainsKey(questID)) return QuestStatus.InProgress;
        return QuestStatus.NotStarted;
    }

    public quest_runtime_data GetActiveQuest(string questID)
    {
        return activeQuests.TryGetValue(questID, out var quest) ? quest : null;
    }

    public void ResetData()
    {
        activeQuests.Clear();
        completedQuests.Clear();
        Debug.Log("Quest Manager: data direset");
    }
}
