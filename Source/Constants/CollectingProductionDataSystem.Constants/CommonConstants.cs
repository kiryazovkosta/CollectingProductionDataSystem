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

        public static string StandartPassword { get { return standartPassword; } }

        public static string[] PowerUsers { get { return powerUsers; } }
    }
}
