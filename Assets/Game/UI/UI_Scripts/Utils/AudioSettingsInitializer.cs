using UnityEngine;
using FMOD.Studio;

public class AudioSettingsInitializer : MonoBehaviour
{
    [SerializeField] private CustomisedSettings customisedSettings;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (customisedSettings == null)
        {
            Debug.LogWarning("[AudioInit] CustomisedSettings not assigned, trying to load from Resources...");
            customisedSettings = Resources.Load<CustomisedSettings>("CustomisedSettings");
        }

        ApplyVolumes();
    }

    private void ApplyVolumes()
    {
        float masterVol = PlayerPrefs.GetFloat("MasterVolume", customisedSettings.customVolume);
        float musicVol = PlayerPrefs.GetFloat("MusicVolume", customisedSettings.customMusicVolume);
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", customisedSettings.customSoundEffectsVolume);
        float voiceVol = PlayerPrefs.GetFloat("VoiceVolume", customisedSettings.customVoiceVolume);

        FMOD.Studio.VCA masterVCA = FMODUnity.RuntimeManager.GetVCA("vca:/Master");
        FMOD.Studio.VCA musicVCA = FMODUnity.RuntimeManager.GetVCA("vca:/Music");
        FMOD.Studio.VCA sfxVCA = FMODUnity.RuntimeManager.GetVCA("vca:/Effect");
        FMOD.Studio.VCA voiceVCA = FMODUnity.RuntimeManager.GetVCA("vca:/VO");

        masterVCA.setVolume(masterVol);
        musicVCA.setVolume(musicVol);
        sfxVCA.setVolume(sfxVol);
        voiceVCA.setVolume(voiceVol);

        Debug.Log($"[AudioInit] Volumes restored at scene start  Master:{masterVol:F2}, Music:{musicVol:F2}, SFX:{sfxVol:F2}, Voice:{voiceVol:F2}");
    }
}
