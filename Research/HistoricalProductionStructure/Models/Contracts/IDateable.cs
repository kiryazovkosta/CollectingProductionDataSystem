namespace Domain.Contracts
{
    using System;

    public interface IDateable
    {
         DateTime RecordTimestamp { get; set; }
    }
}
