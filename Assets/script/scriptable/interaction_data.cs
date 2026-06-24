using UnityEngine;
public enum InteractionType
{
    Talk,
    Inspect
}
public enum InteractionAction
{
    None,
    StartQuest,
    CompleteQuest
}

[CreateAssetMenu(
    fileName = "Interaction",
    menuName = "RPG/Interaction"
)]
public class interaction_data : ScriptableObject
{
    public string title;

    public InteractionType interactionType;

    [TextArea(3, 5)]
    public string[] lines;

    [Header("Action")]
    public InteractionAction action;

    public data_quest quest;
}
