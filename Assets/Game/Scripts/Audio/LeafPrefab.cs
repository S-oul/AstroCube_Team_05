using FMOD;
using UnityEngine;

public class LeafPrefab : MonoBehaviour
{

    Vector3 _oldPos = Vector3.zero;
    void Update()
    {
        if(_oldPos != transform.position)
        {
            //C LA CAMÏ

            Destroy(this.gameObject, 10f);
        }
        _oldPos = transform.position;
    }
}
