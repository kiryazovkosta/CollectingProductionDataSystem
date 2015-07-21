namespace CollectingProductionDataSystem.ConsoleTestClient
{
    using System;
    using System.Linq;

    using CollectingProductionDataSystem.Data;
    using CollectingProductionDataSystem.Models;

    public class TestClient
    {
        public static void Main()
        {
            using (var db = new CollectingDataSystemDbContext())
            {
                //var areas = db.Areas;
                //foreach (var area in areas)
                //{
                //    Console.WriteLine(area.Name);    
                //}

                var plants = db.Plants;
                foreach (var plant in plants)
                {
                    Console.WriteLine(plant.Id); 
                }
            }
        }
    }
}
