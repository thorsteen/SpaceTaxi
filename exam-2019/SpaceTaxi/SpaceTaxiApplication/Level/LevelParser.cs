using System;
using System.Collections.Generic;
using System.IO;

namespace SpaceTaxiApplication.Level {
    public class LevelParser {
        
        /// <summary>
        ///  Looks up a text file and saves all the text as strings, line by line
        /// </summary>
        /// <param name="fileName">The name of the file to look in</param>
        /// <returns>a string array, containing each line</returns>
        public string[] TextInFile(string fileName) {
 
            string[] lines = File.ReadAllLines(GetLevelFilePath(fileName));

            return lines;
        } 
        
        /// <summary>
        /// This function is able to translate level file text into a Level object.
        /// This is done by a for-loop that iterates over the whole array, and then separates them into smaller pieces. 
        /// </summary>
        /// <param name="levelFileLines">a string array containing each line from a level file</param>
        /// <returns>A Level</returns>
        private Level TextToLevel(string[] levelFileLines) {

            List<string> map = new List<string>();
            
            string mapName = "";
            
            Dictionary<char,string> keyLegend = new Dictionary<char, string>();
            
            List<Customer.Customer> customers = new List<Customer.Customer>();
            
            List<char> platforms = new List<char>();
            

            // Gives the Map to a list 
            for (int lineNum = 0; lineNum < levelFileLines.Length; lineNum++) {
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
                    string temp2 = temp.Replace("Customer: ", "");
                    string[] customerStrings = temp2.Split(new string[] {" "}, StringSplitOptions.None);

                    customers.Add(new Customer.Customer(
                        customerStrings[0],
                        Int32.Parse(customerStrings[1]),customerStrings[2][0],
                        customerStrings[3], 
                        Int32.Parse(customerStrings[4]), 
                        Int32.Parse(customerStrings[5]),map.ToArray()));      
                }
            }
            
            return new Level(map.ToArray(), mapName, platforms, keyLegend, customers);
        }
        
        /// <summary>
        /// Creates a level via TextToLevel and TextInFile
        /// </summary>
        /// <param name="levelFileName">The name of the file to look in</param>
        /// <returns>A Level</returns>
        public Level CreateLevel(string levelFileName) {
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