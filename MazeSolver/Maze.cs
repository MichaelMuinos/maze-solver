/*
 * User: Michael
 * Date: 11/13/2016
 * Time: 5:53 PM
 * 
 */
using System;

namespace MazeSolver {
	
	/*
	 * Maze class is represented by the source path and destination path of images.
	 * All data members are read only and can't be modified once initialized.
	 */
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
