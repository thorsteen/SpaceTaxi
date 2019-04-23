using System.Collections.Generic;

namespace SpaceTaxi_1 {
    public class LevelParser {
        private string TextInFile(string fileName) { //returns the string of a text file
            return "dummy text";
        } //RETURNS USELESS TEXT FOR NOW

        private Level TextToLevel(string levelFileString) { //translates text into a level
            return new Level(new char[,] {{ 'a', 'b' }, { 'c', 'd' }}, "Dummy map", 2, new Dictionary<char, string> {{'a', "test.png"}}, new List<Customer>());
        } //RETURNS USELESS MAP FOR NOW
        
        public Level CreateLevel(string levelFileName) { //creates a level from a file name
            return TextToLevel(TextInFile(levelFileName));
        }
    }
}