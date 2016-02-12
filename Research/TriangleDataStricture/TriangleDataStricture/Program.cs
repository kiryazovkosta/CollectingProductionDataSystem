namespace TriangleDataStricture
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using TriangleDataStricture.Models;

    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new AppContext())
            {
                Console.Write("Въведете месец: ");
                var str = Console.ReadLine();
                while (str != "x")
                {
                    int month = int.Parse(str);
                    var targetDate = new DateTime(2016, month, 1);

                    var result = db.Fuels
                                   .Include(x => x.QuantityRecords)
                                   .Select(x => new
                                   {
                                       Id = x.Id,
                                       Name = x.Name,
                                       Data = x.QuantityRecords
                                               .OrderByDescending(y => y.RecordMonth)
                                               .FirstOrDefault(z => z.RecordMonth <= targetDate)
                                   });
                    int ix = 0;
                    foreach (var record in result)
                    {
                        //if (ix++ == 0)
                        //{
                        //    Console.ReadKey();
                        //}
                        Console.WriteLine("{0} | {1} | {2} | {3} ",
                            record.Id.ToString().PadRight(10, ' '),
                            record.Name.ToString().PadRight(20, ' '),
                            record.Data == null ? targetDate.ToString().PadRight(15, ' ') : record.Data.RecordMonth.ToString().PadRight(15, ' '),
                            record.Data == null ? 0M.ToString().PadRight(10, ' ') : record.Data.Quantity.ToString().PadRight(10, ' '));
                    }

                    Console.Write("Въведете месец: ");
                    str = Console.ReadLine();
                }
            }
        }
    }
}
