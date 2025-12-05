using UnityEngine;

public class AUDIO_VoiceLineTrigger : MonoBehaviour
{
    [Tooltip("La clé (ID) telle qu'elle est écrite dans la FMOD Audio Table")]
    public string voiceLineKey;

    [Tooltip("Si coché, le son se jouera à chaque fois que l'objet est activé. Sinon, seulement au lancement du jeu.")]
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
            if (AUDIO_ProgrammerInstrument.Instance != null)
            {
                AUDIO_ProgrammerInstrument.Instance.PlayVoiceLine(voiceLineKey);
            }
            else
            {
                Debug.LogWarning("AUDIO_ProgrammerInstrument introuvable ! Vérifiez qu'il y a bien un GameObject 'AudioManager' avec le script dans la scène.");
            }
        }
    }
}