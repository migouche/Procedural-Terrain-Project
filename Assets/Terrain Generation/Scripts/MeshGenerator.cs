using UnityEngine;
using UnityEditor;
using Pinwheel.MeshToFile;
//using System.Diagnostics;

[HelpURL("https://github.com/migouche/Procedural-Terrain-Project")]

[ExecuteAlways]
[RequireComponent(typeof(MeshRenderer))] [RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    [HideInInspector]
    public Mesh mesh;

    [HideInInspector]
    public MeshRenderer meshRenderer;

    Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;

    private Vector3 LPos;

    public bool UseCollider;

    [Header("Terrain Properties")]  //Terrain Settings
    [Tooltip("Number of Vertices on the X Axis")]
    public int XSize = 20;
    [Tooltip("Number of Vectices on the Z axis")]
    public int ZSize = 20;

    [Space(5)]

    public NoiseLayer[] Layers = { new NoiseLayer { scale = 5, height = 2 } };

    [Space(10)]

    [Header("Gizmos")]  //Gizmos Settings
    [Tooltip("Wether or not to draw the vertices of the Terrain")]
    public bool DrawVertices;

    [Tooltip("Wether or not to draw the Terrain Wireframe")]
    public bool DrawWireFrame;

    [Tooltip("Gizmos' Colour")]
    public Color color;
        
    void Start()
    {

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

    }

    // Update is called once per frame
    void Update()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        if (LPos != transform.position || true)
        {
            vertices = new Vector3[(XSize + 1) * (ZSize + 1)];

            CreateMesh();
            UpdateMesh();
        }
        LPos = transform.position;
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
                uvs[ii] = new Vector2((float)(x + transform.position.x) / XSize , (float)(z + transform.position.z) / ZSize);
                ii++;
            }
        }
    }
    
    public void UpdateCollider()
	{
        if (GetComponent<MeshCollider>())
            GetComponent<MeshCollider>().sharedMesh = mesh;
        else
            gameObject.AddComponent<MeshCollider>().sharedMesh = mesh;
	}

    void UpdateMesh()
	{
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        mesh.uv = uvs;

        if (UseCollider)
            UpdateCollider();
        else if (GetComponent<MeshCollider>())
            DestroyImmediate(GetComponent<MeshCollider>());
    }


    private void OnDrawGizmos()
	{
        Gizmos.color = color;
        if (vertices != null)
        {
            if(DrawVertices)
                foreach (Vector3 v in vertices)
                    Gizmos.DrawSphere(v + transform.position, .1f);
                    
            if(DrawWireFrame)
                Gizmos.DrawWireMesh(mesh, transform.position);
        }
    }
    
    public void LoadFromConfig(TerrainConfiguration config)
    {
        XSize = config.X; ZSize = config.Z;
        Layers = config.layers;
        UseCollider = config.collider;
    }

    [MenuItem("Terrain Generator/New Terrain", false, 12)] [MenuItem("GameObject/Terrain Generator/New Terrain", false, 12)]
    public static void NewTerrain()
	{
        Instantiate(new GameObject("New Terrain", typeof(MeshGenerator)), Vector3.zero, Quaternion.identity);
	}

    [System.Serializable]
    public class NoiseLayer
	{
        public float scale;
        public float height;
	}

    [System.Serializable]
    public class TerrainConfiguration
	{
        public int X, Z;
        public NoiseLayer[] layers;
        public bool collider;
	}
}
