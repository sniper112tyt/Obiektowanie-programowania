using Wfc;

namespace TilesetDefs
{
	static class Tilesets {
	public static readonly Tile[] test = {
		new(Edge.Blank, Edge.Blank, Edge.Blank, Edge.Blank), // 0
		new(Edge.WallLt, Edge.WallBt, Edge.Blank, Edge.Blank),
		new(Edge.WallRt, Edge.Blank, Edge.Blank, Edge.WallBt),
		new(Edge.Blank, Edge.Blank, Edge.WallRt, Edge.WallTp),
		new(Edge.Blank, Edge.WallTp, Edge.WallLt, Edge.Blank),
		new(Edge.Floor, Edge.WallTp, Edge.Blank, Edge.WallTp), // 5
		new(Edge.WallLt, Edge.Blank, Edge.WallLt, Edge.Floor),
		new(Edge.Blank, Edge.WallBt, Edge.Floor, Edge.WallBt),
		new(Edge.WallRt, Edge.Floor, Edge.WallRt, Edge.Blank),
		new(Edge.Floor, Edge.Floor, Edge.WallRt, Edge.WallTp),
		new(Edge.Floor, Edge.WallTp, Edge.WallLt, Edge.Floor), // 10
		new(Edge.WallLt, Edge.WallBt, Edge.Floor, Edge.Floor),
		new(Edge.WallRt, Edge.Floor, Edge.Floor, Edge.WallBt),
		new(Edge.Floor, Edge.Floor, Edge.Floor, Edge.Floor),
		new(Edge.Floor, Edge.Floor, Edge.Floor, Edge.Floor),
		new(Edge.DecoRt, Edge.DecoTp, Edge.Floor, Edge.Floor), // 15
		new(Edge.DecoLt, Edge.Floor, Edge.Floor, Edge.DecoTp),
		new(Edge.Floor, Edge.Floor, Edge.DecoLt, Edge.DecoBt),
		new(Edge.Floor, Edge.DecoBt, Edge.DecoRt, Edge.Floor),
		new(Edge.Deco, Edge.DecoTp, Edge.Floor, Edge.DecoTp),
		new(Edge.DecoLt, Edge.Floor, Edge.DecoLt, Edge.Deco), // 20
		new(Edge.Floor, Edge.DecoBt, Edge.Deco, Edge.DecoBt),
		new(Edge.DecoRt, Edge.Deco, Edge.DecoRt, Edge.Floor),
		new(Edge.Deco, Edge.Deco, Edge.Deco, Edge.Deco),
		new(Edge.Floor, Edge.WallTp, Edge.Corridor, Edge.WallTp),
		new(Edge.WallLt, Edge.Corridor, Edge.WallLt, Edge.Floor), // 25
		new(Edge.Corridor, Edge.WallBt, Edge.Floor, Edge.WallBt),
		new(Edge.WallRt, Edge.Floor, Edge.WallRt, Edge.Corridor),
		new(Edge.Corridor, Edge.Blank, Edge.Corridor, Edge.Blank),
		new(Edge.Blank, Edge.Corridor, Edge.Blank, Edge.Corridor),
		new(Edge.Corridor, Edge.Corridor, Edge.Blank, Edge.Corridor), // 30
		new(Edge.Corridor, Edge.Blank, Edge.Corridor, Edge.Corridor),
		new(Edge.Blank, Edge.Corridor, Edge.Corridor, Edge.Corridor),
		new(Edge.Corridor, Edge.Corridor, Edge.Corridor, Edge.Blank),
		new(Edge.Corridor, Edge.Corridor, Edge.Blank, Edge.Blank),
		new(Edge.Corridor, Edge.Blank, Edge.Blank, Edge.Corridor), // 35
		new(Edge.Blank, Edge.Blank, Edge.Corridor, Edge.Corridor),
		new(Edge.Blank, Edge.Corridor, Edge.Corridor, Edge.Blank)
		};
	}
}