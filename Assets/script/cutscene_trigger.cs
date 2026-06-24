using UnityEngine;
using UnityEngine.Playables;

public class cutscene_trigger : MonoBehaviour
{
    public string cutsceneID;

    public PlayableDirector director;
    public bool playOnlyOnce = true;
    private bool hasPlayed = false;

    private void Start()
    {
        if (data_world_state_manager.Instance.CompletedCutscenes.Contains(cutsceneID))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (playOnlyOnce && hasPlayed)
            return;

        director.Play();
        hasPlayed = true;
        data_world_state_manager.Instance.CompletedCutscenes.Add(cutsceneID);
    }
}
