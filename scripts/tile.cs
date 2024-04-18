using System.Collections.Generic;

namespace Wfc {
	public enum Edge {
	Blank,
	WallTp,
	WallRt,
	WallBt,
	WallLt,
	Floor,
	DecoTp,
	DecoRt,
	DecoBt,
	DecoLt,
	Deco,
	Corridor
	}

	public struct Tile {
		public Edge edgeTp;
		public Edge edgeRt;
		public Edge edgeBt;
		public Edge edgeLt;

		public Tile(Edge edgeTp, Edge edgeRt, Edge edgeBt, Edge edgeLt) {
			this.edgeTp = edgeTp;
			this.edgeRt = edgeRt;
			this.edgeBt = edgeBt;
			this.edgeLt = edgeLt;
		}
	};
}