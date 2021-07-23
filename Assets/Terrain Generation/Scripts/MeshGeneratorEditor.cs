using UnityEngine;
using UnityEditor;
using System.IO;
using Pinwheel.MeshToFile;
using System.Collections.Generic;
using System.Linq;
 
[CustomEditor(typeof(MeshGenerator))]
public class MeshGeneratorEditor : Editor
{
    public int X, Z;
    public bool col;
    public List<MeshGenerator.NoiseLayer> layers;
    //public MeshGenerator.NoiseLayer[] layers;
    MeshGenerator MeshG;

    public override void OnInspectorGUI()
    {
        MeshG = (MeshGenerator)target;
        //DrawDefaultInspector();

        X = EditorGUILayout.IntField("X:", MeshG.XSize);
        Z = EditorGUILayout.IntField("Z:", MeshG.ZSize);
        GUILayout.Space(5);
        col = EditorGUILayout.Toggle("Use Collider", MeshG.UseCollider);

        GUILayout.Space(10);

        GUILayout.Label("Noise Layers", EditorStyles.boldLabel);

        //if (layers == null || layers.Count == 0)
       //   layers = new List<MeshGenerator.NoiseLayer> { new MeshGenerator.NoiseLayer{ scale = 0, height = 0 } };
        layers = MeshG.Layers.ToList();

        for (int i = 0; i < layers.Count; i++)
        {

            float height = layers[i].height;
            float scale = layers[i].scale;

            GUILayout.Label("Layer " + (i + 1), EditorStyles.boldLabel);

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();

            height = EditorGUILayout.FloatField("Height", layers[i].height);
            scale = EditorGUILayout.FloatField("Scale", layers[i].scale);

            height = Mathf.Clamp(height, 0, float.MaxValue);
            scale = Mathf.Clamp(scale, 0, float.MaxValue);

            layers[i] = new MeshGenerator.NoiseLayer { scale = scale, height = height };

            GUILayout.EndVertical();

            if (GUILayout.Button("Delete layer"))
                layers.RemoveAt(i);

            GUILayout.EndHorizontal();


            GUILayout.Space(10);
        }

        if (GUILayout.Button("Add Layer"))
            if (layers.Count < 10)
                layers.Add(new MeshGenerator.NoiseLayer { scale = 0, height = 0 });
            else
                EditorUtility.DisplayDialog("Can not add more layers!", "You have reached the limit of 10 layers", "ok");

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Save as obj"))
        {
            Debug.Log("Save Mesh");

            string path = EditorUtility.OpenFolderPanel("Select folder to save Mesh as obj", "", "");

            File.Create(path + "/Terrain.obj");

            File.Create(path + "/Material.mtl");

            StaticMeshToObj.meshToObj.Save(MeshG.mesh, MeshG.meshRenderer.sharedMaterial, path);
        }

        if (GUILayout.Button("Save as fbx"))
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

        if (GUILayout.Button("Save configuraton"))
        {
            ConfigSaveAndLoad.SaveConfig(MeshG);
        }

        if (GUILayout.Button("Load Configuration"))
        {
            MeshG.LoadFromConfig(ConfigSaveAndLoad.LoadConfig());
        }
        if (GUI.changed)
        {
            UpdateMeshGenerator();
            Debug.Log("changed");
        }

        GUILayout.EndHorizontal();
    }
    
    public void UpdateMeshGenerator()
    {
        MeshG.XSize = X;
        MeshG.ZSize = Z;
        MeshG.UseCollider = col;
        MeshG.Layers = layers.ToArray();
        MeshG.CreateMesh();
    }
}