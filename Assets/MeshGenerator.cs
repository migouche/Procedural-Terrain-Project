using UnityEngine;
using UnityEditor;

[ExecuteAlways]
[RequireComponent(typeof(MeshRenderer))][RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    Vector2[] uvs;

    public int XSize = 20;
    public int ZSize = 20;

    public NoiseLayer[] Layers = new NoiseLayer[] { new NoiseLayer { scale = 5, height = 2 } };
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
                    Y += Mathf.PerlinNoise((x + transform.position.x) / n.scale, (z + transform.position.z) / n.scale) * n.height;
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
       
        int ii = 0;
        uvs = new Vector2[vertices.Length];
        for (int z = 0; z <= ZSize; z++)
        {
            for (int x = 0; x <= XSize; x++)
            {
                uvs[ii] = new Vector2((float)x / XSize, (float)x / ZSize);
                ii++;
            }
        }
    }

    void UpdateMesh()
	{
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        mesh.uv = uvs;

    }


    private void OnDrawGizmos()
	{
        if (vertices != null)
        {
            for (int i = 0; i < vertices.Length; i++)
                Gizmos.DrawSphere(vertices[i] + transform.position, .1f);
        }
    }

    [MenuItem("GameObject/Terrain Generator/New Terrain", false, 12)]
    public static void NewTerrain()
	{
        new GameObject("New Terrain", typeof(MeshGenerator));
	}

    [System.Serializable]
    public class NoiseLayer
	{
        public float scale;
        public float height;
	}
}
