using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TerrainGenerator : MonoBehaviour
{
    private GameObject tile;
    private GameObject InstantiatedTile;

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    private Vector2[] uvs;

    public int TerrainSize;
    public int TileSize;
    public int VerticesByUnit;

    public NoiseLayer[] Layers;
    // Start is called before the first frame update
    void Start()
    {
        tile = new GameObject();
        mesh = new Mesh();
        tile.AddComponent<MeshRenderer>();
        tile.AddComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        vertices = new Vector3[(TileSize + 1) * (TileSize + 1)];

        CreateMesh();
        UpdateMesh();
    }

    void CreateMesh()
    {
        for (int XSize = 0; XSize < TerrainSize; XSize++)
        {
            for (int ZSize = 0; ZSize < TerrainSize; ZSize++)
            {
                int i = 0;
                for (int z = 0; z <= TileSize; z++)
                {
                    for (int x = 0; x <= TileSize; x++)
                    {
                        float Y = 0;
                        foreach (NoiseLayer n in Layers)
                        {
                            Y += Mathf.PerlinNoise((x + InstantiatedTile.transform.position.x) / n.scale, (z + InstantiatedTile.transform.position.z) / n.scale) * n.height;
                        }
                        vertices[i] = new Vector3(x, Y, z);
                        i++;
                    }
                }

                int vert = 0;
                int tris = 0;
                triangles = new int[TileSize * TileSize * 6];
                for (int z = 0; z < TileSize; z++)
                {
                    for (int x = 0; x < TileSize; x++)
                    {
                        triangles[tris] = vert;
                        triangles[tris + 1] = vert + TileSize + 1;
                        triangles[tris + 2] = vert + 1;
                        triangles[tris + 3] = vert + 1;
                        triangles[tris + 4] = vert + TileSize + 1;
                        triangles[tris + 5] = vert + TileSize + 2;
                        vert++;
                        tris += 6;
                    }
                    vert++;
                }
                int ii = 0;
                uvs = new Vector2[vertices.Length];
                for (int z = 0; z <= TileSize; z++)
                {
                    for (int x = 0; x <= TileSize; x++)
                    {
                        uvs[ii] = new Vector2((float)x / TileSize, (float)x / TileSize);
                        ii++;
                    }
                }
            }
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        if (vertices != null)
        {
            for (int i = 0; i < vertices.Length; i++)
                Gizmos.DrawSphere(vertices[i], .1f);
        }
    }

    [System.Serializable]
    public class NoiseLayer
    {
        public float scale;
        public float height;
    }
}