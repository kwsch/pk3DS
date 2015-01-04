using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace pk3DS
{
    public partial class RSTE : Form
    {
        public RSTE(bool rom_oras, string[] trdata, string[] trpoke)
        {
            oras = rom_oras;
            trdatapaths = trdata;
            trpokepaths = trpoke;
            abilitylist = Main.getText((oras) ? 37 : 34);
            movelist = Main.getText((oras) ? 14 : 13);
            itemlist = Main.getText((oras) ? 114 : 96);
            specieslist = Main.getText((oras) ? 98 : 80);
            types = Main.getText((oras) ? 18 : 17);
            forms = Main.getText((oras) ? 5 : 5);
            trName = Main.getText((oras) ? 22 : 21);
            trClass = Main.getText((oras) ? 21 : 20);
            InitializeComponent();
            // String Fetching
            #region Combo Box Arrays
            trpk_pkm = new ComboBox[] 
            {
                CB_Pokemon_1_Pokemon,
                CB_Pokemon_2_Pokemon,
                CB_Pokemon_3_Pokemon,
                CB_Pokemon_4_Pokemon,
                CB_Pokemon_5_Pokemon,
                CB_Pokemon_6_Pokemon,
            };
            trpk_lvl = new ComboBox[] 
            { 
                CB_Pokemon_1_Level,
                CB_Pokemon_2_Level,
                CB_Pokemon_3_Level,
                CB_Pokemon_4_Level,
                CB_Pokemon_5_Level,
                CB_Pokemon_6_Level,
            };
            trpk_item = new ComboBox[] 
            { 
                CB_Pokemon_1_Item,
                CB_Pokemon_2_Item,
                CB_Pokemon_3_Item,
                CB_Pokemon_4_Item,
                CB_Pokemon_5_Item,
                CB_Pokemon_6_Item,
            };
            trpk_abil = new ComboBox[] 
            { 
                CB_Pokemon_1_Ability,
                CB_Pokemon_2_Ability,
                CB_Pokemon_3_Ability,
                CB_Pokemon_4_Ability,
                CB_Pokemon_5_Ability,
                CB_Pokemon_6_Ability,
            };
            trpk_m1 = new ComboBox[]
            {
                CB_Pokemon_1_Move_1,
                CB_Pokemon_2_Move_1,
                CB_Pokemon_3_Move_1,
                CB_Pokemon_4_Move_1,
                CB_Pokemon_5_Move_1,
                CB_Pokemon_6_Move_1,
            };
            trpk_m2 = new ComboBox[]
            {
                CB_Pokemon_1_Move_2,
                CB_Pokemon_2_Move_2,
                CB_Pokemon_3_Move_2,
                CB_Pokemon_4_Move_2,
                CB_Pokemon_5_Move_2,
                CB_Pokemon_6_Move_2,
            };
            trpk_m3 = new ComboBox[]
            {
                CB_Pokemon_1_Move_3,
                CB_Pokemon_2_Move_3,
                CB_Pokemon_3_Move_3,
                CB_Pokemon_4_Move_3,
                CB_Pokemon_5_Move_3,
                CB_Pokemon_6_Move_3,
            };
            trpk_m4 = new ComboBox[]
            {
                CB_Pokemon_1_Move_4,
                CB_Pokemon_2_Move_4,
                CB_Pokemon_3_Move_4,
                CB_Pokemon_4_Move_4,
                CB_Pokemon_5_Move_4,
                CB_Pokemon_6_Move_4,
            };
            trpk_IV = new ComboBox[]
            {
                CB_Pokemon_1_IVs,
                CB_Pokemon_2_IVs,
                CB_Pokemon_3_IVs,
                CB_Pokemon_4_IVs,
                CB_Pokemon_5_IVs,
                CB_Pokemon_6_IVs,
            };
            trpk_form = new ComboBox[]
            {
                CB_Pokemon_1_Form,
                CB_Pokemon_2_Form,
                CB_Pokemon_3_Form,
                CB_Pokemon_4_Form,
                CB_Pokemon_5_Form,
                CB_Pokemon_6_Form,
            };
            trpk_gender = new ComboBox[]
            {
                CB_Pokemon_1_Gender,
                CB_Pokemon_2_Gender,
                CB_Pokemon_3_Gender,
                CB_Pokemon_4_Gender,
                CB_Pokemon_5_Gender,
                CB_Pokemon_6_Gender,
            };
            #endregion
            Setup();
        }
        internal static Random rand = new Random();
        internal static uint rnd32()
        {
            return (uint)(rand.Next(1 << 30)) << 2 | (uint)(rand.Next(1 << 2));
        }
        bool oras = false;
        bool start = true;
        bool loading = true;
        byte[][] personal;
        int index = -1;
        #region Global Variables
        private ComboBox[] trpk_pkm;
        private ComboBox[] trpk_lvl;
        private ComboBox[] trpk_item;
        private ComboBox[] trpk_abil;
        private ComboBox[] trpk_m1;
        private ComboBox[] trpk_m2;
        private ComboBox[] trpk_m3;
        private ComboBox[] trpk_m4;
        private ComboBox[] trpk_IV;
        private ComboBox[] trpk_form;
        private ComboBox[] trpk_gender;
        private string[] abilitylist, movelist, itemlist, specieslist, types, forms;
        private string[] trName, trClass;
        #endregion

        // Form Loading
        public void setForms(int species, ComboBox cb)
        {
            // Form Tables
            // 
            var form_unown = new[] {
                    new { Text = "A", Value = 0 },
                    new { Text = "B", Value = 1 },
                    new { Text = "C", Value = 2 },
                    new { Text = "D", Value = 3 },
                    new { Text = "E", Value = 4 },
                    new { Text = "F", Value = 5 },
                    new { Text = "G", Value = 6 },
                    new { Text = "H", Value = 7 },
                    new { Text = "I", Value = 8 },
                    new { Text = "J", Value = 9 },
                    new { Text = "K", Value = 10 },
                    new { Text = "L", Value = 11 },
                    new { Text = "M", Value = 12 },
                    new { Text = "N", Value = 13 },
                    new { Text = "O", Value = 14 },
                    new { Text = "P", Value = 15 },
                    new { Text = "Q", Value = 16 },
                    new { Text = "R", Value = 17 },
                    new { Text = "S", Value = 18 },
                    new { Text = "T", Value = 19 },
                    new { Text = "U", Value = 20 },
                    new { Text = "V", Value = 21 },
                    new { Text = "W", Value = 22 },
                    new { Text = "X", Value = 23 },
                    new { Text = "Y", Value = 24 },
                    new { Text = "Z", Value = 25 },
                    new { Text = "!", Value = 26 },
                    new { Text = "?", Value = 27 },
                };
            var form_castform = new[] {
                    new { Text = types[0], Value = 0 }, // Normal
                    new { Text = forms[789], Value = 1 }, // Sunny
                    new { Text = forms[790], Value = 2 }, // Rainy
                    new { Text = forms[791], Value = 3 }, // Snowy
                };
            var form_shellos = new[] {
                    new { Text = forms[422], Value = 0 }, // West
                    new { Text = forms[811], Value = 1 }, // East
                };
            var form_deoxys = new[] {
                    new { Text = types[0], Value = 0 }, // Normal
                    new { Text = forms[802], Value = 1 }, // Attack
                    new { Text = forms[803], Value = 2 }, // Defense
                    new { Text = forms[804], Value = 3 }, // Speed
                };
            var form_burmy = new[] {
                    new { Text = forms[412], Value = 0 }, // Plant
                    new { Text = forms[805], Value = 1 }, // Sandy
                    new { Text = forms[806], Value = 2 }, // Trash
                };
            var form_cherrim = new[] {
                    new { Text = forms[421], Value = 0 }, // Overcast
                    new { Text = forms[809], Value = 1 }, // Sunshine
                };
            var form_rotom = new[] {
                    new { Text = types[0], Value = 0 }, // Normal
                    new { Text = forms[817], Value = 1 }, // Heat
                    new { Text = forms[818], Value = 2 }, // Wash
                    new { Text = forms[819], Value = 3 }, // Frost
                    new { Text = forms[820], Value = 4 }, // Fan
                    new { Text = forms[821], Value = 5 }, // Mow
                };
            var form_giratina = new[] {
                    new { Text = forms[487], Value = 0 }, // Altered
                    new { Text = forms[822], Value = 1 }, // Origin
                };
            var form_shaymin = new[] {
                    new { Text = forms[492], Value = 0 }, // Land
                    new { Text = forms[823], Value = 1 }, // Sky
                };
            var form_arceus = new[] {
                    new { Text = types[0], Value = 0 }, // Normal
                    new { Text = types[1], Value = 1 }, // Fighting
                    new { Text = types[2], Value = 2 }, // Flying
                    new { Text = types[3], Value = 3 }, // Poison
                    new { Text = types[4], Value = 4 }, // etc
                    new { Text = types[5], Value = 5 },
                    new { Text = types[6], Value = 6 },
                    new { Text = types[7], Value = 7 },
                    new { Text = types[8], Value = 8 },
                    new { Text = types[9], Value = 9 },
                    new { Text = types[10], Value = 10 },
                    new { Text = types[11], Value = 11 },
                    new { Text = types[12], Value = 12 },
                    new { Text = types[13], Value = 13 },
                    new { Text = types[14], Value = 14 },
                    new { Text = types[15], Value = 15 },
                    new { Text = types[16], Value = 16 },
                    new { Text = types[17], Value = 17 },
                };
            var form_basculin = new[] {
                    new { Text = forms[550], Value = 0 }, // Red
                    new { Text = forms[842], Value = 1 }, // Blue
                };
            var form_darmanitan = new[] {
                    new { Text = forms[555], Value = 0 }, // Standard
                    new { Text = forms[843], Value = 1 }, // Zen
                };
            var form_deerling = new[] {
                    new { Text = forms[585], Value = 0 }, // Spring
                    new { Text = forms[844], Value = 1 }, // Summer
                    new { Text = forms[845], Value = 2 }, // Autumn
                    new { Text = forms[846], Value = 3 }, // Winter
                };
            var form_gender = new[] {
                    new { Text = "♂", Value = 0 }, // Male
                    new { Text = "♀", Value = 1 }, // Female
                };
            var form_therian = new[] {
                    new { Text = forms[641], Value = 0 }, // Incarnate
                    new { Text = forms[852], Value = 1 }, // Therian
                };
            var form_kyurem = new[] {
                    new { Text = types[0], Value = 0 }, // Normal
                    new { Text = forms[853], Value = 1 }, // White
                    new { Text = forms[854], Value = 2 }, // Black
                };
            var form_keldeo = new[] {
                    new { Text = forms[647], Value = 0 }, // Ordinary
                    new { Text = forms[855], Value = 1 }, // Resolute
                };
            var form_meloetta = new[] {
                    new { Text = forms[648], Value = 0 }, // Aria
                    new { Text = forms[856], Value = 1 }, // Pirouette
                };
            var form_genesect = new[] {
                    new { Text = types[0], Value = 0 }, // Normal
                    new { Text = types[10], Value = 1 }, // Douse
                    new { Text = types[12], Value = 2 }, // Shock
                    new { Text = types[9], Value = 3 }, // Burn
                    new { Text = types[14], Value = 4 }, // Chill
                };
            var form_flabebe = new[] {
                    new { Text = forms[669], Value = 0 }, // Red
                    new { Text = forms[884], Value = 1 }, // Yellow
                    new { Text = forms[885], Value = 2 }, // Orange
                    new { Text = forms[886], Value = 3 }, // Blue
                    new { Text = forms[887], Value = 4 }, // White
                };
            var form_floette = new[] {
                    new { Text = forms[669], Value = 0 }, // Red
                    new { Text = forms[884], Value = 1 }, // Yellow
                    new { Text = forms[885], Value = 2 }, // Orange
                    new { Text = forms[886], Value = 3 }, // Blue
                    new { Text = forms[887], Value = 4 }, // White
                    new { Text = forms[888], Value = 5 }, // Eternal
                };
            var form_furfrou = new[] {
                    new { Text = forms[676], Value = 0 }, // Natural
                    new { Text = forms[893], Value = 1 }, // Heart
                    new { Text = forms[894], Value = 2 }, // Star
                    new { Text = forms[895], Value = 3 }, // Diamond
                    new { Text = forms[896], Value = 4 }, // Deputante
                    new { Text = forms[897], Value = 5 }, // Matron
                    new { Text = forms[898], Value = 6 }, // Dandy
                    new { Text = forms[899], Value = 7 }, // La Reine
                    new { Text = forms[900], Value = 8 }, // Kabuki 
                    new { Text = forms[901], Value = 9 }, // Pharaoh
                };
            var form_aegislash = new[] {
                    new { Text = forms[681], Value = 0 }, // Shield
                    new { Text = forms[903], Value = 1 }, // Blade
                };
            var form_butterfly = new[] {
                    new { Text = forms[666], Value = 0 }, // Icy Snow
                    new { Text = forms[861], Value = 1 }, // Polar
                    new { Text = forms[862], Value = 2 }, // Tundra
                    new { Text = forms[863], Value = 3 }, // Continental 
                    new { Text = forms[864], Value = 4 }, // Garden
                    new { Text = forms[865], Value = 5 }, // Elegant
                    new { Text = forms[866], Value = 6 }, // Meadow
                    new { Text = forms[867], Value = 7 }, // Modern 
                    new { Text = forms[868], Value = 8 }, // Marine
                    new { Text = forms[869], Value = 9 }, // Archipelago
                    new { Text = forms[870], Value = 10 }, // High-Plains
                    new { Text = forms[871], Value = 11 }, // Sandstorm
                    new { Text = forms[872], Value = 12 }, // River
                    new { Text = forms[873], Value = 13 }, // Monsoon
                    new { Text = forms[874], Value = 14 }, // Savannah 
                    new { Text = forms[875], Value = 15 }, // Sun
                    new { Text = forms[876], Value = 16 }, // Ocean
                    new { Text = forms[877], Value = 17 }, // Jungle
                    new { Text = forms[878], Value = 18 }, // Fancy
                    new { Text = forms[879], Value = 19 }, // Poké Ball
                };
            var form_list = new[] {
                    new { Text = "", Value = 0}, // None
                };
            var form_pump = new[] {
                    new { Text = forms[904], Value = 0 }, // Small
                    new { Text = forms[710], Value = 1 }, // Average
                    new { Text = forms[905], Value = 2 }, // Large
                    new { Text = forms[907], Value = 3 }, // Super
                };
            var form_mega = new[] {
                    new { Text = types[0], Value = 0}, // Normal
                    new { Text = forms[723], Value = 1}, // Mega
                };
            var form_megaxy = new[] {
                    new { Text = types[0], Value = 0}, // Normal
                    new { Text = forms[724], Value = 1}, // Mega X
                    new { Text = forms[725], Value = 2}, // Mega Y
                };

            var form_primal = new[] {
                    new { Text = types[0], Value = 0},
                    new { Text = forms[800], Value = 1},
                };
            var form_hoopa = new[] {
                    new { Text = types[0], Value = 0},
                    new { Text = forms[912], Value = 1},
                };
            var form_pikachu = new[] {
                    new { Text = types[0], Value = 0}, // Normal
                    new { Text = forms[729], Value = 1}, // Rockstar
                    new { Text = forms[730], Value = 2}, // Belle
                    new { Text = forms[731], Value = 3}, // Pop
                    new { Text = forms[732], Value = 4}, // PhD
                    new { Text = forms[733], Value = 5}, // Libre
                    new { Text = forms[734], Value = 6}, // Cosplay
                };

            cb.DataSource = form_list;
            cb.DisplayMember = "Text";
            cb.ValueMember = "Value";

            // Mega List
            int[] mspec = {     // XY
                                   003, 009, 065, 094, 115, 127, 130, 142, 181, 212, 214, 229, 248, 257, 282, 303, 306, 308, 310, 354, 359, 380, 381, 445, 448, 460, 
                                // ORAS
                                015, 018, 080, 208, 254, 260, 302, 319, 323, 334, 362, 373, 376, 384, 428, 475, 531, 719,
                          };
            for (int i = 0; i < mspec.Length; i++)
            {
                if (mspec[i] == species)
                {
                    cb.DataSource = form_mega;
                    cb.Enabled = true; // Mega Form Selection
                    return;
                }
            }

            // MegaXY List
            if ((species == 6) || (species == 150))
            {
                cb.DataSource = form_megaxy;
                cb.Enabled = true; // Mega Form Selection
                return;
            }

            // Regular Form List
            if (species == 025) { form_list = form_pikachu; }
            else if (species == 201) { form_list = form_unown; }
            else if (species == 351) { form_list = form_castform; }
            else if (species == 386) { form_list = form_deoxys; }
            else if (species == 421) { form_list = form_cherrim; }
            else if (species == 479) { form_list = form_rotom; }
            else if (species == 487) { form_list = form_giratina; }
            else if (species == 492) { form_list = form_shaymin; }
            else if (species == 493) { form_list = form_arceus; }
            else if (species == 550) { form_list = form_basculin; }
            else if (species == 555) { form_list = form_darmanitan; }
            else if (species == 646) { form_list = form_kyurem; }
            else if (species == 647) { form_list = form_keldeo; }
            else if (species == 648) { form_list = form_meloetta; }
            else if (species == 649) { form_list = form_genesect; }
            else if (species == 676) { form_list = form_furfrou; }
            else if (species == 681) { form_list = form_aegislash; }
            else if (species == 670) { form_list = form_floette; }

            else if ((species == 669) || (species == 671)) { form_list = form_flabebe; }
            else if ((species == 412) || (species == 413)) { form_list = form_burmy; }
            else if ((species == 422) || (species == 423)) { form_list = form_shellos; }
            else if ((species == 585) || (species == 586)) { form_list = form_deerling; }
            else if ((species == 710) || (species == 711)) { form_list = form_pump; }

            else if ((species == 666) || (species == 665) || (species == 664)) { form_list = form_butterfly; }
            else if ((species == 592) || (species == 593) || (species == 678)) { form_list = form_gender; }
            else if ((species == 641) || (species == 642) || (species == 645)) { form_list = form_therian; }

            // ORAS
            else if (species == 382 || species == 383) { form_list = form_primal; }
            else if (species == 720) { form_list = form_hoopa; }

            else
            {
                cb.Enabled = false;
                return;
            };

            cb.DataSource = form_list;
            cb.Enabled = true;
        }
        // Ability Loading
        private void refreshFormAbility(object sender, EventArgs e)
        {
            int i = Array.IndexOf(trpk_form, sender as ComboBox);
            refreshPKMSlotAbility(i);
        }
        private void refreshSpeciesAbility(object sender, EventArgs e)
        {
            int i = Array.IndexOf(trpk_pkm, sender as ComboBox);
            setForms(trpk_pkm[i].SelectedIndex, trpk_form[i]);
            refreshPKMSlotAbility(i);
        }
        private void refreshPKMSlotAbility(int slot)
        {
            int species = trpk_pkm[slot].SelectedIndex;
            uint[] abils = { 0, 0, 0 };
            int formnum = trpk_form[slot].SelectedIndex;

            if (formnum == 0) { }
            // Previous Games
            else if (species == 492 && formnum == 1) { species = 727; } // Shaymin Sky
            else if (species == 487 && formnum == 1) { species = 728; } // Giratina-O
            else if (species == 550 && formnum == 1) { species = 738; } // Basculin Blue
            else if (species == 646 && formnum == 1) { species = 741; } // Kyurem White
            else if (species == 646 && formnum == 2) { species = 742; } // Kyurem Black
            else if (species == 641 && formnum == 1) { species = 744; } // Tornadus-T
            else if (species == 642 && formnum == 1) { species = 745; } // Thundurus-T
            else if (species == 645 && formnum == 1) { species = 746; } // Landorus-T

            // XY
            else if (species == 678 && formnum == 1) { species = 748; } // Meowstic Female
            else if (species == 094 && formnum == 1) { species = 747; } // Mega Gengar
            else if (species == 282 && formnum == 1) { species = 758; } // Mega Gardevoir
            else if (species == 181 && formnum == 1) { species = 759; } // Mega Ampharos
            else if (species == 003 && formnum == 1) { species = 760; } // Mega Venusaur
            else if (species == 006 && formnum == 1) { species = 761; } // Mega Charizard X
            else if (species == 006 && formnum == 2) { species = 762; } // Mega Charizard Y
            else if (species == 150 && formnum == 1) { species = 763; } // Mega MewtwoX
            else if (species == 150 && formnum == 2) { species = 764; } // Mega MewtwoY
            else if (species == 257 && formnum == 1) { species = 765; } // Mega Blaziken
            else if (species == 308 && formnum == 1) { species = 766; } // Mega Medicham
            else if (species == 229 && formnum == 1) { species = 767; } // Mega Houndoom
            else if (species == 306 && formnum == 1) { species = 768; } // Mega Aggron
            else if (species == 354 && formnum == 1) { species = 769; } // Mega Banette
            else if (species == 248 && formnum == 1) { species = 770; } // Mega Tyranitar
            else if (species == 212 && formnum == 1) { species = 771; } // Mega Scizor
            else if (species == 127 && formnum == 1) { species = 772; } // Mega Pinsir
            else if (species == 142 && formnum == 1) { species = 773; } // Mega Aerodactyl
            else if (species == 448 && formnum == 1) { species = 774; } // Mega Lucario
            else if (species == 460 && formnum == 1) { species = 775; } // Mega Abomasnow
            else if (species == 009 && formnum == 1) { species = 777; } // Mega Blastoise
            else if (species == 115 && formnum == 1) { species = 778; } // Mega Kangaskhan
            else if (species == 130 && formnum == 1) { species = 779; } // Mega Gyarados
            else if (species == 359 && formnum == 1) { species = 780; } // Mega Absol
            else if (species == 065 && formnum == 1) { species = 781; } // Mega Alakazam
            else if (species == 214 && formnum == 1) { species = 782; } // Mega Heracross
            else if (species == 303 && formnum == 1) { species = 783; } // Mega Mawile
            else if (species == 310 && formnum == 1) { species = 784; } // Mega Manectric
            else if (species == 445 && formnum == 1) { species = 785; } // Mega Garchomp
            else if (species == 381 && formnum == 1) { species = 786; } // Mega Latios
            else if (species == 380 && formnum == 1) { species = 787; } // Mega Latias

            // ORAS
            else if (species == 382 && formnum == 1) { species = 812; } // Primal Kyogre
            else if (species == 383 && formnum == 1) { species = 813; } // Primal Groudon
            else if (species == 720 && formnum == 1) { species = 821; } // Hoopa Unbound
            else if (species == 015 && formnum == 1) { species = 825; } // Mega Beedrill
            else if (species == 018 && formnum == 1) { species = 808; } // Mega Pidgeot
            else if (species == 080 && formnum == 1) { species = 806; } // Mega Slowbro
            else if (species == 208 && formnum == 1) { species = 807; } // Mega Steelix
            else if (species == 254 && formnum == 1) { species = 800; } // Mega Sceptile
            else if (species == 260 && formnum == 1) { species = 799; } // Mega Swampert
            else if (species == 302 && formnum == 1) { species = 801; } // Mega Sableye
            else if (species == 319 && formnum == 1) { species = 805; } // Mega Sharpedo
            else if (species == 323 && formnum == 1) { species = 822; } // Mega Camerupt
            else if (species == 334 && formnum == 1) { species = 802; } // Mega Altaria
            else if (species == 362 && formnum == 1) { species = 809; } // Mega Glalie
            else if (species == 373 && formnum == 1) { species = 824; } // Mega Salamence
            else if (species == 376 && formnum == 1) { species = 811; } // Mega Metagross
            else if (species == 384 && formnum == 1) { species = 814; } // Mega Rayquaza
            else if (species == 428 && formnum == 1) { species = 823; } // Mega Lopunny
            else if (species == 475 && formnum == 1) { species = 803; } // Mega Gallade
            else if (species == 531 && formnum == 1) { species = 804; } // Mega Audino
            else if (species == 719 && formnum == 1) { species = 810; } // Mega Diancie

            byte[] persdata = File.ReadAllBytes("personal" + Path.DirectorySeparatorChar + species.ToString("000") + ".bin");
            Array.Copy(persdata, 0x18, abils, 0, 3);
            trpk_abil[slot].Items.Clear();
            trpk_abil[slot].Items.Add("Any (1 or 2)");
            trpk_abil[slot].Items.Add(abilitylist[abils[0]] + " (1)");
            trpk_abil[slot].Items.Add(abilitylist[abils[1]] + " (2)");
            trpk_abil[slot].Items.Add(abilitylist[abils[2]] + " (H)");
            trpk_abil[slot].SelectedIndex = 0;
        }
        // Set Loading
        private void changeTrainerType(object sender, EventArgs e)
        {
            if (start || loading) return;
            int pkm = CB_numPokemon.SelectedIndex;
            {
                for (int i = 0; i < 6; i++) // enable all if the pkm exists
                {
                    trpk_pkm[i].Enabled =
                    trpk_gender[i].Enabled =
                    trpk_abil[i].Enabled =
                    trpk_IV[i].Enabled =
                    trpk_lvl[i].Enabled = (i < pkm);

                    trpk_item[i].Enabled = (i < pkm) && (checkBox_Item.Checked);

                    trpk_m1[i].Enabled =
                    trpk_m2[i].Enabled =
                    trpk_m3[i].Enabled =
                    trpk_m4[i].Enabled = (i < pkm) && (checkBox_Moves.Checked);

                    if (!trpk_pkm[i].Enabled)
                    {
                        trpk_pkm[i].SelectedIndex =
                        trpk_gender[i].SelectedIndex =
                        trpk_form[i].SelectedIndex =
                        trpk_abil[i].SelectedIndex =
                        trpk_IV[i].SelectedIndex =
                        trpk_lvl[i].SelectedIndex = 0;
                    }
                    if (!trpk_item[i].Enabled)
                    {
                        trpk_item[i].SelectedIndex = 0;
                    }
                    if (!trpk_m1[i].Enabled)
                    {
                        trpk_m1[i].SelectedIndex =
                        trpk_m2[i].SelectedIndex =
                        trpk_m3[i].SelectedIndex =
                        trpk_m4[i].SelectedIndex = 0;
                    }
                }
                for (int i = pkm; i < 6; i++)
                    trpk_form[i].Enabled = false;
            }
        }

        // Dumping
        private string getTRSummary()
        {
            string toret = "======\n";

            toret += CB_TrainerID.SelectedIndex + " - " + CB_Trainer_Class.Text.Substring(6, CB_Trainer_Class.Text.Length - 6) + " " + CB_TrainerID.Text.Substring(6, CB_TrainerID.Text.Length - 6) + "\n";
            toret += "======\n";
            int pkm = CB_numPokemon.SelectedIndex;
            toret += "Pokemon: " + pkm + "\n";
            for (int i = 0; i < pkm; i++)
            {
                toret += trpk_pkm[i].Text + " (Lv. " + trpk_lvl[i].SelectedIndex + ") ";
                if (trpk_item[i].SelectedIndex > 0)
                    toret += "@" + trpk_item[i].Text;
                if (trpk_abil[i].SelectedIndex != 0)
                {
                    string abil = trpk_abil[i].Text;
                    abil = abil.Substring(0, abil.Length - 4);
                    toret += " (Ability: " + abil + ")";
                }
                if (checkBox_Moves.Checked)
                {
                    toret += " (Moves: ";
                    if (trpk_m1[i].SelectedIndex > 0) toret += trpk_m1[i].Text;
                    if (trpk_m2[i].SelectedIndex > 0) toret += " / " + trpk_m2[i].Text;
                    if (trpk_m3[i].SelectedIndex > 0) toret += " / " + trpk_m3[i].Text;
                    if (trpk_m4[i].SelectedIndex > 0) toret += " / " + trpk_m4[i].Text;
                    toret += ")";
                }
                toret += " IVs: All " + (Convert.ToInt32(trpk_IV[i].SelectedIndex) / 8);
                toret += "\n";
            }
            toret += "\n";
            return toret;
        }
        private void B_Dump_Click(object sender, EventArgs e)
        {
            string toret = "";
            for (int i = 1; i < 950; i++)
            {
                CB_TrainerID.SelectedIndex = i;
                string tdata = getTRSummary();
                toret += tdata;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "Battles.txt";
            sfd.Filter = "Text File|*.txt";
            
            System.Media.SystemSounds.Asterisk.Play();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string path = sfd.FileName;
                File.WriteAllText(path, toret, System.Text.Encoding.Unicode);
            }
        }

        // Top Level Functions
        private string[] trdatapaths;
        private string[] trpokepaths;
        // Change Read
        private void changeTrainerIndex(object sender, EventArgs e)
        {
            if (index > -1) writeFile();
            readFile(); // Load the new file.
        }
        private void readFile()
        {
            if (start) return;
            index = CB_TrainerID.SelectedIndex;
            loading = true;
            int format;

            if (index == 0) return;

            // Load Trainer Data
            using (BinaryReader br = new BinaryReader((Stream) File.OpenRead(trdatapaths[index])))
            {
                if (br.BaseStream.Length != 24) // required length
                    throw new Exception("Invalid file attempted to load.\n\n" + trdatapaths[index]);

                // load trainer data
                tabControl1.Enabled = true;
                {
                    br.BaseStream.Position = 0;

                    format = br.ReadUInt16();
                    checkBox_Item.Checked = ((format >> 1) & 1) == 1;
                    checkBox_Moves.Checked = ((format) & 1) == 1;

                    CB_Trainer_Class.SelectedIndex = br.ReadUInt16();
                    br.ReadByte();
                    br.ReadByte();

                    CB_Battle_Type.SelectedIndex = br.ReadByte();
                    CB_numPokemon.SelectedIndex = br.ReadByte();

                    CB_Item_1.SelectedIndex = br.ReadUInt16();
                    CB_Item_2.SelectedIndex = br.ReadUInt16();
                    CB_Item_3.SelectedIndex = br.ReadUInt16();
                    CB_Item_4.SelectedIndex = br.ReadUInt16();
                    CB_AI.SelectedIndex = br.ReadByte();
                    br.ReadByte();
                    br.ReadByte();
                    br.ReadByte();

                    checkBox_Healer.Checked = Convert.ToBoolean(br.ReadByte());
                    CB_Money.SelectedIndex = br.ReadByte();

                    CB_Prize.SelectedIndex = br.ReadUInt16();
                }
            }
            // Load Pokemon Data
            using (BinaryReader br = new BinaryReader((Stream)File.OpenRead(trpokepaths[index])))
            {
                br.BaseStream.Position = 0;
                for (int i = 0; i < CB_numPokemon.SelectedIndex; i++)
                {
                    trpk_IV[i].SelectedIndex = br.ReadByte();
                    byte PID = br.ReadByte();
                    trpk_lvl[i].SelectedIndex = br.ReadUInt16();
                    trpk_pkm[i].SelectedIndex = br.ReadUInt16();
                    setForms(trpk_pkm[i].SelectedIndex, trpk_form[i]);
                    trpk_form[i].SelectedIndex = br.ReadUInt16();
                    refreshPKMSlotAbility(i); // Repopulate Abilities

                    trpk_abil[i].SelectedIndex = PID >> 4;
                    trpk_gender[i].SelectedIndex = PID & 3;

                    if (((format >> 1) & 1) == 1) // Items Exist in Data
                        trpk_item[i].SelectedIndex = br.ReadUInt16();
                    if (((format) & 1) == 1) // Moves Exist in Data
                    {
                        trpk_m1[i].SelectedIndex = br.ReadUInt16();
                        trpk_m2[i].SelectedIndex = br.ReadUInt16();
                        trpk_m3[i].SelectedIndex = br.ReadUInt16();
                        trpk_m4[i].SelectedIndex = br.ReadUInt16();
                    }
                }
            }
            loading = false;
            changeTrainerType(null, null); // Prompt cleaning update of PKM fields
        }
        // Change Write
        private void writeFile()
        {
            // fetch trainer format we're saving as
            // index = index;
            ushort format = (ushort)(Convert.ToByte(checkBox_Moves.Checked) + (Convert.ToByte(checkBox_Item.Checked) << 1));

            // Write Trainer Data
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.BaseStream.Position = 0;
                bw.Write((ushort)format);
                bw.Write((ushort)CB_Trainer_Class.SelectedIndex);
                bw.Write((byte)0);
                bw.Write((byte)0);
                bw.Write((byte)CB_Battle_Type.SelectedIndex);
                bw.Write((byte)CB_numPokemon.SelectedIndex);
                bw.Write((ushort)CB_Item_1.SelectedIndex);
                bw.Write((ushort)CB_Item_2.SelectedIndex);
                bw.Write((ushort)CB_Item_3.SelectedIndex);
                bw.Write((ushort)CB_Item_4.SelectedIndex);
                bw.Write((byte)CB_AI.SelectedIndex);
                bw.Write((byte)0);
                bw.Write((byte)0);
                bw.Write((byte)0);
                bw.Write((byte)Convert.ToByte(checkBox_Healer.Checked));
                bw.Write((byte)CB_Money.SelectedIndex);
                bw.Write((ushort)CB_Prize.SelectedIndex);

                File.WriteAllBytes(trdatapaths[index], ms.ToArray());
            }
            // Load Pokemon Data
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.BaseStream.Position = 0;
                for (int i = 0; i < CB_numPokemon.SelectedIndex; i++)
                {
                    bw.Write((byte)trpk_IV[i].SelectedIndex);
                    byte PID = (byte)((trpk_abil[i].SelectedIndex << 4) + trpk_gender[i].SelectedIndex);
                    bw.Write((byte)PID);
                    bw.Write((ushort)trpk_lvl[i].SelectedIndex);

                    bw.Write((ushort)trpk_pkm[i].SelectedIndex);
                    bw.Write((ushort)trpk_form[i].SelectedIndex);

                    if (((format >> 1) & 1) == 1) // Items Exist in Data
                        bw.Write((ushort)trpk_item[i].SelectedIndex);
                    if (((format) & 1) == 1) // Moves Exist in Data
                    {
                        bw.Write((ushort)trpk_m1[i].SelectedIndex);
                        bw.Write((ushort)trpk_m2[i].SelectedIndex);
                        bw.Write((ushort)trpk_m3[i].SelectedIndex);
                        bw.Write((ushort)trpk_m4[i].SelectedIndex);
                    }
                }
                File.WriteAllBytes(trpokepaths[index],ms.ToArray());
            }
        }

        private void Setup()
        {
            start = true;
            string[] personalList = Directory.GetFiles("personal");
            personal = new byte[personalList.Length][];
            for (int i = 0; i < personalList.Length; i++)
                personal[i] = File.ReadAllBytes("personal" + Path.DirectorySeparatorChar + i.ToString("000") + ".bin");

            CB_TrainerID.Items.Clear();
            for (int i = 0; i < trdatapaths.Length; i++)
                CB_TrainerID.Items.Add(String.Format("{1} - {0}", i.ToString("000"), trName[i]));

            CB_Trainer_Class.Items.Clear();
            for (int i = 0; i < trClass.Length; i++)
                CB_Trainer_Class.Items.Add(String.Format("{1} - {0}", i.ToString("000"), trClass[i]));

            specieslist[0] = "---";
            abilitylist[0] = itemlist[0] = movelist[0] = "";

            for (int i = 0; i < 6; i++)
            {
                trpk_pkm[i].Items.Clear();
                foreach (string s in specieslist)
                    trpk_pkm[i].Items.Add(s);

                trpk_m1[i].Items.Clear();
                trpk_m2[i].Items.Clear();
                trpk_m3[i].Items.Clear();
                trpk_m4[i].Items.Clear();
                foreach (string s in movelist)
                {
                    trpk_m1[i].Items.Add(s);
                    trpk_m2[i].Items.Add(s);
                    trpk_m3[i].Items.Add(s);
                    trpk_m4[i].Items.Add(s);
                }

                trpk_item[i].Items.Clear();
                foreach (string s in itemlist)
                    trpk_item[i].Items.Add(s);

                trpk_lvl[i].Items.Clear();
                for (int z = 0; z <= 100; z++)
                    trpk_lvl[i].Items.Add((z).ToString());

                trpk_IV[i].Items.Clear();
                for (int z = 0; z < 256; z++)
                    trpk_IV[i].Items.Add(z.ToString());

                trpk_gender[i].Items.Clear();
                trpk_gender[i].Items.Add("♂ / M");
                trpk_gender[i].Items.Add("♀ / F");
                trpk_gender[i].Items.Add("- / G");

                trpk_form[i].Items.Add("");

                trpk_pkm[i].SelectedIndex = 0;
            }
            CB_Item_1.Items.Clear();
            CB_Item_2.Items.Clear();
            CB_Item_3.Items.Clear();
            CB_Item_4.Items.Clear();
            CB_Prize.Items.Clear();
            foreach (string s in itemlist)
            {
                CB_Item_1.Items.Add(s);
                CB_Item_2.Items.Add(s);
                CB_Item_3.Items.Add(s);
                CB_Item_4.Items.Add(s);
                CB_Prize.Items.Add(s);
            }

            CB_AI.Items.Clear(); CB_Money.Items.Clear();
            for (int i = 0; i < 256; i++)
            { CB_AI.Items.Add(i.ToString()); CB_Money.Items.Add(i.ToString()); }

            CB_Battle_Type.Items.Clear();
            CB_Battle_Type.Items.Add("Single");
            CB_Battle_Type.Items.Add("Double");
            CB_Battle_Type.Items.Add("Triple");
            CB_Battle_Type.Items.Add("Rotation");
            CB_Battle_Type.Items.Add("Horde");

            CB_TrainerID.SelectedIndex = 1;
            start = false;
            readFile();
            System.Media.SystemSounds.Asterisk.Play();
        }

        public static bool rPKM, rMove, rAbility, rDiffAI, rDiffIV, rClass, rGift, rItem, rDoRand;
        public static decimal rGiftPercent;
        private void B_Randomize_Click(object sender, EventArgs e)
        {
            rPKM = rMove = rAbility = rDiffAI = rDiffIV = rClass = rGift = rItem = rDoRand = false; // init to false
            rGiftPercent = 0; // 0
            (new TrainerRand()).ShowDialog(); // Open Randomizer Config to get config vals
            if (rDoRand)
                Randomize();
        }
        private void Randomize()
        {
            for (int i = 1; i < CB_TrainerID.Items.Count; i++)
            {
                CB_TrainerID.SelectedIndex = i; // data is loaded

                // Setup
                checkBox_Moves.Checked = rMove;
                checkBox_Item.Checked = rItem;

                // Randomize Trainer Stats
                if (rDiffAI)
                    CB_AI.SelectedIndex = CB_AI.Items.Count - 1; // max
                if (rClass)
                    CB_Trainer_Class.SelectedIndex = (int)(rnd32() % (CB_Trainer_Class.Items.Count));

                if (rGift && rnd32() % 100 < rGiftPercent)
                #region Random Prize Logic
                {
                    ushort[] items;
                    uint rand = rnd32() % 10;
                    if (rand < 2) // held item
                        items = (oras) ? Legal.Pouch_Items_ORAS : Legal.Pouch_Items_XY;
                    else if (rand < 5) // medicine
                        items = (oras) ? Legal.Pouch_Medicine_ORAS : Legal.Pouch_Medicine_XY;
                    else // berry
                        items = Legal.Pouch_Berry_XY;
                    CB_Prize.SelectedIndex = items[(rnd32() % items.Length)];
                }
                #endregion
                else if (rGift)
                    CB_Prize.SelectedIndex = 0;

                ushort[] itemlist = (oras) ? Legal.Pouch_Items_ORAS : Legal.Pouch_Items_XY;
                itemlist = itemlist.Concat(Legal.Pouch_Berry_XY).ToArray();
                int moves = trpk_m1[0].Items.Count;
                int itemC = itemlist.Length;
                // Randomize Pokemon
                for (int p = 0; p < CB_numPokemon.SelectedIndex; p++)
                {
                    if (rPKM)
                    {
                        // randomize pokemon
                        int species = (int)(rnd32() % 722);
                        trpk_pkm[p].SelectedIndex = species;
                        // Set Gender
                        {
                            int gv = personal[species][0x12];
                            int g = (int)(rnd32() % 0x100);
                            if (gv == 0xFF) // genderless
                                g = 2;
                            else if (gv == 0xFE) // female only
                                g = 1;
                            else if (gv == 0) // male only
                                g = 0;
                            else
                                g = Convert.ToInt16(g < gv); // if greater than, is female

                            trpk_gender[p].SelectedIndex = g;
                        }

                        // randomize form
                        int forms = (trpk_form[p].Items.Count - 1);
                        if (forms > 0)
                            trpk_form[p].SelectedIndex = (int)(rnd32() % (forms + 1));
                    }
                    if (rAbility)
                        trpk_abil[p].SelectedIndex = (int)(1 + rnd32() % 3);
                    if (rDiffIV)
                        trpk_IV[p].SelectedIndex = 255;
                    if (rItem)
                        #region RandomItem
                        trpk_item[p].SelectedIndex = itemlist[(rnd32() % itemC)];
                        #endregion
                    if (rMove)
                    {
                        trpk_m1[p].SelectedIndex = (int)(rnd32() % (moves));
                        trpk_m2[p].SelectedIndex = (int)(rnd32() % (moves));
                        trpk_m3[p].SelectedIndex = (int)(rnd32() % (moves));
                        trpk_m4[p].SelectedIndex = (int)(rnd32() % (moves));
                    }
                }
            }
            CB_TrainerID.SelectedIndex = 1;
            Util.Alert("Randomized all trainers according to specification!", "Press the Dump to TXT to view the new trainer information!");
        }
    }
}