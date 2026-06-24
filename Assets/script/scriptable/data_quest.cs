using System;
using System.Collections.Generic;
using UnityEngine;

public enum QuestObjectiveType
{
    Kill,
    Collect,
    Talk,
    ReachValue,
    ReachLocation
}

public enum QuestStatus
{
    NotStarted,
    InProgress,
    Completed
}

[Serializable]
public class QuestObjective
{
    public QuestObjectiveType type;
    [Header("Target ID")]
    public string typeID;
    [Header("Amount Needed")]
    public int requiredAmount = 1;
}

[CreateAssetMenu(
    fileName = "Quest",
    menuName = "RPG/Quest"
)]
public class data_quest : ScriptableObject
{
    [Header("Info")]
    public string questID;
    public string questName;
    [TextArea(3, 5)]
    public string description;

    [Header("Objectives")]
    public List<QuestObjective> objectives = new List<QuestObjective>();

    [Header("Quest Settings")]
    public bool retroactiveProgress = true;

    [Header("Reward")]
    public int rewardGold;
    public int rewardExp;
}