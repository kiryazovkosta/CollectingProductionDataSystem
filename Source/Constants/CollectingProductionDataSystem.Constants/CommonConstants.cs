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

        private static readonly int dailyInfoDailyInfoHydrocarbonsShiftTypeId = 52;

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

        public static int DailyInfoDailyInfoHydrocarbonsShiftTypeId { get { return dailyInfoDailyInfoHydrocarbonsShiftTypeId; } }
    }
}
