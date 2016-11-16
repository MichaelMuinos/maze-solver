/*
 * User: Michael
 * Date: 11/14/2016
 * Time: 12:27 AM
 * 
 */
using System;

namespace MazeSolver
{
	/*
	 * Point struct represents a pixel coordinate on the maze picture
	 */
	public struct Point {
		public int x;
		public int y;
		
		public Point(int x, int y) {
			this.x = x;
			this.y = y;
		}
	}
	
}
