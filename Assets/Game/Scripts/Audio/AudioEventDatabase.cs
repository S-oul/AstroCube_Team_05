using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/EventDatabase")]
public class AudioEventDatabase : ScriptableObject
{
    [System.Serializable]
    public class AudioEventDefinition
    {
        public AudioEventID ID;
        public AK.Wwise.Event WwiseEvent;
        public bool Is3D = false;
    }


    public List<AudioEventDefinition> Events;

    private Dictionary<AudioEventID, AudioEventDefinition> _cache;

    public AudioEventDefinition GetSoundFromID(AudioEventID id)
    {
        if (_cache == null)
            _cache = Events.ToDictionary(e => e.ID);
        return _cache.TryGetValue(id, out var def) ? def : null;
    }
}
