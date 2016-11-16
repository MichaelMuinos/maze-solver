/*
 * User: Michael
 * Date: 11/11/2016
 * Time: 10:30 PM
 * 
 */
using System;

namespace MazeSolver {
	
	/*
	 * Driver class to process the command arguments.
	 */
	class Program {
		
		public static void Main(string[] args) {
			// If arg length not 2, incorrect amount of args
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