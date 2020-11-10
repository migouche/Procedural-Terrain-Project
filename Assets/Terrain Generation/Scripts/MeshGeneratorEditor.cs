using UnityEngine;
using UnityEditor;
using System.IO;
 
[CustomEditor(typeof(MeshGenerator))]
public class MeshGeneratorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		MeshGenerator MeshG = (MeshGenerator)target;
		DrawDefaultInspector();
		if (GUILayout.Button("Save Mesh"))
		{
			Debug.Log("Save Mesh");

            string path = EditorUtility.OpenFolderPanel("Select Folder for saving Mesh to obj", "", "");
            
			File.Create(path + "/Terrain.obj");

			File.Create(path + "/Material.mtl");

			StaticMeshToObj.meshToObj.Save(MeshG.mesh, MeshG.meshRenderer.sharedMaterial, path);
		}
	}
}