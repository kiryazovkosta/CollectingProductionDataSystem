namespace TriangleDataStricture.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TriangleDataStricture.Models;

    public sealed class Configuration : DbMigrationsConfiguration<AppContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(AppContext context)
        {

            var records = new List<Fuel>(){
              new Fuel { Id = 1, Name = "FirstFuel" },
              new Fuel { Id = 2, Name = "SecondFuel" },
              new Fuel { Id = 3, Name = "ThurdFuel" },
              new Fuel { Id = 4, Name = "FourthFuel" },
              new Fuel { Id = 5, Name = "FifthFuel" }
            };

            //for (int i = 1; i <= 500; i++)
            //{
            //    records.Add(new Fuel { Id = i, Name = string.Format("Fuel_{0:d3}", i) });
            //}

            //foreach (var record in records)
            //{
            //    for (int i = 1; i < 13; i++)
            //    {
            //        record.QuantityRecords.Add(new QuantityRecord() { FuelId = record.Id, RecordMonth = new DateTime(2016, i, 1), Quantity = i });
            //        record.QuantityRecords.Add(new QuantityRecord() { FuelId = record.Id, RecordMonth = new DateTime(2017, i, 1), Quantity = i });
            //        record.QuantityRecords.Add(new QuantityRecord() { FuelId = record.Id, RecordMonth = new DateTime(2018, i, 1), Quantity = i });
            //        record.QuantityRecords.Add(new QuantityRecord() { FuelId = record.Id, RecordMonth = new DateTime(2019, i, 1), Quantity = i });
            //        record.QuantityRecords.Add(new QuantityRecord() { FuelId = record.Id, RecordMonth = new DateTime(2020, i, 1), Quantity = i });

            //    }
            //}

            ////foreach (var record in records)
            ////{
            ////    record.QuantityRecords.Add(new QuantityRecord() { FuelId = record.Id, RecordMonth = new DateTime(2016, (record.Id % 12 == 0) ? 12 : record.Id % 12, 1), Quantity = record.Id });
            ////}

            records[0].QuantityRecords = new List<QuantityRecord>()
            {
                new QuantityRecord(){FuelId = records[0].Id,RecordMonth= new DateTime(2016,1,1), Quantity = 1 },
                //new QuantityRecord(){FuelId = records[0].Id,RecordMonth= new DateTime(2016,2,1), Quantity = 2 },
                //new QuantityRecord(){FuelId = records[0].Id,RecordMonth= new DateTime(2016,3,1), Quantity = 3 },
                //new QuantityRecord(){FuelId = records[0].Id,RecordMonth= new DateTime(2016,4,1), Quantity = 4 },
                new QuantityRecord(){FuelId = records[0].Id,RecordMonth= new DateTime(2016,10,1), Quantity = 5 },
            };

            records[1].QuantityRecords = new List<QuantityRecord>()
            {
                //new QuantityRecord(){FuelId = records[1].Id,RecordMonth= new DateTime(2016,1,1), Quantity = 11 },
                new QuantityRecord(){FuelId = records[1].Id,RecordMonth= new DateTime(2016,2,1), Quantity = 12 },
                //new QuantityRecord(){FuelId = records[1].Id,RecordMonth= new DateTime(2016,3,1), Quantity = 13 },
                //new QuantityRecord(){FuelId = records[1].Id,RecordMonth= new DateTime(2016,4,1), Quantity = 14 },
                //new QuantityRecord(){FuelId = records[1].Id,RecordMonth= new DateTime(2016,5,1), Quantity = 15 },
            };

            records[2].QuantityRecords = new List<QuantityRecord>()
            {
                //new QuantityRecord(){FuelId = records[2].Id,RecordMonth= new DateTime(2016,1,1), Quantity = 31 },
                //new QuantityRecord(){FuelId = records[2].Id,RecordMonth= new DateTime(2016,2,1), Quantity = 32 },
                new QuantityRecord(){FuelId = records[2].Id,RecordMonth= new DateTime(2016,3,1), Quantity = 33 },
                //new QuantityRecord(){FuelId = records[2].Id,RecordMonth= new DateTime(2016,4,1), Quantity = 34 },
                //new QuantityRecord(){FuelId = records[2].Id,RecordMonth= new DateTime(2016,5,1), Quantity = 35 },
            };

            records[3].QuantityRecords = new List<QuantityRecord>()
            {
                //new QuantityRecord(){FuelId = records[3].Id,RecordMonth= new DateTime(2016,1,1), Quantity = 41 },
                //new QuantityRecord(){FuelId = records[3].Id,RecordMonth= new DateTime(2016,2,1), Quantity = 42 },
                //new QuantityRecord(){FuelId = records[3].Id,RecordMonth= new DateTime(2016,3,1), Quantity = 43 },
                new QuantityRecord(){FuelId = records[3].Id,RecordMonth= new DateTime(2016,4,1), Quantity = 44 },
                //new QuantityRecord(){FuelId = records[3].Id,RecordMonth= new DateTime(2016,5,1), Quantity = 45 },
            };

            records[4].QuantityRecords = new List<QuantityRecord>()
            {
                //new QuantityRecord(){FuelId = records[4].Id,RecordMonth= new DateTime(2016,1,1), Quantity = 51 },
                //new QuantityRecord(){FuelId = records[4].Id,RecordMonth= new DateTime(2016,2,1), Quantity = 52 },
                //new QuantityRecord(){FuelId = records[4].Id,RecordMonth= new DateTime(2016,3,1), Quantity = 53 },
                //new QuantityRecord(){FuelId = records[4].Id,RecordMonth= new DateTime(2016,4,1), Quantity = 54 },
                new QuantityRecord(){FuelId = records[4].Id,RecordMonth= new DateTime(2016,5,1), Quantity = 55 },
            };





            ////  This method will be called after migrating to the latest version.

            ////  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            ////  to avoid creating duplicate seed data. E.g.
            ////
            //foreach (var record in records)
            //{
            //    context.Fuels.AddOrUpdate(
            //     p => p.Id,
            //     record);
            //}
            //context.Fuels.AddOrUpdate(
            //  p => p.Id,
            //  records[0],
            //  records[1],
            //  records[2],
            //  records[3],
            //  records[4]
            //);

        }
    }
}
