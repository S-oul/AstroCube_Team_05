using System.Collections;
using FMODUnity;
using UnityEngine;

public class GameActionShowSubtitle : AGameAction
{
    [SerializeField] private string _locutor;
    [SerializeField] private string _csvName;
    [SerializeField] private string _localizationID;
    [SerializeField] private float _duration;
    [SerializeField] private Color _color;
    
    [SerializeField] private EventReference _startSubtitleEvent;
    [SerializeField] private EventReference _stopSubtitleEvent;

    private bool _isFinished = true;
    
    protected override void ExecuteSpecific()
    {
        StartCoroutine(PrintSubtitle());
    }

    private IEnumerator PrintSubtitle()
    {
        _isFinished = false;
        if (!_startSubtitleEvent.IsNull) RuntimeManager.PlayOneShot(_startSubtitleEvent, transform.position);
        LocalizationManager.Instance.PrintStringFromID(_csvName, _localizationID, _locutor, _color);
        yield return new WaitForSeconds(_duration);
        LocalizationManager.Instance.ClearString();
        if (!_stopSubtitleEvent.IsNull) RuntimeManager.PlayOneShot(_stopSubtitleEvent, transform.position);
        _isFinished = true;
    }

    public override string BuildGameObjectName()
    {
        return $"SUBTITLE {_duration}s : <{_csvName}:{_localizationID}>";
    }
    
    protected override bool IsFinishedSpecific()
    {
        return _isFinished;
    }
    
}
