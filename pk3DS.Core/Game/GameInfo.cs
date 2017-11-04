namespace pk3DS.Core
{
    public class GameInfo
    {
        public int MaxSpeciesID { get; private set; }
        public int MaxItemID { get; private set; }
        public int MaxMoveID { get; private set; }
        public ushort[] HeldItems { get; private set; }
        public int MaxAbilityID { get; private set; }

        public GameInfo(GameConfig gameConfig)
        {
            switch (gameConfig.Version)
            {
                case GameVersion.XY: LoadXY(); break;
                case GameVersion.ORASDEMO:
                case GameVersion.ORAS: LoadAO(); break;
                case GameVersion.SMDEMO:
                case GameVersion.SM: LoadSM(); break;
                case GameVersion.USUM: LoadUSUM(); break;
            }
        }

        private void LoadXY()
        {
            MaxSpeciesID = Legal.MaxSpeciesID_6;
            MaxMoveID = Legal.MaxMoveID_6_XY;
            MaxItemID = Legal.MaxItemID_6_XY;
            HeldItems = Legal.HeldItem_XY;
            MaxAbilityID = Legal.MaxAbilityID_6_XY;
        }

        private void LoadAO()
        {
            MaxSpeciesID = Legal.MaxSpeciesID_6;
            MaxMoveID = Legal.MaxMoveID_6_AO;
            MaxItemID = Legal.MaxItemID_6_AO;
            HeldItems = Legal.HeldItem_AO;
            MaxAbilityID = Legal.MaxAbilityID_6_AO;
        }

        private void LoadSM()
        {
            MaxSpeciesID = Legal.MaxSpeciesID_7;
            MaxMoveID = Legal.MaxMoveID_7;
            MaxItemID = Legal.MaxItemID_7;
            HeldItems = Legal.HeldItems_SM;
            MaxAbilityID = Legal.MaxAbilityID_7;
        }

        private void LoadUSUM()
        {
            MaxSpeciesID = Legal.MaxSpeciesID_7_USUM;
            MaxMoveID = Legal.MaxMoveID_7_USUM;
            MaxItemID = Legal.MaxItemID_7_USUM;
            HeldItems = Legal.HeldItems_SM;
            MaxAbilityID = Legal.MaxAbilityID_7;
        }
    }
}
