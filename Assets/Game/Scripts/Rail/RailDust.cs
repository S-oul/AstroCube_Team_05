using NaughtyAttributes;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class RailDust : MonoBehaviour
{
    public float lenght = 1;
    public bool isEnd = false;

    [SerializeField] private bool isPowered = false;

    RailDust before;
    RailDust After;

    [SerializeField] LayerMask layerMask;

    MeshRenderer _renderer;
    [SerializeField] Material _poweredMat;
    [SerializeField] Material _baseMat;

    public bool doprint = false;
    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _renderer.material = isPowered ? _poweredMat : _baseMat;

        CheckForDust();
    }

    RaycastHit hitInfo;

    public bool IsPowered { get => isPowered; set => isPowered = value; }

    [Button("CheckForDust")]
    void CheckForDust()
    {
        if (doprint) print(0);

        if (Physics.SphereCast(
            transform.position + transform.forward * transform.lossyScale.z / 2
            , lenght
            , transform.forward
            , out hitInfo
            , 3f
            , layerMask)
        ){

            if (doprint) print(1);
            After = hitInfo.transform.GetComponent<RailDust>();
            if (!After) return;
            if (doprint) print(2);

            if (IsPowered && !After.IsPowered)
            {
                if (doprint) print(3);
                After.PowerRail(true);
            }
        }
        else if (After)
        {
            After.PowerRail(false);
            After = null;
        }
    }

    public void PowerRail(bool shouldPower)
    {
        if (doprint) print(4);
        IsPowered = shouldPower;
        _renderer.material = shouldPower ? _poweredMat : _baseMat;
        CheckForDust();

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward * transform.lossyScale.z / 2, lenght);
    }


}
