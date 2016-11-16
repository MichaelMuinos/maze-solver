/*
 * User: Michael
 * Date: 11/13/2016
 * Time: 5:38 PM
 * 
 */
using System;
using System.Drawing;
using System.Collections.Generic;

namespace MazeSolver {
	
	/*
	 * MazeSolver class hosts all logic for solving maze by passing a Maze object to solve method.
	 */
	public class MazeSolver {
		
		/*
		 * Shade enum handles all colors the maze can have based on criteria given.
		 */
		enum Shade {
			Black,
			White,
			Red,
			Blue
		};
		
		/*
		 * Solve method takes Maze object and extracts the start point on the maze.
		 * It is the only method that has been made public in order to interact with 
		 * the rest of the methods inside.
		 */
		public void Solve(Maze maze) {
			int xStart = 0;
			int yStart = 0;
			bool foundStart = false;
			
			// Start maze
			Bitmap inputMaze = new Bitmap (@maze.getSourcePath());
			// Solved maze
			Bitmap outputMaze = new Bitmap (inputMaze.Width, inputMaze.Height);
			for (int i = 0; i < inputMaze.Width; ++i) {
				for (int j = 0; j < inputMaze.Height; ++j) {
					// Redraw pixels from start maze to solved maze
					outputMaze.SetPixel(i, j, inputMaze.GetPixel(i, j));
					// If pixel is red, get its neighbors and check if it is white
					if(!foundStart && extractColor(inputMaze.GetPixel(i, j)) == Shade.Red) {
						List<Point> adjacentStartPixels = GetAdjacent(new Point(i, j));
						for(int k = 0; k < adjacentStartPixels.Count; k++) {
							Point adjTemp = adjacentStartPixels[k];
							// If neighbor is white, set start point to current i and j coordinates
							if(extractColor(inputMaze.GetPixel(adjTemp.x, adjTemp.y)) == Shade.White) {
								xStart = i;
								yStart = j;
								foundStart = true;
								break;
							}
						}
					}
				}
			}
			
			// Perform Breadth First Search on maze
			List<Point> solution = performBFS(new Point(xStart, yStart), inputMaze);
			if(solution != null) {
				// Solution found, redraw path on our output maze
				for(int i = 0; i < solution.Count; i++) {
					outputMaze.SetPixel(solution[i].x, solution[i].y, Color.Green);
				}
			} else {
				Console.WriteLine("Crap. No solution was found!");
			}
			
			// Save new maze to destination path
			outputMaze.Save (@maze.getDestinationPath());
		}
		
		/*
		 * Determines the path between the red and blue pixel (start to end) using Breadth First Search.
		 * Build a new path for every pixel that is not black (a wall) by getting the adjacent pixels.
		 * If the end of our path is blue, we have found the solution.
		 */
		private List<Point> performBFS(Point start, Bitmap maze) {
			Queue<List<Point>> queue = new Queue<List<Point>>();
			HashSet<Point> visited = new HashSet<Point>();
			List<Point> startList = new List<Point>();
			startList.Add(start);
			queue.Enqueue(startList);
			
			while(queue.Count != 0) {
				// Get first element in queue
				List<Point> temp = queue.Dequeue();
				// Take last point in path
				Point endOfTemp = temp[temp.Count - 1];
				
				// If last point in path is blue, we solved it.
				if(extractColor(maze.GetPixel(endOfTemp.x, endOfTemp.y)) == Shade.Blue) {
					return temp;
				}
				
				// Get neighbors of our last point in path, since we know it is not blue.
				List<Point> adjacentPositions = GetAdjacent(endOfTemp);
				for(int i = 0; i < adjacentPositions.Count; i++) {
					Point adjacentTemp = adjacentPositions[i];
					// For each neighbor, check if it has been visited
					if(!visited.Contains(adjacentTemp)) {
						Shade color = extractColor(maze.GetPixel(adjacentTemp.x, adjacentTemp.y));
						// If the color of our adjacent node is white or blue, create a new path to our queue
						if(color == Shade.White || color == Shade.Blue) {
							visited.Add(adjacentTemp);
							// Add new path to queue with extra adjacent pixel
							List<Point> list = new List<Point>();
							list.AddRange(temp);
							list.Add(adjacentTemp);
							queue.Enqueue(list);
						}
					}
				}
			}
			return null;
		}
		
		/*
		 * Gets all adjacent pixels from given point in maze 
		 */
		private List<Point> GetAdjacent(Point position) {
			List<Point> list = new List<Point>();
			int xCoord = position.x;
			int yCoord = position.y;
			// add all adjacent pixels from position to a list
			list.Add(new Point(xCoord - 1, yCoord));
			list.Add(new Point(xCoord + 1, yCoord));
			list.Add(new Point(xCoord, yCoord - 1));
			list.Add(new Point(xCoord, yCoord + 1));
			return list;
		}
		
		/*
		 * Determine what color the pixel is
		 */
		private Shade extractColor(Color color) {
			// Brightness is low, we know its black
		    if(color.GetBrightness() < 0.2) return Shade.Black;
		    // Brightness is high, we know its white
		    if(color.GetBrightness() > 0.8) return Shade.White;
		    // Hue has less than 30 degree angle, we know it is in range of red
		    if(color.GetHue() < 30) return Shade.Red;
		    // Hue has more than 210 and less than 27- angle, we know it is in range of blue
		    if(color.GetHue() < 270 && color.GetHue() > 210) return Shade.Blue;
	    	return Shade.Red;
		}
		
	}
	
}
