using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_3 {
    public class Customer {
        private DynamicShape shape;
        private Image customerLeft;
        private Image customerMoveLeft;
        private Image customerRight;
        private Image customerMoveRight;
        public List<Customer> CustomerList;
        public List<Image> CustomerLeftStride;
        public List<Image> CustomerRightStride;
        private AnimationContainer walking;
        private int walkingLenght = 200;
        public Entity Entity { get; private set; }

        public string name;
        public int secondsUntilSpawn;
        public char homePlatform;
        public string destinationPlatform;
        public int dropOffTimeLimit;
        public int scoreForDelivery;
        
        public Customer(string Name, int SecondsUntilSpawn, char HomePlatform, string DestinationPlatform, int DropoffTimeLimit, int ScoreForDelivery) {
            name = Name;
            secondsUntilSpawn = SecondsUntilSpawn;
            homePlatform = HomePlatform;
            destinationPlatform = DestinationPlatform;
            dropOffTimeLimit = DropoffTimeLimit;
            scoreForDelivery = ScoreForDelivery;
            
            
            shape = new DynamicShape(new Vec2F(), new Vec2F());
            customerLeft = new Image(Path.Combine("Assets", "Images", "CustomerStandLeft.png"));
            customerMoveLeft = new Image(Path.Combine("Assets","Images","CustomerWalkLeft.png"));
            customerRight = new Image(Path.Combine("Assets", "Images", "CustomerStandRight.png"));
            customerMoveRight = new Image(Path.Combine("Assets", "Images", "CustomerWalkRight.png"));
            CustomerList = new List<Customer>();
            CustomerLeftStride = ImageStride.CreateStrides(2, Path.Combine("Assets","Images",
            "CustomerWalkLeft.png"));
            CustomerLeftStride = ImageStride.CreateStrides(2, Path.Combine("Assets","Images",
                "CustomerWalkRight.png"));
            walking = new AnimationContainer(200);
            Entity = new Entity(shape,new DIKUArcade.Graphics.Image(Path.Combine("Assets","Images","CustomerStandLeft.png")));

        }
    }
}