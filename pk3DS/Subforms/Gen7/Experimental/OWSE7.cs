using System.Windows.Forms;

namespace pk3DS
{
    public partial class OWSE7 : Form
    {
        private readonly lzGARCFile EncounterData;
        private readonly lzGARCFile WorldData;
        private readonly lzGARCFile ZoneData;
        public OWSE7(lzGARCFile ed, lzGARCFile zd, lzGARCFile wd)
        {
            EncounterData = ed;
            ZoneData = zd;
            WorldData = wd;


            locationList = Main.getText(TextName.metlist_000000);
            locationList = SMWE.getGoodLocationList(locationList);
        }
        private string[] locationList;
    }
}
