using static pk3DS.Core.CTR.Images.XLIMUtil;

namespace pk3DS.Core.CTR
{
    public class XLIMOrienter
    {
        private readonly XLIMOrientation _orientation;

        public uint Width { get; }
        public uint Height { get; }
        public uint PanelsPerWidth { get; }

        public XLIMOrienter(int width, int height, XLIMOrientation orientation)
        {
            Width = (uint)NextLargestPow2(GreatestCommonMultiple(width, 8));
            Height = (uint)NextLargestPow2(GreatestCommonMultiple(height, 8));

            uint stride = orientation == XLIMOrientation.None ? Width : Height;
            PanelsPerWidth = (uint)GreatestCommonMultiple((int)stride, 8) / 8;

            _orientation = orientation;
        }

        public Coordinate Get(uint i)
        {
            DecimalToCartesian(i & 0x3F, out uint x, out uint y);

            // Shift Tile Coordinate into Tilemap
            var tile = i >> 6;
            x |= (tile % PanelsPerWidth) << 3;
            y |= (tile / PanelsPerWidth) << 3;

            var coord = new Coordinate(x, y);
            if (_orientation.HasFlagFast(XLIMOrientation.Rotate90))
                coord.Rotate90(Height);
            if (_orientation.HasFlagFast(XLIMOrientation.Transpose))
                coord.Transpose();
            return coord;
        }

        internal static uint C11(uint x)
        {
            x &= 0x55555555;                  // x = -f-e -d-c -b-a -9-8 -7-6 -5-4 -3-2 -1-0
            x = (x ^ (x >> 1)) & 0x33333333; // x = --fe --dc --ba --98 --76 --54 --32 --10
            x = (x ^ (x >> 2)) & 0x0f0f0f0f; // x = ---- fedc ---- ba98 ---- 7654 ---- 3210
            x = (x ^ (x >> 4)) & 0x00ff00ff; // x = ---- ---- fedc ba98 ---- ---- 7654 3210
            x = (x ^ (x >> 8)) & 0x0000ffff; // x = ---- ---- ---- ---- fedc ba98 7654 3210
            return x;
        }

        // Morton Translation

        /// <summary>
        /// Combines X/Y Coordinates to a decimal ordinate.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        internal static uint CartesianToDecimal(uint x, uint y)
        {
            x &= 0x0000ffff;
            y &= 0x0000ffff;
            x |= x << 8;
            y |= y << 8;
            x &= 0x00ff00ff;
            y &= 0x00ff00ff;
            x |= x << 4;
            y |= y << 4;
            x &= 0x0f0f0f0f;
            y &= 0x0f0f0f0f;
            x |= x << 2;
            y |= y << 2;
            x &= 0x33333333;
            y &= 0x33333333;
            x |= x << 1;
            y |= y << 1;
            x &= 0x55555555;
            y &= 0x55555555;
            return x | (y << 1);
        }

        /// <summary>
        /// Decimal Ordinate In to X / Y Coordinate Out
        /// </summary>
        /// <param name="d">Loop integer which will be decoded to X/Y</param>
        /// <param name="x">Output X coordinate</param>
        /// <param name="y">Output Y coordinate</param>
        internal static void DecimalToCartesian(uint d, out uint x, out uint y)
        {
            x = d;
            y = x >> 1;
            x &= 0x55555555;
            y &= 0x55555555;
            x |= x >> 1;
            y |= y >> 1;
            x &= 0x33333333;
            y &= 0x33333333;
            x |= x >> 2;
            y |= y >> 2;
            x &= 0x0f0f0f0f;
            y &= 0x0f0f0f0f;
            x |= x >> 4;
            y |= y >> 4;
            x &= 0x00ff00ff;
            y &= 0x00ff00ff;
            x |= x >> 8;
            y |= y >> 8;
            x &= 0x0000ffff;
            y &= 0x0000ffff;
        }
    }
}