using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[ExecuteAlways]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    public int XSize;
    public int ZSize;
    public float height;

    public NoiseLayer[] Layers;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

    }

    // Update is called once per frame
    void Update()
    {
        vertices = new Vector3[(XSize + 1) * (ZSize + 1)];

        CreateMesh();
        UpdateMesh();
    }

    void CreateMesh()
	{
        int i = 0;
        for(int z = 0; z <= ZSize; z++)
		{
            for (int x = 0; x <= XSize; x++)
			{
                float Y = 0;
                foreach(NoiseLayer n in Layers)
                {
                    Y += Mathf.PerlinNoise(x * n.scale, z * n.scale) * n.height;
				}
                vertices[i] = new Vector3(x, Y, z);
                i++;
			}
		}

        int vert = 0;
        int tris = 0;
        triangles = new int[XSize * ZSize * 6];
        for (int z = 0; z < ZSize; z++)
        {
            for (int x = 0; x < XSize; x++)
            {
                triangles[tris] = vert;
                triangles[tris + 1] = vert + XSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + XSize + 1;
                triangles[tris + 5] = vert + XSize + 2;
                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    void UpdateMesh()
	{
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

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

    [Serializable]
    public class NoiseLayer
	{
        public float scale;
        public float height;
	}
}
