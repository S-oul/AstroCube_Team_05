using System.Collections.Generic;
using UnityEngine;

public class LazerBeam
{
    Vector3 _pos, _dir;
    GameObject _lazerObj;
    LineRenderer _lazer;
    List<Vector3> _lazerIndices = new List<Vector3>();
    float _maxDistance;

    // Timer statique : partagé entre les instances
    static float _aimHitTime = 0f;
    static bool _wasHittingAimLastFrame = false;

    public LazerBeam(Vector3 pos, Vector3 dir, Material material, float maxDistance)
    {
        _lazerObj = new GameObject("Lazer Beam");
        _pos = pos;
        _dir = dir;
        _maxDistance = maxDistance;

        _lazer = _lazerObj.AddComponent<LineRenderer>();
        _lazer.startWidth = 0.1f;
        _lazer.endWidth = 0.1f;
        _lazer.material = material;
        _lazer.startColor = Color.red;
        _lazer.endColor = Color.red;

        CastRay(_pos, _dir);
    }

    public void CastRay(Vector3 pos, Vector3 dir)
    {
        _lazerIndices.Clear();
        _lazerIndices.Add(pos);

        Ray ray = new Ray(pos, dir);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _maxDistance, ~0, QueryTriggerInteraction.Ignore))
        {
            CheckHit(hit, dir);
        }
        else
        {
            _lazerIndices.Add(ray.GetPoint(_maxDistance));
            ResetAimTimer(); // rien touché
            UpdateLazer();
        }
    }

    void CheckHit(RaycastHit hitInfo, Vector3 direction)
    {
        _lazerIndices.Add(hitInfo.point); // TOUJOURS ajouter le point touché pour le rendu

        if (hitInfo.collider.CompareTag("Aim"))
        {
            _aimHitTime += Time.deltaTime;

            if (_aimHitTime >= 5f)
            {
                EventManager.TriggerPlayerWin();
                _aimHitTime = 0f; // évite de spammer
            }

            _wasHittingAimLastFrame = true;
        }
        else
        {
            ResetAimTimer();

            if (hitInfo.collider.CompareTag("Mirror"))
            {
                Vector3 pos = hitInfo.point;
                Vector3 dir = Vector3.Reflect(direction, hitInfo.normal);
                CastRay(pos, dir); // Rebond continue le faisceau
                return; // ne pas appeler UpdateLazer maintenant
            }
        }

        UpdateLazer(); // Appel dans tous les cas SAUF si on fait un rebond (appel récursif)
    }

    void ResetAimTimer()
    {
        if (_wasHittingAimLastFrame)
        {
            _aimHitTime = 0f;
            _wasHittingAimLastFrame = false;
        }
    }

    void UpdateLazer()
    {
        _lazer.positionCount = _lazerIndices.Count;

        for (int i = 0; i < _lazerIndices.Count; i++)
        {
            _lazer.SetPosition(i, _lazerIndices[i]);
        }
    }
}
