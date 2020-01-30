using ArkanoidGame.Interfaces;
using System;
using System.Drawing;


namespace ArkanoidGame
{
    class Platform
    {
        class Coordinates : ICoordinates
        {
            #region fields
            public int X_1 { set; private get; }
            public int X_Size { set; private get; } = 33;
            public int Y_1 { set; private get; }
            public int Y_Size { set; private get; } = 17;
            #endregion

            public void Deconstruct(out int X_1, out int Y_1, out int X_Size, out int Y_Size)
            {
                #region
                X_1 = this.X_1;
                Y_1 = this.Y_1;
                X_Size = this.X_Size;
                Y_Size = this.Y_Size;
                #endregion
            }
        }

        #region fields
        public const int CountOfPlatforms = 21;
        public Image PlatformImage { get; } = new Bitmap(@"Pictures\platforms.png");
        private readonly ICoordinates[] PlatformCoordinates;
        #endregion
        public Platform()
        {
            #region
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

            PlatformCoordinates[9] = new Coordinates() { X_1 = 108, Y_1 = 0 };
            PlatformCoordinates[10] = new Coordinates() { X_1 = 108, Y_1 = 20 };
            PlatformCoordinates[11] = new Coordinates() { X_1 = 108, Y_1 = 40 };

            PlatformCoordinates[12] = new Coordinates() { X_1 = 144, Y_1 = 0 };
            PlatformCoordinates[13] = new Coordinates() { X_1 = 144, Y_1 = 20 };
            PlatformCoordinates[14] = new Coordinates() { X_1 = 144, Y_1 = 40 };

            PlatformCoordinates[15] = new Coordinates() { X_1 = 180, Y_1 = 0 };
            PlatformCoordinates[16] = new Coordinates() { X_1 = 180, Y_1 = 20 };
            PlatformCoordinates[17] = new Coordinates() { X_1 = 180, Y_1 = 40 };

            PlatformCoordinates[18] = new Coordinates() { X_1 = 216, Y_1 = 0 };
            PlatformCoordinates[19] = new Coordinates() { X_1 = 216, Y_1 = 20 };
            PlatformCoordinates[20] = new Coordinates() { X_1 = 216, Y_1 = 40 };

            #endregion
        }
                    
        public ICoordinates this[int i]
        {
            #region
            get
            {
                if (i < 0 || i >= CountOfPlatforms) throw new IndexOutOfRangeException();

                return PlatformCoordinates[i];
            }
            #endregion
        }
    }
}
