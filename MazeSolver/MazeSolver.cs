/*
 * Created by SharpDevelop.
 * User: Michael
 * Date: 11/13/2016
 * Time: 5:38 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Collections.Generic;

namespace MazeSolver {
	
	public class MazeSolver {
		
		enum Shade {
			Black,
			White,
			Red,
			Blue
		};
		
		public void Solve(Maze maze) {
			int xStart = 0;
			int yStart = 0;
			bool foundStart = false;
			
			Bitmap inputMaze = new Bitmap (@maze.getSourcePath());
			Bitmap outputMaze = new Bitmap (inputMaze.Width, inputMaze.Height);
			for (int x = 0; x < inputMaze.Width; ++x) {
				for (int y = 0; y < inputMaze.Height; ++y) {
					outputMaze.SetPixel(x, y, inputMaze.GetPixel(x, y));
					if(!foundStart && extractColor(inputMaze.GetPixel(x, y)) == Shade.Red) {
						List<Tuple<int,int>> adjacentStartPixels = GetAdjacent(new Tuple<int,int>(x, y));
						for(int z = 0; z < adjacentStartPixels.Count; z++) {
							Tuple<int,int> adjTemp = adjacentStartPixels[z];
							if(extractColor(inputMaze.GetPixel(adjTemp.Item1, adjTemp.Item2)) == Shade.White) {
								xStart = x;
								yStart = y;
								foundStart = true;
							}
						}
					}
				}
			}
			
			List<Tuple<int,int>> solution = performBFS(new Tuple<int,int>(xStart, yStart), inputMaze);
			if(solution != null) {
				for(int i = 0; i < solution.Count; i++) {
					outputMaze.SetPixel(solution[i].Item1, solution[i].Item2, Color.Green);
				}
			}
			
			outputMaze.Save (@maze.getDestinationPath());
		}
		
		private List<Tuple<int,int>> performBFS(Tuple<int,int> start, Bitmap maze) {
			Queue<List<Tuple<int,int>>> queue = new Queue<List<Tuple<int,int>>>();
			HashSet<Tuple<int,int>> hashSet = new HashSet<Tuple<int,int>>();
			List<Tuple<int,int>> startList = new List<Tuple<int, int>>();
			startList.Add(start);
			queue.Enqueue(startList);
			
			while(queue.Count != 0) {
				List<Tuple<int,int>> temp = new List<Tuple<int, int>>();
				temp.AddRange(queue.Dequeue());
				Tuple<int,int> endOfTemp = temp[temp.Count - 1];
				
				if(extractColor(maze.GetPixel(endOfTemp.Item1, endOfTemp.Item2)) == Shade.Blue) {
					return temp;
				}
				
				List<Tuple<int,int>> adjacentPositions = new List<Tuple<int, int>>();
				adjacentPositions.AddRange(GetAdjacent(endOfTemp));
				for(int i = 0; i < adjacentPositions.Count; i++) {
					Tuple<int,int> adjacentTemp = adjacentPositions[i];
					if(!hashSet.Contains(adjacentTemp)) {
						Shade color = extractColor(maze.GetPixel(adjacentTemp.Item1, adjacentTemp.Item2));
						if(color == Shade.White || color == Shade.Blue) {
							hashSet.Add(adjacentTemp);
							// Add new path to queue with extra adjacent pixel
							List<Tuple<int,int>> list = new List<Tuple<int, int>>();
							list.AddRange(temp);
							list.Add(adjacentTemp);
							queue.Enqueue(list);
						}
					}
				}
			}
			return null;
		}
		
		private List<Tuple<int,int>> GetAdjacent(Tuple<int,int> position) {
			List<Tuple<int,int>> list = new List<Tuple<int,int>>();
			// add all adjacent pixels from position to a list
			list.Add(new Tuple<int,int>(position.Item1 - 1, position.Item2));
			list.Add(new Tuple<int,int>(position.Item1 + 1, position.Item2));
			list.Add(new Tuple<int,int>(position.Item1, position.Item2 - 1));
			list.Add(new Tuple<int,int>(position.Item1, position.Item2 + 1));
			return list;
		}
		
		private Shade extractColor(Color color) {
		    if(color.GetBrightness() < 0.2) return Shade.Black;
		    if(color.GetBrightness() > 0.8) return Shade.White;
		    
		    if(color.GetHue() < 30) return Shade.Red;
	    	if(hue < 270 && hue > 210) return Shade.Blue;
		  
	    	return Shade.Red;
		}
		
	}
	
}
