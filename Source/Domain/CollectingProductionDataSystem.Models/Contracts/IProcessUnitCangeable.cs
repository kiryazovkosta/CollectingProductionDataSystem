//-----------------------------------------------------------------------
// <copyright file="IProcessUnitCangeable.cs" company="Business Management System Ltd.">
//     Copyright "2017" (c) Business Management System Ltd.. All rights reserved.
// </copyright>
// <author>Nikolay.Kostadinov</author>
//-----------------------------------------------------------------------

namespace CollectingProductionDataSystem.Models.Contracts
{
    using Productions;

    /// <summary>
    /// Summary description for IProcessUnitCangeable
    /// </summary>
    public interface IProcessUnitCangeable
    {
        int ProcessUnitId { get; }
        ProcessUnit HistorycalProcessUnit { get; set; }
    }
}
