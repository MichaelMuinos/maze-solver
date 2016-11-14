/*
 * Created by SharpDevelop.
 * User: Michael
 * Date: 11/13/2016
 * Time: 5:53 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace MazeSolver {

	public class Maze {
		
		private readonly String sourcePath;
		private readonly String destinationPath;
		
		public Maze(String sourcePath, String destinationPath) {
			this.sourcePath = sourcePath;
			this.destinationPath = destinationPath;
		}
		
		public String getSourcePath() {
			return sourcePath;
		}
		
		public String getDestinationPath() {
			return destinationPath;
		}
		
	}
	
}
