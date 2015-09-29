namespace CollectingProductionDataSystem.Application.FileServices
{
    using System;
    using System.Runtime.InteropServices.ComTypes;
    using CollectingProductionDataSystem.Data.Contracts;

    public interface IFileUploadService
    {
        IEfStatus UploadFileToDatabase(IStream fileStream, string delimiter);
        IEfStatus UploadFileToDatabase(string fileName, string delimiter);
    }
}
