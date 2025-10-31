using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RailDetector : MonoBehaviour
{

    [SerializeField] float lenght = 1;
    [SerializeField] LayerMask layerMask;


    public RailGroup groupIsIn;
    public bool imEnd =true;

    [SerializeField] List<Transform> railPos = new List<Transform>();
    public List<Transform> RailPos { get => railPos; }


    int baseLayer;
    private void Start()
    {
        baseLayer = gameObject.layer;
    }
    void LaunchDelay()
    {
        StartCoroutine(DelayCheck());
    }
    IEnumerator DelayCheck()
    {
        yield return new WaitForSeconds(.4f);
        CheckForRails();
    }


    RailDetector oldRail;
    [Button("CheckForRails")]
    void CheckForRails()
    {
        gameObject.layer = 2;
        var cols = Physics.OverlapSphere(transform.position, lenght, (int)layerMask);
        gameObject.layer = baseLayer;

        var result = cols.FirstOrDefault(c => c != this);
        if (!result)
        {
            groupIsIn?.RemoveFromGroup(this);
            oldRail?.groupIsIn?.RemoveFromGroup(this);
            oldRail = null;

            groupIsIn = new RailGroup(this);
            
            return;
        }

        RailDetector otherRail = result.GetComponent<RailDetector>();
        oldRail = otherRail;
        if (otherRail.groupIsIn == groupIsIn)
        {
            return;
        }

        imEnd = false;
        otherRail.imEnd = false;
        //I Do not have group but other Have
        if (otherRail.groupIsIn.usable && !groupIsIn.usable)
        {
            otherRail.groupIsIn.AddToGroup(this);
        }

        //I Do have group but other don't have
        if (!otherRail.groupIsIn.usable && groupIsIn.usable)
        {
            groupIsIn.AddToGroup(otherRail);
        }

        //no one has a group so create one
        if (!otherRail.groupIsIn.usable && !groupIsIn.usable)
        {
            //this auto notify Manager and self and other self;
            new RailGroup(this, otherRail);
        }

        //Both have groupe so merge
        if (otherRail.groupIsIn.usable && groupIsIn.usable)
        {
            if (otherRail.groupIsIn.detectors.Count >= groupIsIn.detectors.Count)
            {
                otherRail.groupIsIn.MergeGroupe(ref groupIsIn);
            }
            else
            {
                groupIsIn.MergeGroupe(ref otherRail.groupIsIn);
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lenght);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, railPos[0].position);

    }
}
