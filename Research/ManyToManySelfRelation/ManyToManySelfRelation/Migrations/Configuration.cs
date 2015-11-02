namespace ManyToManySelfRelation.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using ManyToManySelfRelation.Models;

    public sealed class Configuration : DbMigrationsConfiguration<AppContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(AppContext context)
        {

            var records = new List<Record>{
              new Record { Id = 1, Name = "FirstRecord" },
              new Record { Id = 2, Name = "SecondRecord" },
              new Record { Id = 3, Name = "ThurdRecord" },
              new Record { Id = 4, Name = "FourthRecord" },
              new Record { Id = 5, Name = "FifthRecord" }
            };

            records[0].RelatedRecords = new List<RelatedRecords>()
            {
                new RelatedRecords(){RecordId = records[0].Id,RelatedRecordId=records[1].Id},
                new RelatedRecords(){RecordId = records[0].Id,RelatedRecordId=records[2].Id}
            };

            records[1].RelatedRecords = new List<RelatedRecords>()
            {
                new RelatedRecords(){RecordId = records[1].Id,RelatedRecordId=records[3].Id},
                new RelatedRecords(){RecordId = records[1].Id,RelatedRecordId=records[4].Id}
            };

            records[4].RelatedRecords = new List<RelatedRecords>()
            {
                new RelatedRecords(){RecordId = records[4].Id,RelatedRecordId=records[1].Id},
            };

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.Records.AddOrUpdate(
              p => p.Id,
              records[0],
              records[1],
              records[2],
              records[3],
              records[4]
            );

        }
    }
}
