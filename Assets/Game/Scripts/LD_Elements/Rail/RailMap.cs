using AmplifyShaderEditor;
using JetBrains.Annotations;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RailMap : MonoBehaviour
{
    Face XPlus;
    Face YPlus;
    Face ZPlus;
    Face XMoins;
    Face YMoins;
    Face ZMoins;

    MapCell[] _allMapCells = new MapCell[0];


    private void OnEnable()
    {
        EventManager.OnEndCubeRotation += CreateMaps;
    }
    private void OnDisable()
    {
        EventManager.OnEndCubeRotation -= CreateMaps;
    }
    private void Start()
    {
        XPlus.actual = new MapCell[9];
        YPlus.actual = new MapCell[9];
        ZPlus.actual = new MapCell[9];

        XMoins.actual = new MapCell[9];
        YMoins.actual = new MapCell[9];
        ZMoins.actual = new MapCell[9];

        _allMapCells = transform.parent.GetComponentsInChildren<MapCell>();
        //CreateMaps();



    }


    List<MapCell> Unsortted = new();
    List<MapCell> path = new();
    void Path()
    {
        Unsortted = _allMapCells.ToList();

        MapCell cell = Unsortted[0];
        Unsortted.Remove(cell);
        path.Add(cell);

        bool dir = false;

        Vector3 north = cell.LocalPosOnFace + Vector2.up;
        if (north.y >= 0 && north.y <= 2)
        {
            var globalPos = cell.transform.TransformPoint(north);
            foreach (MapCell t in Unsortted)
            {
                if (t.transform.position == globalPos)
                {
                    Unsortted.Remove(t);
                    path.Add(t);

                    recursivePath(dir, t);
                }
            }
        }
        else
        {
            //GO NORTH
        }

    }
    void recursivePath(bool dir, MapCell cell)
    {
        Vector3 north = cell.LocalPosOnFace + Vector2.up;
        if (north.y >= 0 && north.y <= 2)
        {
            var globalPos = cell.transform.TransformPoint(north);
            foreach (MapCell t in Unsortted)
            {
                if (t.transform.position == globalPos)
                {
                    Unsortted.Remove(t);
                    path.Add(t);

                    recursivePath(dir, t);
                }
            }
        }

    }

    /*    [Button]
        public void CreateMaps()
        {
            for (int i = 0; i < 9; i++)
            {
                XPlus.actual[i] = null;
                YPlus.actual[i] = null;
                ZPlus.actual[i] = null;

                XMoins.actual[i] = null;
                YMoins.actual[i] = null;
                ZMoins.actual[i] = null;
            }

            XPlus.North = YPlus.actual;
            XPlus.South = YMoins.actual;
            XPlus.West = ZMoins.actual;
            XPlus.East = ZPlus.actual;

            YPlus.North = ZPlus.actual;
            YPlus.South = ZMoins.actual;
            YPlus.West = XMoins.actual;
            YPlus.East = XPlus.actual;

            ZPlus.North = YPlus.actual;
            ZPlus.South = YMoins.actual;
            ZPlus.West = XPlus.actual;
            ZPlus.East = XMoins.actual;


            XMoins.North = YPlus.actual;
            XMoins.South = YMoins.actual;
            XMoins.West = ZPlus.actual;
            XMoins.East = ZMoins.actual;

            YMoins.North = ZPlus.actual;
            YMoins.South = ZMoins.actual;
            YMoins.West = XMoins.actual;
            YMoins.East = XPlus.actual;

            ZMoins.North = YPlus.actual;
            ZMoins.South = YMoins.actual;
            ZMoins.West = XMoins.actual;
            ZMoins.East = XPlus.actual;*

            foreach (var mapCell in _allMapCells)
            {

                var VRight = mapCell.transform.parent.right;
                var CubePos = mapCell.transform.parent.parent.localPosition;
                CubePos += Vector3.one;

                //YMoins 
                if (VRight.y > 0.1f)
                {
                    mapCell.LocalPosOnFace = new Vector2(CubePos.x, CubePos.z);
                    YMoins.actual[(int)(CubePos.z * 3 + CubePos.x)] = mapCell;
                    continue;
                }
                //YPlus;
                if (VRight.y < -0.5f)
                {
                    mapCell.LocalPosOnFace = new Vector2(CubePos.x, CubePos.z);
                    YPlus.actual[(int)(CubePos.z * 3 + CubePos.x)] = mapCell;
                    continue;
                }


                if (VRight.x > 0.1f)
                {
                    mapCell.LocalPosOnFace = new Vector2(CubePos.z, CubePos.y);
                    XMoins.actual[(int)(CubePos.y * 3 + CubePos.z)] = mapCell;
                    continue;
                }
                if (VRight.x < -0.1f)
                {
                    mapCell.LocalPosOnFace = new Vector2(CubePos.z, CubePos.y);
                    XPlus.actual[(int)(CubePos.y * 3 + CubePos.z)] = mapCell;
                    continue;
                }


                if (VRight.z > 0.1f)
                {
                    mapCell.LocalPosOnFace = new Vector2(CubePos.x, CubePos.y);
                    ZMoins.actual[(int)(CubePos.z * 3 + CubePos.y)] = mapCell;
                    continue;
                }
                if (VRight.z < -0.1f)
                {
                    mapCell.LocalPosOnFace = new Vector2(CubePos.x, CubePos.y);
                    ZPlus.actual[(int)(CubePos.z * 3 + CubePos.x)] = mapCell;
                    continue;
                }
            }


            for (int i = 0; i < 9; i++)
            {
                if (XPlus.actual[i]) print("XPlus " + XPlus.actual[i].LocalPosOnFace);
                if (YPlus.actual[i]) print("YPlus " + YPlus.actual[i].LocalPosOnFace);
                if (ZPlus.actual[i]) print("ZPlus " + ZPlus.actual[i].LocalPosOnFace);

                if (XMoins.actual[i]) print("XMoins " + XMoins.actual[i].LocalPosOnFace);
                if (YMoins.actual[i]) print("YMoins " + YMoins.actual[i].LocalPosOnFace);
                if (ZMoins.actual[i]) print("ZMoins " + ZMoins.actual[i].LocalPosOnFace);
            }

            FindPath();
        }*/

    /*
        [Button]
        public void FindPath()
        {
            var groscon2 = _allMapCells.ToList();
            MapCell[] allRanged = new MapCell[54];

            XPlus.actual.CopyTo(allRanged, 0);
            YPlus.actual.CopyTo(allRanged, 9);
            ZPlus.actual.CopyTo(allRanged, 18);
            XMoins.actual.CopyTo(allRanged, 27);
            YMoins.actual.CopyTo(allRanged, 36);
            ZMoins.actual.CopyTo(allRanged, 45);


            for (int i = 0; i < allRanged.Length; i++)
            {
                var gc = allRanged[i];
                groscon2.Remove(gc);



                //Verify();
            }
        }

        public bool Verify(MapCell mc, Face face)
        {
            Vector3 north = mc.LocalPosOnFace + Vector2.up;
            if (
                north.x >= 0 && north.x <= 2
                && north.y >= 0 && north.y <= 2
               )
            {
            }
            else
            {
                //GO NORTH
            }







                return Verify();
        }*/
}

struct Face
{
    public MapCell[] actual;

    public MapCell[] North;
    public MapCell[] South;
    public MapCell[] West;
    public MapCell[] East;

}