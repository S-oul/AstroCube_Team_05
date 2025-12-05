using System;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Runtime.InteropServices;

public class AUDIO_ProgrammerInstrument : MonoBehaviour
{
    public static AUDIO_ProgrammerInstrument Instance { get; private set; }

    [Header("Configuration")]
    [SerializeField] private EventReference voiceLineEvent;

    private EventInstance dialogueInstance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlayVoiceLine(string key, Vector3 position = default)
    {
        if (string.IsNullOrEmpty(key)) return;

        //UnityEngine.Debug.Log($"PlayVoiceLine called for key: '{key}' at position: {position}");

        dialogueInstance = RuntimeManager.CreateInstance(voiceLineEvent);
        
        if (position != default)
        {
            dialogueInstance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        }

        GCHandle stringHandle = GCHandle.Alloc(key, GCHandleType.Pinned);
        dialogueInstance.setUserData(GCHandle.ToIntPtr(stringHandle));

        dialogueInstance.setCallback(new EVENT_CALLBACK(DialogueEventCallback));

        dialogueInstance.start();
        dialogueInstance.release();
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
                    //UnityEngine.Debug.Log($"FMOD Callback: Creating sound for key '{key}'");
                    var parameter = (FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES));
                    FMOD.Studio.SOUND_INFO soundInfo;

                    FMOD.RESULT keyResult = RuntimeManager.StudioSystem.getSoundInfo(key, out soundInfo);

                    if (keyResult != FMOD.RESULT.OK)
                    {
                        //UnityEngine.Debug.LogWarning($"FMOD: Clé '{key}' introuvable dans l'Audio Table. Result: {keyResult}");
                        break;
                    }

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
                        //UnityEngine.Debug.Log($"FMOD Callback: Sound created successfully for key '{key}'");
                    }
                    else
                    {
                        //UnityEngine.Debug.LogError($"FMOD Callback: Failed to create sound for key '{key}'. Result: {soundResult}");
                    }
                }
                break;

            case EVENT_CALLBACK_TYPE.DESTROY_PROGRAMMER_SOUND:
                {
                    //UnityEngine.Debug.Log($"FMOD Callback: Destroying sound for key '{key}'");
                    var parameter = (FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES));
                    var sound = new FMOD.Sound(parameter.sound);
                    sound.release();
                }
                break;

            case EVENT_CALLBACK_TYPE.DESTROYED:
                {
                    stringHandle.Free();
                }
                break;
        }
        return FMOD.RESULT.OK;
    }
}