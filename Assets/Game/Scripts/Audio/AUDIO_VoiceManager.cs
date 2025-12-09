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

    [Tooltip("Replay onEnable")]
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
                    var charVoice = entry.targetObject.GetComponent<AUDIO_CharacterVoice>();
                    if (charVoice != null)
                        charVoice.PlayVoice(entry.VO_ID);
                    
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