using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_3 {
    public class Customer {
        private DynamicShape shape;

        public Entity Entity { get; private set; }
        
        public string Name;
        public int SecondsUntilSpawn;
        public char HomePlatform;
        public string DestinationPlatform;
        public int DropOffTimeLimit;
        public int ScoreForDelivery;
        
        public Vec2I PlatformCoords;
        public Vec2F MyCoords;
        
        public bool PickedUp;
        public bool Delivered;
        
        public Customer(string name, int secondsUntilSpawn, char homePlatform, string destinationPlatform, int dropoffTimeLimit, int scoreForDelivery, string[] map) {
            this.Name = name;
            this.SecondsUntilSpawn = secondsUntilSpawn;
            this.HomePlatform = homePlatform;
            this.DestinationPlatform = destinationPlatform;
            DropOffTimeLimit = dropoffTimeLimit;
            this.ScoreForDelivery = scoreForDelivery;

            PlatformCoords = FindSymbolCoords.Find(map, this.HomePlatform);
            MyCoords = new Vec2F(1f / 40f * (float)PlatformCoords.Y,22f/23f - (1f / 23f * (float)PlatformCoords.X)+1f / 23f);
            
            PickedUp = false;
            Delivered = false;
            
            shape = new DynamicShape(MyCoords, new Vec2F(0.05f,0.05f));
            Entity = new Entity(shape,new DIKUArcade.Graphics.Image(Path.Combine("Assets","Images","CustomerStandLeft.png")));
        }
    }
}