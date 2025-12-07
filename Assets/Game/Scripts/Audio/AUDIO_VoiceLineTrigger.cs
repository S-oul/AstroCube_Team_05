using UnityEngine;

public class AUDIO_VoiceLineTrigger : MonoBehaviour
{
    [Tooltip("ID of the voice line")]
    public string voiceLineKey;

    [Tooltip("Replay onEnable")]
    public bool playOnEnable = true;

    private void Start()
    {
        if (!playOnEnable)
        {
            PlaySound();
        }
    }

    private void OnEnable()
    {
        if (playOnEnable)
        {
            PlaySound();
        }
    }

    private void PlaySound()
    {
        if (!string.IsNullOrEmpty(voiceLineKey))
        {
            var charVoice = GetComponent<AUDIO_CharacterVoice>();
            if (charVoice != null)
                charVoice.PlayVoice(voiceLineKey);
        }
    }
}