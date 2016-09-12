//-----------------------------------------------------------------------
// <copyright file="FileUploadService.cs" company="Business Management Systems Ltd.">
//     Copyright (c) Business Management Systems. All rights reserved.
// </copyright>
// <author>Nikolay Kostadinov</author>
//-----------------------------------------------------------------------

namespace CollectingProductionDataSystem.Application.FileServices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using static System.String;
    using System.Text;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Constants;
    using CollectingProductionDataSystem.Data.Contracts;
    using Ninject;
    using Resources = App_Resources.ErrorMessages;

    /// <summary>
    /// File upload service
    /// </summary>
    public class FileUploadService : IFileUploadService
    {
        private const int ASSEMBLY_NAME_POSITION = 0;
        private const int ENTITY_NAME_POSITION = 1;
        private const int DATETIME_FORMAT_POSITION = 2;
        private const int ENTITY_NAME_FILE_LINE = 0;
        private const int PROPERTIES_DESCRIPTION_FILE_LINE = 1;
        private const int FILE_CONTENT_STARTING_LINE = 2;
        private readonly IProductionData data;
        private readonly IKernel kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileUploadService" /> class.
        /// </summary>
        /// <param name="dataParam">The data parameter.</param>
        /// <param name="kernelParam">The kernel parameter.</param>
        public FileUploadService(IProductionData dataParam, IKernel kernelParam)
        {
            this.kernel = kernelParam;
            this.data = dataParam;
        }

        /// <summary>
        /// Uploads the file to database.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns></returns>
        public IEfStatus UploadFileToDatabase(string fileName, string delimiter)
        {
            FileDescriptor fileResult = GetRecordsFromFile(fileName, delimiter);
            return ParseRecordsAndPersistToDatabase(fileResult, delimiter);
        }

        /// <summary>
        /// Uploads the file to database.
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public IEfStatus UploadFileToDatabase(Stream fileStream, string delimiter, string fileName)
        {
            FileDescriptor fileResult = GetRecordsFromFileAsStream(fileStream, delimiter, fileName);
            return ParseRecordsAndPersistToDatabase(fileResult, delimiter);
        }

        /// <summary>
        /// Gets the records from file as stream.
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        private FileDescriptor GetRecordsFromFileAsStream(Stream fileStream, string delimiter, string fileName)
        {
             using (StreamReader file = new StreamReader(fileStream, Encoding.GetEncoding("windows-1251")))
            {
                FileDescriptor fileDescriptor = new FileDescriptor() { Status = this.kernel.Get<IEfStatus>() };

                char[] charSeparator = { Convert.ToChar(delimiter) };
                try
                {
                    GetRecordsDescription(file, fileDescriptor, charSeparator);
                    GetPropertiesDescription(file, fileDescriptor, charSeparator);
                    string line = string.Empty;
                    while ((line = file.ReadLine()) != null)
                    {
                        fileDescriptor.Records.Add(line);
                    }
                }
                catch (FileLoadException)
                {
                   fileDescriptor.Status.SetErrors(
                        this.GetValidationResult(Format(Resources.FileProcessError, fileName))
                        );
                }
                catch (OutOfMemoryException)
                {
                    fileDescriptor.Status.SetErrors(
                        this.GetValidationResult(Format(Resources.FileProcessError, fileName))
                        );

                }
                catch (IOException)
                {
                    fileDescriptor.Status.SetErrors(
                        this.GetValidationResult(Format(Resources.FileProcessError, fileName))
                        );
                }
                catch (ArgumentNullException anEx)
                {
                    fileDescriptor.Status.SetErrors(
                        this.GetValidationResult(Format(Resources.InvalidRecordType, anEx.Message))
                        );
                }

                return fileDescriptor;
            }
        }

        /// <summary>
        /// Gets the properties description.
        /// </summary>
        /// <param name="fileReader">The file reader.</param>
        /// <param name="fileDescriptor">The file descriptor.</param>
        /// <param name="charSeparator">The char separator.</param>
        private static void GetPropertiesDescription(StreamReader fileReader, FileDescriptor fileDescriptor, char[] charSeparator)
        {
            string line = fileReader.ReadLine();
            if (!IsNullOrEmpty(line))
            {

                string[] result = line.Split(charSeparator, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < result.Length; i++)
                {
                    fileDescriptor.Properties.Add(new PropertyDescription { Position = i, Name = result[i] });
                }
            }
            else
            {
                throw new FileLoadException("Cannot read the header information!");
            }

        }

        /// <summary>
        /// Gets the records description.
        /// </summary>
        /// <param name="fileReader">The file reader.</param>
        /// <param name="fileDescriptor">The file descriptor.</param>
        /// <param name="charSeparator">The char separator.</param>
        private void GetRecordsDescription(StreamReader fileReader, FileDescriptor fileDescriptor, char[] charSeparator)
        {
            string line = fileReader.ReadLine();
            if (!IsNullOrEmpty(line))
            {
                try
                {
                    string[] result = line.Split(charSeparator, StringSplitOptions.RemoveEmptyEntries);

                    string assemblyName = result[ASSEMBLY_NAME_POSITION];
                    string entityName = result[ENTITY_NAME_POSITION];

                    fileDescriptor.RecordOriginalType = ExtractType(assemblyName, entityName);
                    fileDescriptor.DateTimeFormat = result[DATETIME_FORMAT_POSITION];
                }
                catch (IndexOutOfRangeException ex)
                {
                    throw new FileLoadException("Cannot read the header information!",ex);
                }
            }
            else
            {
                throw new FileLoadException("Cannot read the header information!");
            }
        }

        private IEfStatus ParseRecordsAndPersistToDatabase(FileDescriptor fileResult, string delimiter)
        {
            if (!fileResult.Status.IsValid)
            {
                return fileResult.Status;
            }

            IEnumerable<object> objResultl;
            IEfStatus status = Parse(fileResult, delimiter, out objResultl);
            if (!status.IsValid)
            {
                return status;
            }

            status = SaveRecordsToDataBase(objResultl, fileResult.RecordOriginalType);

            return status;
        }

        /// <summary>
        /// Parses the specified file result.
        /// </summary>
        /// <param name="fileResult">The file result.</param>
        /// <param name="objResultl">The obj resultl.</param>
        /// <returns></returns>
        private IEfStatus Parse(FileDescriptor fileResult, string delimiter, out IEnumerable<object> objResult)
        {
            IEfStatus status = this.kernel.Get<IEfStatus>();
            List<ValidationResult> errors = new List<ValidationResult>();
            var outResult = new List<object>();

            objResult = new List<object>();

            int lineId = FILE_CONTENT_STARTING_LINE;

            foreach (var record in fileResult.Records)
            {
                IEnumerable<string> propertiesValues = null;
                try
                {
                    propertiesValues = GetPropertiesValues(record, delimiter, fileResult.Properties.Count());
                }
                catch (ArgumentException)
                {
                    errors.Add(new ValidationResult(string.Format(Resources.InvalidRecord, lineId)));
                }

                Dictionary<string, object> properties = ConvertStringCollectionToPropDictionary(propertiesValues, fileResult);

                var lineObject = this.CreateObject(fileResult.RecordOriginalType, properties);

                outResult.Add(lineObject);

                lineId++;
            }

            objResult = outResult;

            return status;
        }

        /// <summary>
        /// Converts the string collection to prop dictionary.
        /// </summary>
        /// <param name="propertiesValues">The properties values.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        private Dictionary<string, object> ConvertStringCollectionToPropDictionary(IEnumerable<string> propertiesValues, FileDescriptor fileResult)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture =
                System.Globalization.CultureInfo.InvariantCulture;

            Dictionary<string, object> result = new Dictionary<string, object>();

            var valuesArray = propertiesValues.ToArray();
            var propertyDescriptionArray = fileResult.Properties.ToArray();
            Dictionary<string, PropertyInfo> propertyPrototypes = fileResult.RecordOriginalType.GetProperties()
                            .ToDictionary(x => x.Name);

            if (propertiesValues == null)
            {
                throw new ArgumentNullException("propertiesValues", string.Empty);
            }

            if (fileResult.Properties == null)
            {
                throw new ArgumentNullException("fileResult.Properties", string.Empty);
            }

            for (int i = 0; i < propertiesValues.Count(); i++)
            {
                if (!string.IsNullOrEmpty(valuesArray[i]))
                {

                    Type t = Nullable.GetUnderlyingType(propertyPrototypes[propertyDescriptionArray[i].Name].PropertyType)
                        ?? propertyPrototypes[propertyDescriptionArray[i].Name].PropertyType;

                    object safeValue;
                    if (t == typeof(DateTime))
                    {
                        if (valuesArray[i] == null)
                        {
                            safeValue = null;
                        }
                        else
                        {
                            safeValue = DateTime.ParseExact(valuesArray[i], fileResult.DateTimeFormat, CultureInfo.CurrentCulture);
                        }
                    }
                    else
                    {
                        if (t != typeof(DateTime) && t == typeof(string))
                        {
                            if (valuesArray[i] != null)
                            {
                                valuesArray[i] = valuesArray[i].Replace(',', '.');
                            }
                        }

                        safeValue = (valuesArray[i] == null) ? null : Convert.ChangeType(valuesArray[i], t);
                    }

                    result.Add(propertyDescriptionArray[i].Name, safeValue);
                }
            }

            return result;
        }

        /// <summary>
        /// Creates the object.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        private object CreateObject(Type entityType, Dictionary<string, object> properties)
        {
            MethodInfo method = typeof(FileUploadService).GetMethod("CreateNewObject", BindingFlags.Instance | BindingFlags.NonPublic);
            MethodInfo generic = method.MakeGenericMethod(entityType);
            return generic.Invoke(this, new[] { properties });
        }


        /// <summary>
        /// Gets the properties values.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        private IEnumerable<string> GetPropertiesValues(string record, string delimiter, int count)
        {
            var result = record.Split(new string[] { delimiter }, StringSplitOptions.None);

            for (int i = 0; i < result.Count(); i++)
            {
                if (result[i].ToLower() == "null")
                {
                    result[i] = null;
                }
            }

            if (result.Count() != count)
            {
                throw new ArgumentException();
            }

            return result;
        }

        /// <summary>
        /// Gets the records from file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns></returns>
        private FileDescriptor GetRecordsFromFile(string fileName, string delimiter)
        {
            IEfStatus status = FileExist(fileName);
            var result = new FileDescriptor();

            if (!status.IsValid)
            {
                return new FileDescriptor { Status = status };
            }

            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read))//, Encoding.GetEncoding("windows-1251")))
            {
                result = this.GetRecordsFromFileAsStream(file, delimiter, fileName);
            }

            return result;
        }

        /// <summary>
        /// Extracts the type.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="entityName">Name of the entity.</param>
        /// <returns></returns>
        private Type ExtractType(string assemblyName, string entityName)
        {
            var objHandler = Activator.CreateInstance(assemblyName, entityName);
            var obj = objHandler.Unwrap();
            return obj.GetType();
        }

        /// <summary>
        /// Files the exist.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        private IEfStatus FileExist(string fileName)
        {
            var efResult = this.kernel.Get<IEfStatus>();

            if (!File.Exists(fileName))
            {
                efResult.SetErrors(
                   this.GetValidationResult(string.Format(Resources.FileNotFound, fileName)));
            }

            return efResult;
        }

        private List<ValidationResult> GetValidationResult(string message)
        {
            return new List<ValidationResult> {
                        new ValidationResult(string.Format(message))
                    };
        }

        /// <summary>
        /// Saves the records to data base.
        /// </summary>
        /// <param name="objResultl">The obj resultl.</param>
        /// <param name="entityName">Name of the entity.</param>
        /// <returns></returns>
        private IEfStatus SaveRecordsToDataBase(IEnumerable<object> objResult, Type entityType)
        {
            // construct the real collection type as List<entityType>
            var listType = typeof(List<>);
            var collectionType = listType.MakeGenericType(new Type[] { entityType });
            var collectionToPersist = Activator.CreateInstance(collectionType);

            // copy all the records from input collection to the new List<entityType>
            var addMethod = collectionType.GetMethod("Add");

            foreach (var record in objResult)
            {
                addMethod.Invoke(collectionToPersist, new object[] { record });
            }

            data.DbContext.GetType().GetMethod("BulkInsert")
            .MakeGenericMethod(entityType)
            .Invoke(data.DbContext, new object[] { collectionToPersist, CommonConstants.LoadingUser });
            var res = data.SaveChanges(CommonConstants.LoadingUser);
            if (res.IsValid)
            {
                res.ResultRecordsCount = objResult.Count();
            }
            return res;
        }

        /// <summary>
        /// Creates the new object.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="inputParams">The input params.</param>
        /// <returns></returns>
        private T CreateNewObject<T>(Dictionary<string, object> inputParams)
            where T : new()
        {
            var newObj = new T();
            var props = newObj.GetType();
            foreach (var item in inputParams)
            {
                var prop = props.GetProperty(item.Key);
                prop.SetValue(newObj, item.Value);
            }

            return newObj;
        }
    }

    /// <summary>
    /// DTO class
    /// </summary>
    internal class PropertyDescription
    {
        public int Position { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// DTO Class
    /// </summary>
    internal class FileDescriptor
    {
        private string dateTimeFormat;
        private Type recordOriginalType;

        public FileDescriptor()
        {
            this.dateTimeFormat = string.Empty;
            this.Properties = new List<PropertyDescription>();
            this.Records = new List<string>();
            this.recordOriginalType = null;
        }

        /// <summary>
        /// Gets or sets the type of the record original.
        /// </summary>
        /// <value>The type of the record original.</value>
        public Type RecordOriginalType
        {
            get { return this.recordOriginalType; }
            set
            {
                if (value != null)
                {
                    this.recordOriginalType = value;
                }
                else
                {
                    throw new ArgumentNullException(nameof(RecordOriginalType));
                }
            }
        }

        /// <summary>
        /// Gets or sets the date time format.
        /// </summary>
        /// <value>The date time format.</value>
        public string DateTimeFormat
        {
            get { return this.dateTimeFormat; }
            set
            {
                if (value != null)
                {
                    this.dateTimeFormat = value;
                }
                else
                {
                    throw new ArgumentNullException(nameof(DateTimeFormat));
                }
            }
        }

        public ICollection<PropertyDescription> Properties { get; set; }

        public ICollection<string> Records { get; set; }

        public IEfStatus Status { get; set; }
    }
}
