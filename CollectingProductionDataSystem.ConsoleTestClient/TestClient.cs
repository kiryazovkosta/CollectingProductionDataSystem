namespace CollectingProductionDataSystem.ConsoleTestClient
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using CollectingProductionDataSystem.Data;
    using CollectingProductionDataSystem.Models;

    public class TestClient
    {
        public static void Main()
        {
            using (var db = new CollectingDataSystemDbContext())
            {
                try
                {
                    //var factories = db.Factories;
                    //foreach (var factory in factories)
                    //{
                    //    Console.WriteLine(factory.Name); 
                    //}
                    var tanksData = db.InventoryTanksData.Include("Product").Where(d => d.TankId == 1).ToList();
                    foreach (InventoryTanksData item in tanksData)
	                {
                        Console.WriteLine(item.Product.Name);
	                }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
