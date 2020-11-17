using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Json;
using System.IO;
using UnityEditor;
using System.Text;
using static MeshGenerator;

public static class ConfigSaveAndLoad
{
	public static void SaveConfig(TerrainConfiguration config)
	{
		string path = EditorUtility.SaveFilePanel("Save terrain configuration", "", "config", "ptg");
		var ms = new MemoryStream();
		var jsonser = new DataContractJsonSerializer(typeof(TerrainConfiguration));
		jsonser.WriteObject(ms, config);
		byte[] JsonBytes = ms.ToArray();
		ms.Close();
		string Json = Encoding.UTF8.GetString(JsonBytes, 0, JsonBytes.Length);
		var fs = new FileStream(path,  FileMode.OpenOrCreate);
		fs.Write(JsonBytes, 0, JsonBytes.Length);
		fs.Close();
	}

	public static void SaveConfig(MeshGenerator meshGenerator)
	{
		TerrainConfiguration config = new TerrainConfiguration() { X = meshGenerator.XSize, Z = meshGenerator.ZSize, layers = meshGenerator.Layers, collider = meshGenerator.UseCollider };
		SaveConfig(config);
	}

	public static TerrainConfiguration LoadConfig()
	{
        string path = EditorUtility.OpenFilePanel("Select terrain configuration (.ptg)", "", "ptg");

        var jsonser = new DataContractJsonSerializer(typeof(TerrainConfiguration));
        var ms = new MemoryStream(Encoding.UTF8.GetBytes(File.ReadAllText(path)));

        var config = jsonser.ReadObject(ms) as TerrainConfiguration;
        ms.Close();
        return config;   
  	}
}
