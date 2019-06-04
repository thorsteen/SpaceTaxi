namespace SpaceTaxi_3.States
{
    public class LevelController
    {
        private string LevelName;
        
        /// <summary>
        /// Default level
        /// </summary>
        public LevelController()
        {
            LevelName = "the-beach.txt";
        }
        
        /// <summary>
        /// Sets on of the two level name for later use of LevelController in levelParser.
        /// </summary>
        /// <param name="i">int</param>
        public void setLevel(int i)
        {
            if (i == 0)
            {
                LevelName = "short-n-sweet.txt";
            }
            else
            {
                LevelName = "the-beach.txt";   
            }
        }
        
        /// <summary>
        /// Returns the set level name.
        /// </summary>
        /// <returns>String</returns>
        public string returnLevel()
        {
            return LevelName;
        }
        
        
    }
}