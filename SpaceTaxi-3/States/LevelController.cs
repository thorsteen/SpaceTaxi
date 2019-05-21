namespace SpaceTaxi_3.States
{
    public class LevelController
    {
        private string LevelName;
        
        public LevelController()
        {
            LevelName = "the-beach.txt";
        }

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

        public string returnLevel()
        {
            return LevelName;
        }
    }
}