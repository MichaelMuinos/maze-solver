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
			for (int i = 0; i < inputMaze.Width; ++i) {
				for (int j = 0; j < inputMaze.Height; ++j) {
					outputMaze.SetPixel(i, j, inputMaze.GetPixel(i, j));
					if(!foundStart && extractColor(inputMaze.GetPixel(i, j)) == Shade.Red) {
						List<Tuple<Point>> adjacentStartPixels = GetAdjacent(new Tuple<Point>(new Point(i, j)));
						for(int k = 0; k < adjacentStartPixels.Count; k++) {
							Tuple<Point> adjTemp = adjacentStartPixels[k];
							if(extractColor(inputMaze.GetPixel(adjTemp.Item1.x, adjTemp.Item1.y)) == Shade.White) {
								xStart = i;
								yStart = j;
								foundStart = true;
							}
						}
					}
				}
			}
			
			List<Tuple<Point>> solution = performBFS(new Tuple<Point>(new Point(xStart, yStart)), inputMaze);
			if(solution != null) {
				for(int i = 0; i < solution.Count; i++) {
					outputMaze.SetPixel(solution[i].Item1.x, solution[i].Item1.y, Color.Green);
				}
			}
			
			outputMaze.Save (@maze.getDestinationPath());
		}
		
		private List<Tuple<Point>> performBFS(Tuple<Point> start, Bitmap maze) {
			Queue<List<Tuple<Point>>> queue = new Queue<List<Tuple<Point>>>();
			HashSet<Tuple<Point>> hashSet = new HashSet<Tuple<Point>>();
			List<Tuple<Point>> startList = new List<Tuple<Point>>();
			startList.Add(start);
			queue.Enqueue(startList);
			
			while(queue.Count != 0) {
				List<Tuple<Point>> temp = new List<Tuple<Point>>();
				temp.AddRange(queue.Dequeue());
				Tuple<Point> endOfTemp = temp[temp.Count - 1];
				
				if(extractColor(maze.GetPixel(endOfTemp.Item1.x, endOfTemp.Item1.y)) == Shade.Blue) {
					return temp;
				}
				
				List<Tuple<Point>> adjacentPositions = new List<Tuple<Point>>();
				adjacentPositions.AddRange(GetAdjacent(endOfTemp));
				for(int i = 0; i < adjacentPositions.Count; i++) {
					Tuple<Point> adjacentTemp = adjacentPositions[i];
					if(!hashSet.Contains(adjacentTemp)) {
						Shade color = extractColor(maze.GetPixel(adjacentTemp.Item1.x, adjacentTemp.Item1.y));
						if(color == Shade.White || color == Shade.Blue) {
							hashSet.Add(adjacentTemp);
							// Add new path to queue with extra adjacent pixel
							List<Tuple<Point>> list = new List<Tuple<Point>>();
							list.AddRange(temp);
							list.Add(adjacentTemp);
							queue.Enqueue(list);
						}
					}
				}
			}
			return null;
		}
		
		private List<Tuple<Point>> GetAdjacent(Tuple<Point> position) {
			List<Tuple<Point>> list = new List<Tuple<Point>>();
			int xCoord = position.Item1.x;
			int yCoord = position.Item1.y;
			// add all adjacent pixels from position to a list
			list.Add(new Tuple<Point>(new Point(xCoord - 1, yCoord)));
			list.Add(new Tuple<Point>(new Point(xCoord + 1, yCoord)));
			list.Add(new Tuple<Point>(new Point(xCoord, yCoord - 1)));
			list.Add(new Tuple<Point>(new Point(xCoord, yCoord + 1)));
			return list;
		}
		
		private Shade extractColor(Color color) {
		    if(color.GetBrightness() < 0.2) return Shade.Black;
		    if(color.GetBrightness() > 0.8) return Shade.White;
		    
		    if(color.GetHue() < 30) return Shade.Red;
		    if(color.GetHue() < 270 && color.GetHue() > 210) return Shade.Blue;
		  
	    	return Shade.Red;
		}
		
	}
	
}
