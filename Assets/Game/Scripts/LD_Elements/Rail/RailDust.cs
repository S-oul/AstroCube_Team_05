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

    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _renderer.material = isPowered ? _poweredMat : _baseMat;

        CheckForDust();
    }


    public bool IsPowered { get => isPowered; set => isPowered = value; }

    [Button("CheckForDust")]
    void CheckForDust()
    {
        var cols = Physics.OverlapSphere(transform.position + transform.forward * transform.lossyScale.z / 2, lenght,(int)layerMask);
        var dust = cols.First(c => c.transform != this.transform);
        if (dust)
        {

            After = dust.transform.GetComponent<RailDust>();
            if (!After) return;

            if (IsPowered && !After.IsPowered)
            {
                After.PowerRail(true);
                After.CheckForDust();
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
        IsPowered = shouldPower;
        _renderer.material = shouldPower ? _poweredMat : _baseMat;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward * transform.lossyScale.z / 2, lenght);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position + transform.forward * transform.lossyScale.z / (2 + .01f), transform.forward * lenght);

    }


}
