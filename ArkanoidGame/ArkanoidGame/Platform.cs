using System;
using System.Drawing;
namespace ArkanoidGame
{
    class Coordinates
    {
        public int X_1 { set; get; }
        public int X_Size { set; get; } = 33;
        public int Y_1 { set; get; }
        public int Y_Size { set; get; } = 17;
    }
    class Platform
    {
        public const int CountOfPlatforms = 9;
        public Image PlatformImage { get; } = new Bitmap(@"Pictures\platforms.png");
        private Coordinates[] PlatformCoordinates;

        public Platform()
        {
            PlatformCoordinates = new Coordinates[CountOfPlatforms];
            PlatformCoordinates[0] = new Coordinates() { X_1 = 0, Y_1 = 0};
            PlatformCoordinates[1] = new Coordinates() { X_1 = 0, Y_1 = 20};
            PlatformCoordinates[2] = new Coordinates() { X_1 = 0, Y_1 = 40};

            PlatformCoordinates[3] = new Coordinates() { X_1 = 36, Y_1 = 0};
            PlatformCoordinates[4] = new Coordinates() { X_1 = 36, Y_1 = 20};
            PlatformCoordinates[5] = new Coordinates() { X_1 = 36, Y_1 = 40};

            PlatformCoordinates[6] = new Coordinates() { X_1 = 72, Y_1 = 0};
            PlatformCoordinates[7] = new Coordinates() { X_1 = 72, Y_1 = 20};
            PlatformCoordinates[8] = new Coordinates() { X_1 = 72, Y_1 = 40};
        }
                    
        public Coordinates this[int i]
        {
            get 
            {
                if (i < 0 || i >= CountOfPlatforms) throw new IndexOutOfRangeException();

                return PlatformCoordinates[i];
            }
        }
    }
}
