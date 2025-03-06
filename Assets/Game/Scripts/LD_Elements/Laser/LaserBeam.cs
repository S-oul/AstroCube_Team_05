using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam
{
    Vector3 _pos, _dir;
    GameObject _laserObj;
    LineRenderer _laser;
    List<Vector3> _laserIndices = new List<Vector3>();

    public LaserBeam(Vector3 pos, Vector3 dir, Material material)
    {
        this._laser = new LineRenderer();
        this._laserObj = new GameObject();
        this._laserObj.name = "Laser Beam";
        this._pos = pos;
        this._dir = dir;

        this._laser = this._laserObj.AddComponent(typeof(LineRenderer)) as LineRenderer;
        this._laser.startWidth = 0.1f;
        this._laser.endWidth = 0.1f;
        this._laser.material = material;
        this._laser.startColor = Color.red;
        this._laser.endColor = Color.red;

        CastRay(pos, dir, _laser);
    }

    void CastRay(Vector3 pos, Vector3 dir, LineRenderer laser)
    {
        _laserIndices.Add(pos);

        Ray ray = new Ray(pos, dir);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 30, 1))
        {
           CheckHit(hit, dir, laser);
        }
        else
        {
            _laserIndices.Add(ray.GetPoint(30));
            UpdateLaser();
        }
    }

    void UpdateLaser()
    {
        int count = 0;
        _laser.positionCount = _laserIndices.Count;

        foreach (Vector3 idx in _laserIndices)
        {
            _laser.SetPosition(count, idx);
            count++;
        }
    }

    void CheckHit(RaycastHit hitInfo, Vector3 direction, LineRenderer laser)
    {
        if(hitInfo.collider.tag == "Aim")
        {
            EventManager.TriggerPlayerWin();
        }
       

        if (hitInfo.collider.tag == "Mirror")
        {
            Vector3 pos = hitInfo.point;
            Vector3 dir = Vector3.Reflect(direction, hitInfo.normal);
            CastRay(pos, dir, laser);
        }
        else
        {
            _laserIndices.Add(hitInfo.point);
            UpdateLaser();
        }
    }
}
