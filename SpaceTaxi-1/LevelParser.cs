using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using System.Text;

namespace SpaceTaxi_1 {
    public class LevelParser {
        
        public string[] TextInFile(string fileName) { //returns the string of a text file
 
            UTF8Encoding temp = new UTF8Encoding(true);

            string[] lines = File.ReadAllLines(GetLevelFilePath(fileName));

            return lines;

        } 

        private Level TextToLevel(string[] levelFileLines) { //translates text into a level

            List<string> map = new List<string>();
            string mapName = "";
            Dictionary<char,string> keyLegend = new Dictionary<char, string>();
            List<Customer> custumers = new List<Customer>();

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
                // Makes the dictonary, connecting the lettes and their respective PNG files 
                else if (levelFileLines[lineNum].Contains(".png")) {
                    string temp = levelFileLines[lineNum];
                    string[] tempDict = temp.Split(new string[] {") "}, StringSplitOptions.None);
                    keyLegend.Add(tempDict[0][0],tempDict[1]);
                }
                else if (levelFileLines[lineNum].Contains("Customer")) {
                    string temp = levelFileLines[lineNum];
                    custumers.Add(new Customer(temp.Replace("Customer: ", "")));
                }
            }
            
            

            
            return new Level(map.ToArray(), mapName, new char[] {'c'}, keyLegend, custumers);
       
        } //RETURNS USELESS MAP FOR NOW
        
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
        private string GetLevelFilePath(string filename) {
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