namespace CollectingProductionDataSystem.Application.Contracts
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices.ComTypes;
    using CollectingProductionDataSystem.Data.Contracts;

    public interface IFileUploadService
    {
        IEfStatus UploadFileToDatabase(Stream fileStream, string delimiter, string fileName);
        IEfStatus UploadFileToDatabase(string fileName, string delimiter);
    }
}
