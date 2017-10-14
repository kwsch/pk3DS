using System;

namespace pk3DS.Core.Structures
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
        public int ParentMap {
            get => BitConverter.ToInt32(Data, 0x1C);
            set => BitConverter.GetBytes(value).CopyTo(Data, 0x1C);
        }
        
        // Info Tracking
        public void SetZoneName(string[] locationList, int index)
        {
            LocationName = locationList[ParentMap];
            Name = $"{index:000} - {LocationName}";
        }


        public static ZoneData7[] GetArray(byte[] zoneData)
        {
            ZoneData7[] zd = new ZoneData7[zoneData.Length / SIZE];
            for (int i = 0; i < zd.Length; i++)
                zd[i] = new ZoneData7(zoneData, i);
            return zd;
        }

        public static ZoneData7[] GetZoneData7Array(byte[] zoneData, byte[] worldData, string[] locationList, byte[][] worlds)
        {
            var zones = ZoneData7.GetArray(zoneData);
            for (int i = 0; i < zones.Length; i++)
            {
                zones[i].WorldIndex = BitConverter.ToUInt16(worldData, i * 0x2);
                zones[i].SetZoneName(locationList, i);

                var world = worlds[zones[i].WorldIndex];
                var mappingOffset = BitConverter.ToInt32(world, 0x8);
                for (var ofs = mappingOffset; ofs < world.Length; ofs += 4)
                {
                    if (BitConverter.ToUInt16(world, ofs) != i)
                        continue;
                    zones[i].AreaIndex = BitConverter.ToUInt16(world, ofs + 2);
                    break;
                }
            }
            return zones;
        }
    }
}
