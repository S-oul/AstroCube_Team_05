using MoreMountains.Feedbacks;
using UnityEngine;

public class RailObject : MonoBehaviour
{
    [SerializeField] RailConnector _railImOn;

    float _railLenght;
    [SerializeField] float _objRailPos = 0;

    private void Start()
    {
        _railImOn._GetRailLenght();
        transform.position = _railImOn._GetObjPos(_objRailPos);
    }

    private void Update()
    {
        Vector3 forward = _railImOn._GetObjPos(_objRailPos + 0.1f);//All can be doone in one func for perf (forxward backward)
        Vector3 backward = _railImOn._GetObjPos(_objRailPos - 0.1f);//All can be doone in one func for perf (forxward backward)

        float difForward = forward.y - transform.position.y;
        float difBackward = backward.y - transform.position.y;

        print(difForward);
        if (difForward < 0f)
        {
            transform.position = forward;
            _objRailPos += 0.1f;
        }else if(difBackward < 0f)
        {
            transform.position = backward;
            _objRailPos -= 0.1f;
        }
        else
        {
            transform.position = _railImOn._GetObjPos(_objRailPos);
        }
    }

}
