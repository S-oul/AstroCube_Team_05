using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioEventDatabase _database;

    private void Awake()
    {
        if (AudioManager.Instance)
        {
            DestroyImmediate(this);
            return;
        }
        else
        {
            DontDestroyOnLoad(this);
            Instance = this;
        } 

    }

    public void Play2D(AudioEventID id)
    {
        var def = _database.GetSoundFromID(id);
        def?.WwiseEvent?.Post(Instance.gameObject);
        Debug.Log($"Playing 2D sound: {def?.WwiseEvent?.Name}");
    }

    public void Play3D(AudioEventID id, GameObject source)
    {
        var def = _database.GetSoundFromID(id);
        def?.WwiseEvent?.Post(source);
    }

    public void PlayOneShot(AudioEventID id, GameObject source = null)
    {
        var def = _database.GetSoundFromID(id);
        if (def == null) return;
        def.WwiseEvent?.Post(def.Is3D && source != null ? source : gameObject);
    }
    public void StopAllFrom(GameObject source)
    {
        AkUnitySoundEngine.StopAll(source);
    }

    public void StopAll2D()
    {
        AkUnitySoundEngine.StopAll(gameObject);
    }

    public void StopEvent(AudioEventID id, GameObject source)
    {
        var def = _database.GetSoundFromID(id);
        if (def == null || def.WwiseEvent == null) return;

        def.WwiseEvent.Stop(source);
    }


    private void OnEnable()
    {
        EventManager.OnPlayerFootSteps += OnPlayerFootSteps;
        EventManager.OnStartCubeRotation += OnStartCubeRotation;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerFootSteps -= OnPlayerFootSteps;
        EventManager.OnStartCubeRotation -= OnStartCubeRotation;

    }

    private void OnPlayerFootSteps(GroundTypePlayerIsWalkingOn _groundTypePlayerIsWalkingOn)
    {
        switch (_groundTypePlayerIsWalkingOn)
        {
            case GroundTypePlayerIsWalkingOn.Default:
                Play2D(AudioEventID.MC_FT);
                break;
            case GroundTypePlayerIsWalkingOn.Grass: //example of implémentation
                Play2D(AudioEventID.SFX_CubeRotation); //Placeholder Must change in future
                break;
        }
    }


    private void OnStartCubeRotation()
    {
        Play2D(AudioEventID.SFX_CubeRotation);
    }
}
