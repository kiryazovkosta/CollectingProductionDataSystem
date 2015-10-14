using System.Collections.Generic;
using ManyToManySelfRelation.Models;
using System;
using System.Linq;
using System.Data.Entity;

namespace ManyToManySelfRelation
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new AppContext()) 
            {
                var records = db.Records.Include(x => x.RelatedRecords.Select(y=>y.Record)).ToList();
                var related = new int[] { 1, 2, 3 };
                var newRecord = new List<Record> { new Record() { Name = "NewRecord" } }.Select(x => new Record { Name=x.Name, RelatedRecords = related.Select(y=>new RelatedRecords(){RecordId = x.Id,RelatedRecordId = y}).ToList()}).FirstOrDefault();
                db.Records.Add(newRecord);
                db.SaveChanges();
                foreach (var record in records)
                {
                    Console.WriteLine("{0} {1}",record.Id, record.Name);
                    foreach (var relatedR in record.RelatedRecords)
                    {
                        Console.WriteLine("\t{0} {1}", relatedR.RelatedRecord.Id,relatedR.RelatedRecord.Name);
                    }
                }

            }
        }
    }
}
