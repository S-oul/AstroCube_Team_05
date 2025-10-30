using JetBrains.Annotations;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RailMap : MonoBehaviour
{
    MapCell[] XPlus = new MapCell[9];
    MapCell[] YPlus = new MapCell[9];
    MapCell[] ZPlus = new MapCell[9];

    MapCell[] XMoins = new MapCell[9];
    MapCell[] YMoins = new MapCell[9];
    MapCell[] ZMoins = new MapCell[9];

    [Button]
    public void CreateMaps()
    {

        XPlus = new MapCell[9];
        YPlus = new MapCell[9];
        ZPlus = new MapCell[9];

        XMoins = new MapCell[9];
        YMoins = new MapCell[9];
        ZMoins = new MapCell[9];

        var groscon = transform.parent.GetComponentsInChildren<MapCell>();

        foreach (var mapCell in groscon)
        {

            var VRight = mapCell.transform.parent.right;
            var CubePos = mapCell.transform.parent.parent.localPosition;
            CubePos += Vector3.one;

            //YMoins 
            if (VRight.y > 0.1f)
            {
                mapCell.LocalPosOnFace = new Vector2(CubePos.x, CubePos.z);
                YMoins[(int)(CubePos.z * 3 + CubePos.x)] = mapCell;
                continue;
            }
            //YPlus;
            if (VRight.y < -0.5f)
            {
                mapCell.LocalPosOnFace = new Vector2(CubePos.x, CubePos.z);
                YPlus[(int)(CubePos.z * 3 + CubePos.x)] = mapCell;
                continue;
            }


            if (VRight.x > 0.1f)
            {
                mapCell.LocalPosOnFace = new Vector2(CubePos.z, CubePos.y);
                XMoins[(int)(CubePos.y * 3 + CubePos.z)] = mapCell;
                continue;
            }
            if (VRight.x < -0.1f)
            {
                mapCell.LocalPosOnFace = new Vector2(CubePos.z, CubePos.y);
                XPlus[(int)(CubePos.y * 3 + CubePos.z)] = mapCell;
                continue;
            }


            if (VRight.z > 0.1f)
            {
                mapCell.LocalPosOnFace = new Vector2(CubePos.x, CubePos.y);
                ZMoins[(int)(CubePos.z * 3 + CubePos.y)] = mapCell;
                continue;
            }
            if (VRight.z < -0.1f)
            {
                mapCell.LocalPosOnFace = new Vector2(CubePos.x, CubePos.y);
                ZPlus[(int)(CubePos.z * 3 + CubePos.x)] = mapCell;
                continue;
            }
        }


        for (int i = 0; i < 9; i++)
        {
            if (XPlus[i]) print("XPlus " + XPlus[i].LocalPosOnFace);
            if (YPlus[i]) print("YPlus " + YPlus[i].LocalPosOnFace);
            if (ZPlus[i]) print("ZPlus " + ZPlus[i].LocalPosOnFace);

            if (XMoins[i]) print("XMoins " + XMoins[i].LocalPosOnFace);
            if (YMoins[i]) print("YMoins " + YMoins[i].LocalPosOnFace);
            if (ZMoins[i]) print("ZMoins " + ZMoins[i].LocalPosOnFace);
        }

        return;
    }

    private void OnDrawGizmos()
    {

    }

}
