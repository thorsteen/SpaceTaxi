namespace SpaceTaxi_3.States
{
    public class LevelController
    {
        private string levelName;
        
        /// <summary>
        /// Default level
        /// </summary>
        public LevelController(){
        
            levelName = "the-beach.txt";
        }
        
        /// <summary>
        /// Sets on of the two level name for later use of LevelController in levelParser.
        /// </summary>
        /// <param name="i">int</param>
        public void SetLevel(int i){      
            if (i == 0){
                levelName = "short-n-sweet.txt";
            }
            else{    
                levelName = "the-beach.txt";   
            }
        }
        
        /// <summary>
        /// Returns the set level name.
        /// </summary>
        /// <returns>String</returns>
        public string ReturnLevel(){        
            return levelName;
        }
        
        
    }
}