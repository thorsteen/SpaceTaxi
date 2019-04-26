using System.Collections.Generic;
using System.IO;
using System;
using System.Text;

namespace SpaceTaxi_1 {
    public class LevelParser {
        
        public string[] TextInFile(string fileName) { //returns the string of a text file
 
            UTF8Encoding temp = new UTF8Encoding(true);

            string[] lines = File.ReadAllLines(GetLevelFilePath(fileName));

            return lines;

        } 

        private Level TextToLevel(string[] levelFileString) { //translates text into a level
            return new Level(new string[] {"hej","hej2"}, "Dummy map", new char[] {'c'}, new Dictionary<char, string> {{'a', "test.png"}}, new List<Customer>());
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