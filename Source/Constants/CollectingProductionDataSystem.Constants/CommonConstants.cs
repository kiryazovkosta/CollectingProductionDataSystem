using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingProductionDataSystem.Constants
{
    public static class CommonConstants
    {
        private static readonly string standartPassword = "12345678";

        private static readonly string[] powerUsers = new string[] { "Administrator", "PowerUser" };

        private static readonly string loadingUser = "Initial Loading";

        private static readonly int materialType = 1;

        private static readonly int energyType = 2;

        private static readonly int firstDayInMonth = 1;

        private static readonly int hydroCarbons = 1;

        private static readonly int freshWater = 2;

        private static readonly int circulatingWater = 3;

        private static readonly int potableWater = 4;

        private static readonly int heatEnergy = 5;

        private static readonly int electricalEnergy = 6;

        private static readonly int chemicalClearedWater = 7;
        
        private static int air = 8;

        private static int nitrogen = 9;

        private static int chemicals = 10;

        private static readonly int dailyInfoDailyInfoHydrocarbonsShiftTypeId = 52;

        private static readonly int halfAnHour = 60 * 30;

        private static string phdDateTimeFormat = @"M/d/yyyy hh:mm:ss tt";

        private static int maxDateDifference = 31;

        

        public static string StandartPassword
        {
            get
            {
                return standartPassword;
            }
        }

        public static string[] PowerUsers { get { return powerUsers; } }

        public static string LoadingUser { get { return loadingUser; } }

        public static int MaterialType { get { return materialType; } }

        public static int EnergyType { get { return energyType; } }

        public static int FirstDayInMonth { get { return firstDayInMonth; } }

        public static int HydroCarbons { get { return hydroCarbons; } }

        public static int FreshWater { get { return freshWater; } }

        public static int CirculatingWater { get { return circulatingWater; } }

        public static int PotableWater { get { return potableWater; } }

        public static int HeatEnergy { get { return heatEnergy; } }

        public static int ElectricalEnergy { get { return electricalEnergy; } }

        public static int ChemicalClearedWater { get { return chemicalClearedWater; } }

        public static int Air { get { return air; } }

        public static int Nitrogen { get { return nitrogen; } }

        public static int Chemicals { get; set; }

        public static int DailyInfoDailyInfoHydrocarbonsShiftTypeId { get { return dailyInfoDailyInfoHydrocarbonsShiftTypeId; } }

        public static int MaxDateDifference { get { return maxDateDifference; } }

        public static string PhdDateTimeFormat { get { return phdDateTimeFormat; } }

        public static int HalfAnHour { get { return halfAnHour; } }
    }
}
