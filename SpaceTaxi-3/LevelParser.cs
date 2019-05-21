using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Math;

namespace SpaceTaxi_3 {
    public class LevelParser {
        
        public string[] TextInFile(string fileName) { //returns the string of a text file
 
            string[] lines = File.ReadAllLines(GetLevelFilePath(fileName));

            return lines;

        } 
        /// <summary>
        /// The function is able to translate the file text into several different arrays and strings, which are passed through to the level later on. This is done by a for-loop that itterates through the whole array, and then sepparates them into smaller pieces. 
        /// </summary>
        /// <param name="levelFileLines"></param>
        /// <returns>Level</returns>
        private Level TextToLevel(string[] levelFileLines) { //translates text into a level

            List<string> map = new List<string>();
            string mapName = "";
            Dictionary<char,string> keyLegend = new Dictionary<char, string>();
            List<Customer> customers = new List<Customer>();
            List<char> platforms = new List<char>();
            

            // Gives the Map to a list 
            for (int lineNum = 0; lineNum < levelFileLines.Length -1; lineNum++) {
                if (lineNum < 23) {
                    map.Add(levelFileLines[lineNum]);
                }
                // Gives the MapName
                else if (lineNum == 24) {
                    string temp = levelFileLines[lineNum];
                    mapName = temp.Replace("Name: ", "");
                }
                else if (lineNum == 25) {
                    string temp = levelFileLines[lineNum];
                    string temp2 = temp.Replace("Platforms: ", "");
                    for (int i = 0; i < temp2.Length; i += 3) {
                        platforms.Add(temp2[i]); 
                    }
                    
                }
                // Makes the dictonary, connecting the lettes and their respective PNG files 
                else if (levelFileLines[lineNum].Contains(".png")) {
                    string temp = levelFileLines[lineNum];
                    string[] tempDict = temp.Split(new string[] {") "}, StringSplitOptions.None);
                    keyLegend.Add(tempDict[0][0],tempDict[1]);
                }
                else if (levelFileLines[lineNum].Contains("Customer")) {
                    string temp = levelFileLines[lineNum];
                    customers.Add(new Customer(temp.Replace("Customer: ", ""),new DynamicShape(new Vec2F(0.45f,0.1f),new Vec2F(0.1f,0.1f)),
                        new DIKUArcade.Graphics.Image(Path.Combine("Assets","Images","CustomerStandLeft.png"))));
                }
            }
            
            return new Level(map.ToArray(), mapName, platforms, keyLegend, customers);
       
        } //RETURNS NOT SO USELESS MAP
        /// <summary>
        /// Creates a level via TextToLevel / TextInFile
        /// </summary>
        /// <param name="levelFileName"></param>
        /// <returns></returns>
        public Level CreateLevel(string levelFileName) { //creates a level from a file name
            return TextToLevel(TextInFile(levelFileName));
        }
        
        /// <summary>
        /// Finds full directory path of the given level.
        /// </summary>
        /// <remarks>This code is borrowed from Texture.cs in DIKUArcade.</remarks>
        /// <param name="filename">Filename of the level.</param>
        /// <returns>Directory path of the level.</returns>
        /// <exception cref="FileNotFoundException">File does not exist.</exception>
        public string GetLevelFilePath(string filename) {
            // Find base path.
            DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location));

            while (dir.Name != "bin") {
                dir = dir.Parent;
            }
            dir = dir.Parent;

            // Find level file.
            string path = Path.Combine(dir.FullName.ToString(), "Levels", filename);

            if (!File.Exists(path)) {
                throw new FileNotFoundException($"Error: The file \"{path}\" does not exist.");
            }

            return path;
        }

        
    }
}