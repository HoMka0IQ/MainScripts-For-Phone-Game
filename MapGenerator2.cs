using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapGenerator2 : MonoBehaviour
{
    public int SizeMap;
    public float scale;
    public float intensity;
    float _scale;

    int offsetX;
    int offsetY;
    [Range(0,100f)]
    public float wallSize;

    public string seed;

    public GameObject[] MapGO;


    public GameObject AllMapGO;

    float[,] map;
    private void Start()
    {
        GenerateMap();
    }
    public void GenerateMap()
    {
        _scale = ((float)SizeMap / scale) + ((float)SizeMap / scale);
        //create two numbers for map offset from seed
        System.Random pseudoRandomX = new System.Random((seed + "-1").GetHashCode());
        offsetX = pseudoRandomX.Next(0, 99999);
        System.Random pseudoRandomY = new System.Random((seed + "+1").GetHashCode());
        offsetY = pseudoRandomY.Next(0, 99999);
        //

        //Clear old map
        if (AllMapGO != null)
        {
#if UNITY_EDITOR
            DestroyImmediate(AllMapGO);
#else
            Destroy(AllMapGO);
#endif
        }
        //

        //create emptyObject for all go map
        GameObject emptyObject = new GameObject("EmptyObject");
        AllMapGO = emptyObject;
        //

        map = new float[SizeMap, SizeMap];
        //fill map coordinate
        for (int x = 0; x < SizeMap; x++)
        {
            for (int y = 0; y < SizeMap; y++)
            {
                map[x, y] = Mathf.PerlinNoise((float)x / SizeMap * _scale + offsetX, (float)y / SizeMap * _scale + offsetY);
                if (x == 0 || x == SizeMap - 1 || y == 0 || y == SizeMap - 1)
                {
                    map[x, y] = wallSize / intensity;
                }
            }
        }
        //

        //create map
        for (int x = 0; x < SizeMap; x++)
        {
            for (int y = 0; y < SizeMap; y++)
            {
                
                float perlinNoise = map[x, y];

                setCube(x, y, perlinNoise * intensity, emptyObject, 0);
                if (y + 1 < SizeMap)
                {
                    float UnderPos = (map[x, y] * intensity);
                    for (int i = 1; UnderPos > (map[x, y + 1] * intensity) + 1; i++)
                    {
                        setCube(x, y, (perlinNoise * intensity) - i, emptyObject, 1);
                        UnderPos = (perlinNoise * intensity) - i;
                    }

                }
                if (y - 1 >= 0)
                {
                    float UnderPos = (map[x, y] * intensity);
                    for (int i = 1; UnderPos > (map[x, y - 1] * intensity) + 1; i++)
                    {
                        setCube(x, y, (perlinNoise * intensity) - i, emptyObject, 1);
                        UnderPos = (perlinNoise * intensity) - i;

                    }
                }
                if (x + 1 < SizeMap)
                {
                    float UnderPos = (map[x, y] * intensity);
                    for (int i = 1; UnderPos > (map[x + 1, y] * intensity) + 1; i++)
                    {
                        setCube(x, y, (perlinNoise * intensity) - i, emptyObject, 1);
                        UnderPos = (perlinNoise * intensity) - i;
                    }
                }
                if (x - 1 >= 0)
                {
                    float UnderPos = (map[x, y] * intensity);
                    for (int i = 1; UnderPos > (map[x - 1, y] * intensity) + 1; i++)
                    {
                        setCube(x, y, (perlinNoise * intensity) - i, emptyObject, 1);
                        UnderPos = (perlinNoise * intensity) - i;
                    }
                }


            }
        }
        //
    }
    void RemoveSide(GameObject obj, Vector3 side)
    {
        Mesh mesh = obj.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        int index = 0;
        while (index < triangles.Length)
        {
            if (Vector3.Dot(mesh.normals[triangles[index]], side) > 0)
            {
                triangles[index] = triangles[index + 2];
                triangles[index + 1] = triangles[index + 2];
                index += 3;
            }
            else
            {
                index += 3;
            }
        }

        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
    //Create one cube of map
    void setCube(int x, int y, float perlin, GameObject perent, int numberGoSpawn)
    {
        GameObject cube = Instantiate(MapGO[numberGoSpawn], transform.position, Quaternion.identity);
        cube.transform.position = new Vector3(x * MapGO[0].transform.localScale.x, (int)perlin * MapGO[0].transform.localScale.y, y * MapGO[0].transform.localScale.z);
        cube.transform.SetParent(perent.transform);
        
        if (y + 1 < SizeMap && ((int)perlin * MapGO[0].transform.localScale.y) == ((int)(map[x, y + 1] * intensity) * MapGO[0].transform.localScale.y))
        {
            RemoveSide(cube, new Vector3(0, 0, 1));
        }
        if (y - 1 >= 0 && ((int)perlin * MapGO[0].transform.localScale.y) == ((int)(map[x, y - 1] * intensity) * MapGO[0].transform.localScale.y))
        {
            RemoveSide(cube, new Vector3(0, 0, -1));
        }
        if (x + 1 < SizeMap && ((int)perlin * MapGO[0].transform.localScale.y) == ((int)(map[x + 1, y] * intensity) * MapGO[0].transform.localScale.y))
        {
            RemoveSide(cube, new Vector3(1, 0, 0));
        }
        if (x - 1 >= 0 && ((int)perlin * MapGO[0].transform.localScale.y) == ((int)(map[x - 1, y] * intensity) * MapGO[0].transform.localScale.y))
        {
            RemoveSide(cube, new Vector3(-1, 0, 0));
        }
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(MapGenerator2))]
public class GenerateMap : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        MapGenerator2 test = (MapGenerator2)target;
        if (GUILayout.Button("Generate map"))
        {
            test.GenerateMap();
        }

    }
}
#endif
