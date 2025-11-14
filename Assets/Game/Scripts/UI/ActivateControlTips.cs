using UnityEngine;

public class ActivateControlTips : MonoBehaviour
{
    int _rotationCount = 0;
    [SerializeField] GameObject _restartControlTipPopUp;
    [SerializeField] int _numOfRotationsBeforPopUp;

    private void OnEnable()
    {
        EventManager.OnEndCubeRotation += Add1ToRotationCount;
     //   EventManager.OnPlayerReset += ResetRotationCound;
    }

    void Add1ToRotationCount()
    {
        _rotationCount++;

        if (_rotationCount >= _numOfRotationsBeforPopUp &&
            _restartControlTipPopUp.activeSelf == false) 
            _restartControlTipPopUp.SetActive( true );
    }

    void ResetRotationCound()
    {
        _rotationCount = 0;
        if (_restartControlTipPopUp.activeSelf)
        {
            _restartControlTipPopUp.SetActive ( false );
        }
    }
}
