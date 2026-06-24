using System;
using System.Collections.Generic;

[Serializable]
public class quest_objective_progress
{
    public QuestObjectiveType type;
    public string typeID;
    public int requiredAmount;
    public int currentAmount;

    public int baselineKillCount;
}

[Serializable]
public class quest_runtime_data
{
    public data_quest quest;
    public QuestStatus status;
    public List<quest_objective_progress> objectives;

    public quest_runtime_data(data_quest sourceQuest)
    {
        quest = sourceQuest;
        status = QuestStatus.InProgress;
        objectives = new List<quest_objective_progress>();

        foreach (var obj in sourceQuest.objectives)
        {
            objectives.Add(new quest_objective_progress
            {
                type = obj.type,
                typeID = obj.typeID,
                requiredAmount = obj.requiredAmount,
                currentAmount = 0,
                baselineKillCount = 0
            }
            );
        }
    }
}