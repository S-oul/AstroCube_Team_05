using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioEventDatabase _database;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Play2D(AudioEventID id)
    {
        var def = _database.GetSoundFromID(id);
        def?.WwiseEvent?.Post(gameObject);
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
        EventManager.OnButtonPressed += () => Play2D(AudioEventID.SFX_Button);
    }

    private void OnDisable()
    {
        EventManager.OnButtonPressed -= () => Play2D(AudioEventID.SFX_Button);
        EventManager.OnButtonPressed -= () => Play2D(AudioEventID.SFX_Button);
    }
}
