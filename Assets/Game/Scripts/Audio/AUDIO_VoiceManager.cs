using UnityEngine;
using System.Collections.Generic;

public class AUDIO_VoiceManager : MonoBehaviour
{
    [System.Serializable]
    public struct DialogueEntry
    {
        public GameObject targetObject;
        public string VO_ID;
        [HideInInspector] public bool hasPlayed;
    }

    public List<DialogueEntry> dialogues;

    [Tooltip("Si vrai, le son pourra être rejoué si l'objet est désactivé puis réactivé.")]
    public bool replayOnReenable = true;

    void Update()
    {
        for (int i = 0; i < dialogues.Count; i++)
        {
            var entry = dialogues[i];

            if (entry.targetObject != null)
            {
                bool isActive = entry.targetObject.activeInHierarchy;

                if (isActive && !entry.hasPlayed)
                {
                    if (AUDIO_ProgrammerInstrument.Instance != null)
                    {
                        AUDIO_ProgrammerInstrument.Instance.PlayVoiceLine(entry.VO_ID);
                    }
                    
                    entry.hasPlayed = true;
                }
                else if (!isActive && replayOnReenable && entry.hasPlayed)
                {
                    entry.hasPlayed = false;
                }
            }

            dialogues[i] = entry;
        }
    }
}