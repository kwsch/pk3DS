using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using pk3DS.Properties;

namespace pk3DS.Subforms
{
    public partial class MapPermView : Form
    {
        public MapPermView()
        {
            InitializeComponent();
        }

        private ToolTip mapCoord = new ToolTip();
        public int mapScale = -1;
        public int DrawMap = -1;
        public void drawMap(int Map)
        {
            DrawMap = Map;
            PB_Map.Image = (CHK_AutoDraw.Checked) ? getMapImage() : null;
        }
        public Image getMapImage(bool crop = false, bool entity = true)
        {
            // Load MM
            byte[][] MM = CTR.mini.unpackMini(File.ReadAllBytes(OWSE.MapMatrixes[DrawMap]), "MM");
            var mm = new MapMatrix(MM[0]);

            // Load GR TileMaps
            for (int i = 0; i < mm.EntryList.Length; i++)
            {
                if (mm.EntryList[i] == 0xFFFF) // Mystery Zone
                    continue;
                byte[][] GR = CTR.mini.unpackMini(File.ReadAllBytes(OWSE.MapGRs[mm.EntryList[i]]), "GR");
                mm.Entries[i] = new MapMatrix.Entry(GR[0]);
            }
            mapScale = (int)NUD_Scale.Value;
            Image img = mm.Preview(mapScale, (int)NUD_Flavor.Value);

            if (entity && mapScale == 8)
            {
                // Overlay every... overworld entity
                foreach (var f in OWSE.CurrentZone.Entities.Furniture)
                    try { Util.LayerImage(img, Resources.F, f.X * mapScale, f.Y * mapScale, 0.8); } catch {}
                foreach (var f in OWSE.CurrentZone.Entities.NPCs)
                    try { Util.LayerImage(img, Resources.N, f.X * mapScale, f.Y * mapScale, 0.8); } catch {}
                foreach (var f in OWSE.CurrentZone.Entities.Warps)
                    try { Util.LayerImage(img, Resources.W, (int)(f.pX * mapScale), (int)(f.pY * mapScale), 0.8); } catch {}
                foreach (var f in OWSE.CurrentZone.Entities.Triggers1)
                    try { Util.LayerImage(img, Resources.T1, f.X * mapScale, f.Y * mapScale, 0.8); } catch {}
                foreach (var f in OWSE.CurrentZone.Entities.Triggers2)
                    try { Util.LayerImage(img, Resources.T2, f.X * mapScale, f.Y * mapScale, 0.8); } catch { }
            }
            if (crop)
                img = Util.TrimBitmap((Bitmap)img);
            OWSE.mm = mm;
            return img;
        }

        // UI
        private void hoverMap(object sender, MouseEventArgs e)
        {
            if (mapScale < 0)
                return;

            if (PB_Map.Image == null)
                return;

            int X = e.X / (mapScale);
            int Y = e.Y / (mapScale);

            int entryX = X/40;
            int entryY = Y/40;

            int entry = entryY*(PB_Map.Image.Width/40/mapScale) + entryX;
            int epX = X%40;
            int epY = Y%40;
            int tile = epY * 40 + epX;
            try
            {
                var tileVal = (OWSE.mm.Entries[entry] == null)
                    ? "No Tile"
                    : OWSE.mm.Entries[entry].Tiles[tile].ToString("X8");

                L_MapCoord.Text = String.Format("V:0x{3}{2}X:{0,3}  Y:{1,3}", X, Y, Environment.NewLine, tileVal);
            }
            catch { } 
        }
        private void B_Redraw_Click(object sender, EventArgs e)
        {
            if (DrawMap != -1)
                PB_Map.Image = getMapImage();
        }

        private void MapPermView_FormClosing(object sender, FormClosingEventArgs e)
        {
            CHK_AutoDraw.Checked = false;
            Hide();
            e.Cancel = true;
        }
    }
}
