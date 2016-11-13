/*
 * Created by SharpDevelop.
 * User: Michael
 * Date: 11/11/2016
 * Time: 10:30 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Collections.Generic;

namespace MazeSolver
{
	class Program
	{
		
		public static void Main(string[] args)
		{
			int xStart = 0;
			int yStart = 0;
			bool foundStart = false;
			
			Bitmap image = new Bitmap (@"C:\Users\Luke\Desktop\maze3.png");
			Bitmap newImage = new Bitmap (image.Width, image.Height);
			for (int x = 0; x < image.Width; ++x) {
				for (int y = 0; y < image.Height; ++y) {
					// TESTING: used for drawing path image for testing
					newImage.SetPixel(x, y, image.GetPixel(x, y));
					if(!foundStart && Classify(image.GetPixel(x, y)) == "Red") {
						List<Tuple<int,int>> adjacentStartPixels = getAdjacent(new Tuple<int,int>(x, y));
						for(int z = 0; z < adjacentStartPixels.Count; z++) {
							Tuple<int,int> adjTemp = adjacentStartPixels[z];
							if(Classify(image.GetPixel(adjTemp.Item1, adjTemp.Item2)) == "White") {
								xStart = x;
								yStart = y;
								foundStart = true;
							}
						}
					}
				}
			}
			
			List<Tuple<int,int>> solution = performBFS(new Tuple<int, int>(xStart, yStart), image, newImage);
			Console.WriteLine("Outside BFS");
			if(solution != null) {
				Console.WriteLine("Found solution.");
				for(int i = 0; i < solution.Count; i++) {
					newImage.SetPixel(solution[i].Item1, solution[i].Item2, Color.Green);
				}
			}
			Console.WriteLine("Saving");
			
			//fillWhiteTest(new Tuple<int, int>(xStart, yStart), image, newImage);
			
			newImage.Save (@"C:\Users\Luke\Desktop\customMazenew4.png");
			Console.ReadKey(true);
		}
		
//		public static void fillWhiteTest(Tuple<int,int> start, Bitmap bitmap, Bitmap newImage) {
//			Queue<Tuple<int,int>> queue = new Queue<Tuple<int,int>>();
//			HashSet<Tuple<int,int>> hashSet = new HashSet<Tuple<int,int>>();
//			queue.Enqueue(start);
//			
//			while(queue.Count != 0) {
//				Tuple<int,int> temp = queue.Dequeue();
//				// TESTING
//				//newImage.SetPixel(endOfTemp.Item1, endOfTemp.Item2, Color.Green);
//				
////				// We found the solution!
////				if(Classify(bitmap.GetPixel(endOfTemp.Item1, endOfTemp.Item2)) == "Blue") {
////					return temp;
////				}
//				
//				List<Tuple<int,int>> adjacentPositions = getAdjacent(temp);
//				for(int i = 0; i < adjacentPositions.Count; i++) {
//					Tuple<int,int> adjacentTemp = adjacentPositions[i];
//					
//					if(Classify(bitmap.GetPixel(adjacentTemp.Item1, adjacentTemp.Item2)) == "White" && !hashSet.Contains(adjacentTemp)) {
//						// TESTING
//						newImage.SetPixel(adjacentTemp.Item1, adjacentTemp.Item2, Color.Green);
//						
//						hashSet.Add(adjacentTemp);
//						// Add new path to queue with extra adjacent pixel
//						queue.Enqueue(adjacentTemp);
//					}
//				}
//			}
//		}
		
		public static List<Tuple<int,int>> performBFS(Tuple<int,int> start, Bitmap bitmap, Bitmap newImage) {
			Queue<List<Tuple<int,int>>> queue = new Queue<List<Tuple<int,int>>>();
			HashSet<Tuple<int,int>> hashSet = new HashSet<Tuple<int,int>>();
			List<Tuple<int,int>> startList = new List<Tuple<int, int>>();
			startList.Add(start);
			queue.Enqueue(startList);
			
			//System.IO.StreamWriter file = 
				//new System.IO.StreamWriter(@"C:\Users\Luke\Desktop\WriteLines2.txt");
			
			while(queue.Count != 0) {
				List<Tuple<int,int>> temp = new List<Tuple<int, int>>();
				temp.AddRange(queue.Dequeue());
				Tuple<int,int> endOfTemp = temp[temp.Count - 1];
				// TESTING
				//newImage.SetPixel(endOfTemp.Item1, endOfTemp.Item2, Color.Green);
				
				//file.WriteLine(Classify(bitmap.GetPixel(endOfTemp.Item1, endOfTemp.Item2)));
				// We found the solution!
				if(Classify(bitmap.GetPixel(endOfTemp.Item1, endOfTemp.Item2)) == "Blue") {
					return temp;
				}
				
				List<Tuple<int,int>> adjacentPositions = new List<Tuple<int, int>>();
				adjacentPositions.AddRange(getAdjacent(endOfTemp));
				for(int i = 0; i < adjacentPositions.Count; i++) {
					Tuple<int,int> adjacentTemp = adjacentPositions[i];
					if(!hashSet.Contains(adjacentTemp)) {
						String color = Classify(bitmap.GetPixel(adjacentTemp.Item1, adjacentTemp.Item2));
						if(color == "White" || color == "Blue") {
							// TESTING
							//newImage.SetPixel(adjacentTemp.Item1, adjacentTemp.Item2, Color.Green);
							
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
			//file.Close();
			return null;
		}
		
		public static List<Tuple<int,int>> getAdjacent(Tuple<int,int> position) {
			List<Tuple<int,int>> list = new List<Tuple<int,int>>();
			int i = position.Item1;
			int j = position.Item2;
//			if(i - 1 > 0) 
			list.Add(new Tuple<int,int>(i - 1, j));
//			if(i + 1 < bitmap.Width) 
			list.Add(new Tuple<int,int>(i + 1, j));
//			if(j - 1 > 0) 
			list.Add(new Tuple<int,int>(i, j - 1));
//			if(j + 1 < bitmap.Height) 
			list.Add(new Tuple<int,int>(i, j + 1));
			return list;
		}
		
		public static string Classify(Color c)
		{
		    float hue = c.GetHue();
		    float brightness = c.GetBrightness();
		    
		    if(brightness < 0.2) return "Black";
		    if(brightness > 0.8) return "White";
		    
	    	if(hue < 30) return "Red";
	    	if(hue < 270 && hue > 210) return "Blue";
		  
		    return "Red";
		}
		
	}
}