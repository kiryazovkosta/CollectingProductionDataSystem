//-----------------------------------------------------------------------
// <copyright file="CommonConstants.cs" company="Business Management Systems Ltd.">
//     Copyright (c) Business Management Systems. All rights reserved.
// </copyright>
// <author>Nikolay Kostadinov</author>
//-----------------------------------------------------------------------

namespace CollectingProductionDataSystem.Constants
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class CommonConstants
    {
        private static readonly string standartPassword = "12345678";

        private static readonly string[] powerUsers = new string[] { "Administrator", "PowerUser" };

        private static readonly string[] monthlyTechnologicalApproverUsers = new string[] { "MonthlyTechnologicalApprover" };

        private static readonly string[] monthlyTechnologicalReportWriterUsers = new string[] { "MonthlyTechnologicalReportWriter" };

        private static readonly string loadingUser = "Initial Loading";

        private static readonly int materialType = 1;

        private static readonly int energyType = 2;

        private static readonly int chemicalType = 5;

        private static readonly int firstDayInMonth = 1;

        private static readonly int hydroCarbons = 1;

        private static readonly int freshWater = 2;

        private static readonly int circulatingWater = 3;

        private static readonly int potableWater = 4;

        private static readonly int heatEnergy = 5;

        private static readonly int electricalEnergy = 6;

        private static readonly int chemicalClearedWater = 7;

        private static readonly int air = 8;

        private static readonly int nitrogen = 9;

        private static readonly int chemicals = 10;

        private static readonly int fuels = 11;

        private static readonly int workingHours = 12;

        private static readonly int dailyInfoDailyInfoHydrocarbonsShiftTypeId = 52;

        private static readonly int halfAnHour = 60 * 30;

        private static readonly string phdDateTimeFormat = @"M/d/yyyy hh:mm:ss tt";

        private static readonly int maxDateDifference = 31;

        private static readonly int inputDirection = 2;

        private static readonly int outputDirection = 1;

        private static readonly int inputOutputDirection = 3;

        private static readonly int?[] materialTypechemicalType = new int?[] { MaterialType, ChemicalType };

        private static readonly string phd2SqlDefaultUserName = "Phd2SqlLoader";

        public static string StandartPassword
        {
            get
            {
                return standartPassword;
            }
        }

        public static string[] PowerUsers { get { return powerUsers; } }

        public static string[] MonthlyTechnologicalApproverUsers { get { return monthlyTechnologicalApproverUsers; } }

        public static string[] MonthlyTechnologicalReportWriterUsers { get { return monthlyTechnologicalReportWriterUsers; } }

        public static string LoadingUser { get { return loadingUser; } }

        public static int MaterialType { get { return materialType; } }

        public static int EnergyType { get { return energyType; } }

        public static int ChemicalType { get { return chemicalType; } }

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

        public static int Chemicals { get { return chemicals; } }

        public static int WorkingHours { get { return workingHours; } }

        public static int Fuels { get { return fuels; } }

        public static int DailyInfoDailyInfoHydrocarbonsShiftTypeId { get { return dailyInfoDailyInfoHydrocarbonsShiftTypeId; } }

        public static int MaxDateDifference { get { return maxDateDifference; } }

        public static string PhdDateTimeFormat { get { return phdDateTimeFormat; } }

        public static int HalfAnHour { get { return halfAnHour; } }

        public static int InputDirection { get { return inputDirection; } }

        public static int OutputDirection { get { return outputDirection; } }

        public static int InputOutputDirection { get { return inputOutputDirection; } }

        public static int?[] MaterialTypeChemicalType { get { return materialTypechemicalType; } }

        public static string Phd2SqlDefaultUserName {  get { return phd2SqlDefaultUserName; } }
    }
}
