using UnityEngine;
using UnityEditor;
using System.IO;
using Pinwheel.MeshToFile;

[CustomEditor(typeof(MeshGenerator))]
public class MeshGeneratorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		MeshGenerator MeshG = (MeshGenerator)target;
		base.DrawDefaultInspector();
		if (GUILayout.Button("Save Mesh"))
		{
			Debug.Log("Save Mesh");

			string ObjPath = EditorUtility.SaveFilePanel("Save Mesh as .obj", "", "Terrain", "obj");
			File.Create(ObjPath);

			string MatPath = EditorUtility.SaveFilePanel("Save Mesh Material as .mat", "", "Material", "mat");
			File.Create(MatPath);

			StaticMeshToObj.meshToObj.Save(MeshG.mesh, MeshG.meshRenderer.sharedMaterial, ObjPath, MatPath);
		}
	}
}