using System;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Runtime.InteropServices;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class AUDIO_ProgrammerInstrument : MonoBehaviour
{
    public static AUDIO_ProgrammerInstrument Instance { get; private set; }

    [Header("Event 2D Global")]
    [Tooltip("2D Subtitles")]
    [SerializeField] private EventReference voiceLineEvent2D;

    private static EVENT_CALLBACK dialogueCallback = new EVENT_CALLBACK(DialogueEventCallback);
    
    private EventInstance currentInstance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlayVoiceLine(string key)
    {
        if (string.IsNullOrEmpty(key) || voiceLineEvent2D.IsNull) return;

        // Stop previous if still playing
        if (currentInstance.isValid())
        {
            currentInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentInstance.release();
        }

        EventDescription description = RuntimeManager.GetEventDescription(voiceLineEvent2D);
        description.createInstance(out currentInstance);

        GCHandle stringHandle = GCHandle.Alloc(key, GCHandleType.Normal);
        currentInstance.setUserData(GCHandle.ToIntPtr(stringHandle));
        currentInstance.setCallback(dialogueCallback);

        currentInstance.start();
        // PAS de release() pour garder la référence
    }

    public void Cancel()
    {
        if (currentInstance.isValid())
            currentInstance.stop(STOP_MODE.ALLOWFADEOUT);
    }
    
    public void Pause()
    {
        if (currentInstance.isValid())
            currentInstance.setPaused(true);
    }

    public void Resume()
    {
        if (currentInstance.isValid())
            currentInstance.setPaused(false);
    }

    [AOT.MonoPInvokeCallback(typeof(EVENT_CALLBACK))]
    static FMOD.RESULT DialogueEventCallback(EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
    {
        EventInstance instance = new EventInstance(instancePtr);

        IntPtr userDataPtr;
        instance.getUserData(out userDataPtr);
        GCHandle stringHandle = GCHandle.FromIntPtr(userDataPtr);
        String key = stringHandle.Target as String;

        switch (type)
        {
            case EVENT_CALLBACK_TYPE.CREATE_PROGRAMMER_SOUND:
                {
                    var parameter = (FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES));
                    FMOD.Studio.SOUND_INFO soundInfo;

                    FMOD.RESULT keyResult = RuntimeManager.StudioSystem.getSoundInfo(key, out soundInfo);
                    if (keyResult != FMOD.RESULT.OK) break;

                    FMOD.Sound dialogueSound;
                    FMOD.RESULT soundResult = RuntimeManager.CoreSystem.createSound(
                        soundInfo.name_or_data,
                        soundInfo.mode | FMOD.MODE.LOOP_OFF | FMOD.MODE.CREATECOMPRESSEDSAMPLE | FMOD.MODE.NONBLOCKING,
                        ref soundInfo.exinfo,
                        out dialogueSound
                    );

                    if (soundResult == FMOD.RESULT.OK)
                    {
                        parameter.sound = dialogueSound.handle;
                        parameter.subsoundIndex = soundInfo.subsoundindex;
                        Marshal.StructureToPtr(parameter, parameterPtr, false);
                    }
                }
                break;

            case EVENT_CALLBACK_TYPE.DESTROY_PROGRAMMER_SOUND:
                {
                    var parameter = (FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES));
                    var sound = new FMOD.Sound(parameter.sound);
                    sound.release();
                }
                break;

            case EVENT_CALLBACK_TYPE.STOPPED:
                {
                    instance.release();
                }
                break;

            case EVENT_CALLBACK_TYPE.DESTROYED:
                {
                    if (stringHandle.IsAllocated) stringHandle.Free();
                }
                break;
        }
        return FMOD.RESULT.OK;
    }
}