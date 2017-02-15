//-----------------------------------------------------------------------
// <copyright file="IConfigurable.cs" company="Business Management System Ltd.">
//     Copyright "2017" (c) Business Management System Ltd.. All rights reserved.
// </copyright>
// <author>Nikolay.Kostadinov</author>
//-----------------------------------------------------------------------

namespace CollectingProductionDataSystem.Models.Contracts
{
    /// <summary>
    /// Summary description for IConfigurable
    /// </summary>
    public interface IConfigable
    {
        IProcessUnitCangeable Config { get; }
    }
}
