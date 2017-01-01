using System;

namespace pk3DS
{
    public class ZoneData7
    {
        public const int SIZE = 0x54;
        private readonly byte[] Data;

        public int WorldIndex;
        public int AreaIndex;
        public string Name { get; private set; }
        public string LocationName { get; private set; }

        public ZoneData7(byte[] data)
        {
            Data = (byte[])(data ?? new byte[SIZE]).Clone();
        }
        public ZoneData7(byte[] data, int index)
        {
            Data = new byte[SIZE];
            Array.Copy(data, index * SIZE, Data, 0, SIZE);
        }

        // ZoneData Attributes
        private int ParentMap { get { return BitConverter.ToInt32(Data, 0x1C); } set { BitConverter.GetBytes(value).CopyTo(Data, 0x1C);} }
        
        // Info Tracking
        public void setName(string[] locationList, int index)
        {
            LocationName = locationList[ParentMap];
            Name = $"{index:000} - {LocationName}";
        }
    }
}
