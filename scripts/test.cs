using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Wfc;
using TilesetDefs;
using System.Xml.Schema;

public partial class test : Node2D
{
    [Export] int width;
    [Export] int height;
	[Export] int tileSize;
	[Export] string tilesetName;
	
    public override void _Ready() {
		string[] filenames = Directory.GetFiles($"tileset_scenes/{tilesetName}").OrderBy(f => f[^(char.IsDigit(f[^7]) ? 7 : 6)..^5].ToInt()).ToArray();

		string path;
		PackedScene[] tileScenes = new PackedScene[filenames.Length];

		for (int i = 0; i < filenames.Length; i++) {
			path = $"res://{filenames[i]}";
			tileScenes[i] = GD.Load<PackedScene>(path);
		}

		PackedScene[,] result = WFCMapGenerator.Generate(width, height, Tilesets.test, tileScenes);

		Node2D[,] nodes = new Node2D[width, height];

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				nodes[x,y] = (Node2D)result[x,y].Instantiate();
				AddChild(nodes[x,y]);
				nodes[x,y].Position = new Vector2(x * tileSize, y * tileSize);
			}
		}
    }
}
