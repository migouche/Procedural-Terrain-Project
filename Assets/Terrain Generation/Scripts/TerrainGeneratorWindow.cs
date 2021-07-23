using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TerrainGeneratorWindow : EditorWindow
{

    public int X, Z;
    public bool col;
    public List<MeshGenerator.NoiseLayer> layers;

    [MenuItem("Window/Terrain Generator")]
    public static void StartWindow()
    {
        GetWindow<TerrainGeneratorWindow>("Terrain Generator");
    }

    public void OnGUI()
    {
        GUILayout.Label("Create a terrain configuration", EditorStyles.boldLabel);
        GUILayout.Space(10);
        GUILayout.Label("Size", EditorStyles.boldLabel);
        

        X = EditorGUILayout.IntField("X:", X);
        Z = EditorGUILayout.IntField("Z:", Z);
        GUILayout.Space(5);
        col = EditorGUILayout.Toggle("Use Collider", col);

        GUILayout.Space(10);

        GUILayout.Label("Noise Layers", EditorStyles.boldLabel);

        for (int i = 0; i < layers.Count; i++)
        {

            float height = layers[i].height;
            float scale = layers[i].scale;

            GUILayout.Label("Layer " + (i + 1), EditorStyles.boldLabel);
            
            GUILayout.Space(5);
            
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            
            height = EditorGUILayout.FloatField("Height", height);
            scale = EditorGUILayout.FloatField("Scale", scale);
            
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

        GUILayout.Space(20);
        
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Save Coniguration"))
            Save();

        if (GUILayout.Button("Load Configuration"))
            LoadConfigFromFile();

        GUILayout.EndHorizontal();

        X = Mathf.Clamp(X, 0, 250);
        Z = Mathf.Clamp(Z, 0, 250);
    }

    public void Save()
    {
        ConfigSaveAndLoad.SaveConfig(new MeshGenerator.TerrainConfiguration { X = X, Z = Z, layers = layers.ToArray(), collider = col });   
    }
    
    public void LoadConfigFromFile()
    {
        var config = ConfigSaveAndLoad.LoadConfig();
        if (config != null)
            LoadConfigFromVar(config);
    }
    
    public void LoadConfigFromVar(MeshGenerator.TerrainConfiguration config)
    {
        X = config.X;
        Z = config.Z;

        col = config.collider;

        layers = new List<MeshGenerator.NoiseLayer>();
        foreach (var layer in config.layers)
            layers.Add(layer);
    }
}