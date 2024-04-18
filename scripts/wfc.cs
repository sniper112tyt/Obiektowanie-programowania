using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
using Godot;
using GodotPlugins.Game;

namespace Wfc {
	struct WFCDomain {
		int m_size;
		int? m_value;
		HashSet<int> m_options;

		public readonly bool isCollapsed {
			get {
				return m_options.Count == 1;
			}
		}

		public int Value { 
			get {
				if (isCollapsed && m_value == null) m_value = m_options.ToArray()[0];
				else if (!isCollapsed) throw new NullReferenceException("Attempt to get value of uncollapsed tile.");
				return (int)m_value;
			}
		}

		public HashSet<int> Options {
			readonly get {
				return m_options;
			}
			set {
				m_options = value;
			}
		}

		public readonly int Entropy {
			get {
				return m_options.Count;
			}
		}

		public WFCDomain(int optionCount, bool empty = false) {
			m_options = new HashSet<int>(optionCount);
			m_size = optionCount;
			m_value = null;

			if (!empty)
			for (int i = 0; i < optionCount; i++) {
				m_options.Add(i);
			}
		}

		public void Observe() {
			Random rng = new();
			if (m_options.Count == 1) {
				m_value = m_options.ElementAt(0);
				// GD.Print(m_value);
			}
			else if (m_options.Count != 0) {
				int observedTile = m_options.ElementAt(rng.Next(m_options.Count));
				m_value = observedTile;

				HashSet<int> intersectSet = new() {observedTile};
				m_options.IntersectWith(intersectSet);
			}
		}

		public WFCDomain Copy() {
			WFCDomain d = new(m_size);
			int[] options_arr = new int[m_size];
			Options.CopyTo(options_arr);
			d.Options = options_arr.ToHashSet();
			return d;
		}
	}

	struct WFCConstraint {
		public HashSet<int> top;
		public HashSet<int> right;
		public HashSet<int> bottom;
		public HashSet<int> left;

		public WFCConstraint() {
			top = new();
			right = new();
			bottom = new();
			left = new();
		}
	}

    public static class WFCMapGenerator {
		static readonly Random rng = new();

		private static WFCDomain[,] CopyArray(WFCDomain[,] src, int sizex, int sizey) {
			WFCDomain[,] dest = new WFCDomain[sizex, sizey];
			for (int x = 0; x < sizex; x++) {
				for (int y = 0; y < sizey; y++) {
					dest[x, y] = src[x, y].Copy();
				}
			}
			return dest;
		}
		private static bool[,] CopyArray(bool[,] src, int sizex, int sizey) {
			bool[,] dest = new bool[sizex, sizey];
			for (int x = 0; x < sizex; x++) {
				for (int y = 0; y < sizey; y++) {
					dest[x, y] = src[x, y];
				}
			}
			return dest;
		}

		public static PackedScene[,] Generate(int width, int height, Tile[] tileData, PackedScene[] tileSet) {
			int tileCount = tileData.Length;
			PackedScene[,] map = new PackedScene[width, height];
			bool[,] solvedMask = new bool[width, height];
			bool[,] solvedMask_save = new bool[width, height];
			WFCDomain[,] domains = new WFCDomain[width, height];
			WFCDomain[,] domains_save = new WFCDomain[width, height];
			WFCConstraint[] constraints = new WFCConstraint[tileCount];

			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					domains[x, y] = new WFCDomain(tileCount);
					domains_save[x, y] = new WFCDomain(tileCount);
				}
			}

			// Set constraints
			for (int current = 0; current < tileCount; current++) {
				constraints[current] = new();
				for (int i = 0; i < tileCount; i++) {
					if(tileData[i].edgeBt == tileData[current].edgeTp) {
						constraints[current].top.Add(i);
					}
					if(tileData[i].edgeLt == tileData[current].edgeRt) {
						constraints[current].right.Add(i);
					}
					if(tileData[i].edgeTp == tileData[current].edgeBt) {
						constraints[current].bottom.Add(i);
					}
					if(tileData[i].edgeRt == tileData[current].edgeLt) {
						constraints[current].left.Add(i);
					}
				}
			}
			bool solved = false, randomize, backtrace;

            int fx, fy;
            fx = rng.Next(width);
            fy = rng.Next(height);

            solvedMask[fx, fy] = true;
            domains[fx, fy].Observe();
            GD.Print($"{fx}, {fy}: {domains[fx, fy].Value}, {domains[fx, fy].Entropy}");

			// Main loop
			while (!solved) {
				// GD.Print("------------------------------------------");
				solved = true;
				randomize = true;
				backtrace = false;
				for (int x = 0; x < width; x++) {
					for (int y = 0; y < height; y++) {
						// GD.Print($"Entropy={domains[x, y].Entropy} at {x},{y}, where solved={solvedMask[x, y]}, {backtrace}");
						
						if (domains[x, y].Entropy == 0) {
							backtrace = true;
							solved = false;
							// GD.Print($"Zero at {x},{y}");
						}
						else if (domains[x, y].Entropy == 1) {
							randomize = solvedMask[x, y] && randomize;
							domains[x, y].Observe();
							
							// Propagate constraints
							if (!solvedMask[x, y]) {
								if (y > 0 && !solvedMask[x, y - 1])          domains[x, y - 1].Options.IntersectWith(constraints[domains[x, y].Value].top);
								if (x < width - 1 && !solvedMask[x + 1, y])  domains[x + 1, y].Options.IntersectWith(constraints[domains[x, y].Value].right);
								if (y < height - 1 && !solvedMask[x, y + 1]) domains[x, y + 1].Options.IntersectWith(constraints[domains[x, y].Value].bottom);
								if (x > 0 && !solvedMask[x - 1, y])          domains[x - 1, y].Options.IntersectWith(constraints[domains[x, y].Value].left);
							}
							solvedMask[x, y] = true;
						}
						else {
							solved = false;
						}
					}
				}
				if (solved) break;
				randomize = randomize && !backtrace;

				if (backtrace) {
					// GD.Print("Backtrace");
					domains = CopyArray(domains_save, width, height);
					solvedMask = CopyArray(solvedMask_save, width, height);
					// Array.Copy(domains_save, domains, width * height);
					// Array.Copy(solvedMask_save, solvedMask, width * height);
				}

				if (randomize) {
					// GD.Print("Saving");
					domains_save = CopyArray(domains, width, height);
					solvedMask = CopyArray(solvedMask_save, width, height);
					// Array.Copy(domains, domains_save, width * height);
					// Array.Copy(solvedMask, solvedMask_save, width * height);
					// for (int x = 0; x < width; x++) {
					// 	for (int y = 0; y < height; y++) {
					// 		if(domains_save[x, y].Entropy != 38) {
					// 			GD.Print($"{solvedMask_save[x, y]} at {x},{y}");
					// 		}
					// 	}
					// }

					int rx, ry;
					do {
						rx = rng.Next(width);
						ry = rng.Next(height);
					} while (solvedMask[rx, ry]);

					// GD.Print($"Observing {rx},{ry}");
					domains[rx, ry].Observe();

					// for (int x = 0; x < width; x++) {
					// 	for (int y = 0; y < height; y++) {
					// 		if(domains_save[x, y].Entropy != 38) {
					// 			GD.Print($"{solvedMask_save[x, y]} at {x},{y}");
					// 		}
					// 	}
					// }

				}
			}
			
			// Interpret result
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					// GD.Print($"{domains[x, y].Value}, {x},{y}");
					map[x, y] = tileSet[domains[x, y].Value];
				}
			}

			return map;
		}
    }
}
