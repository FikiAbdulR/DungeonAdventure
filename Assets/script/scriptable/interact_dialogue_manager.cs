using TMPro;
using UnityEngine;

public class interact_dialogue_manager : MonoBehaviour
{
    public static interact_dialogue_manager Instance;

    [SerializeField] private GameObject panel;

    [SerializeField] private TextMeshProUGUI NameText;

    [SerializeField] private TextMeshProUGUI dialogueText;

    private interaction_data currentDialogue;

    private int currentIndex;

    private void Awake()
    {
        Instance = this;

        panel.SetActive(false);
    }

    public void StartDialogue(interaction_data data)
    {
        simple_player_controller.Instance.DisableMovement();

        currentDialogue = data;

        currentIndex = 0;

        NameText.text = data.title;

        panel.SetActive(true);

        ShowCurrentLine();
    }

    public void NextLine()
    {
        currentIndex++;

        if (currentIndex >= currentDialogue.lines.Length)
        {
            EndDialogue();
            return;
        }

        ShowCurrentLine();
    }

    private void ShowCurrentLine()
    {
        dialogueText.text =
            currentDialogue.lines[currentIndex];
    }

    private void EndDialogue()
    {
        panel.SetActive(false);

        ExecuteAction();

        simple_player_controller.Instance.EnableMovement();
    }

    private void ExecuteAction()
    {
        switch (currentDialogue.action)
        {
            case InteractionAction.None:
                break;

            case InteractionAction.StartQuest:

                quest_manager.Instance.StartQuest(currentDialogue.quest);

                break;
        }
    }
}