using NaughtyAttributes;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class RailDust : MonoBehaviour
{
    public float lenght = 1;
    public bool isEnd = false;

    RailDust before;
    RailDust After;

    [SerializeField] LayerMask layerMask;

    RailPart _myRailPart;

    public RailPart MyRailPart { get => _myRailPart; set => _myRailPart = value; }

    void Start()
    {
        MyRailPart = transform.parent.GetComponent<RailPart>();
        CheckForDust();
    }



    [Button("CheckForDust")]
    void CheckForDust()
    {
        var cols = Physics.OverlapSphere(transform.position + transform.forward * transform.lossyScale.z / 2, lenght, (int)layerMask);
        var dust = cols.First(c => c.transform != this.transform);
        if (dust)
        {

            After = dust.transform.GetComponent<RailDust>();
            if (!After) return;

            //MyRailPart.GetLinePos();

        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward * transform.lossyScale.z / 2, lenght);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position + transform.forward * transform.lossyScale.z / (2 + .01f), transform.forward * lenght);

    }


}
