﻿using UnityEngine;
using UnityEditor;
using System.IO;
using Pinwheel.MeshToFile;
 
[CustomEditor(typeof(MeshGenerator))]
public class MeshGeneratorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		MeshGenerator MeshG = (MeshGenerator)target;
		DrawDefaultInspector();

        GUILayout.BeginHorizontal();
        
		if (GUILayout.Button("Save as obj"))
		{
			Debug.Log("Save Mesh");

            string path = EditorUtility.OpenFolderPanel("Select folder to save Mesh as obj", "", "");
            
			File.Create(path + "/Terrain.obj");

			File.Create(path + "/Material.mtl");

			StaticMeshToObj.meshToObj.Save(MeshG.mesh, MeshG.meshRenderer.sharedMaterial, path);
		}
        
        if(GUILayout.Button("Save as fbx"))
        {/*
            string path = EditorUtility.OpenFolderPanel("Select folder to save Mesh as fbx", "", "");

            StaticMeshToObj.meshToFbx.Save(MeshG.mesh, MeshG.meshRenderer.sharedMaterial, path);
        */
            EditorUtility.DisplayDialog("Coming soon!", "This feature is not yet implemented!", "ok");
            //Debug.Log("Coming Soon!");
        }

        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();

		if(GUILayout.Button("Save configuraton"))
		{
			ConfigSaveAndLoad.SaveConfig(MeshG);
		}
        
        if(GUILayout.Button("Load Configuration"))
        {
            MeshG.LoadFromConfig(ConfigSaveAndLoad.LoadConfig());
        }

        GUILayout.EndHorizontal();
    }
}