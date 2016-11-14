/*
 * Created by SharpDevelop.
 * User: Michael
 * Date: 11/11/2016
 * Time: 10:30 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace MazeSolver {
	
	class Program {
		
		public static void Main(string[] args) {
			if(args.Length == 2) {
				MazeSolver mazeSolver = new MazeSolver();
				Console.WriteLine("Solving maze...");
				mazeSolver.Solve(new Maze(args[0], args[1]));
				Console.WriteLine("Maze solved! Check your destination path!");
			} else {
				Console.WriteLine("Insufficient Arguments!");
			}
		}
		
	}
}