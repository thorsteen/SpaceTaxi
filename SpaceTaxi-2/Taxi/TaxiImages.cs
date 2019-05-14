using System.IO;
using DIKUArcade.Graphics;

namespace SpaceTaxi_2.Taxi {
    public static class TaxiImages {
        
        public static Image TaxiThrustBackRight() {
            Image newImage = new Image(Path.Combine("Assets","Images", "Taxi_Thrust_Back_Right.png"));
            return newImage;
        }

        public static Image TaxiThrustBack() {
            Image newImage = new Image(Path.Combine("Assets","Images","Taxi_Thrust_Back.png"));
            return newImage;
        }

        public static Image TaxiThrustBottomBackRight() {
            Image newImage = new Image(Path.Combine("Assets","Images","Taxi_Thrust_Bottom_Back_Right.png"));
            return newImage;
        }

        public static Image TaxiThrustBottomBack() {
            Image newImage = new Image(Path.Combine("Assets","Images","Taxi_Thrust_Bottom_Back.png"));
            return newImage;
        }

        public static Image TaxiThrustBottomRight() {
            Image newImage = new Image(Path.Combine("Assets","Images","Taxi_Thrust_Bottom_Right.png"));
            return newImage;
        }

        public static Image TaxiThrustBottom() {
            Image newImage = new Image(Path.Combine("Assets","Images","Taxi_Thrust_Bottom.png"));
            return newImage;
        }

        public static Image TaxiThrustNoneRight() {
            Image newImage = new Image(Path.Combine("Assets","Images","Taxi_Thrust_None_Right.png"));
            return newImage;
        }

        public static Image TaxiThrustNone() {
            Image newImage = new Image(Path.Combine("Assets","Images","Taxi_Thrust_None.png"));
            return newImage;
        }
    }
}