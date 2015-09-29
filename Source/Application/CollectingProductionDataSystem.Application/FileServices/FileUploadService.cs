namespace CollectingProductionDataSystem.Application.FileServices
{
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices.ComTypes;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Constants;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Infrastructure.Extentions;
    using CollectingProductionDataSystem.Models.Productions;
    using Ninject;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Resources = App_Resources.ErrorMessages;

    public class FileUploadService : CollectingProductionDataSystem.Application.FileServices.IFileUploadService
    {
        const int ASSEMBLY_NAME_POSITION = 0;
        const int ENTITY_NAME_POSITION = 1;
        const int DATETIME_FORMAT_POSITION = 2;
        const int ENTITY_NAME_FILE_LINE = 0;
        const int PROPERTIES_DESCRIPTION_FILE_LINE = 1;
        const int FILE_CONTENT_STARTING_LINE = 2;
        private readonly IProductionData data;
        private readonly IKernel kernel;
        public FileUploadService(IProductionData dataParam, IKernel kernelParam)
        {
            this.kernel = kernelParam;
            this.data = dataParam;
        }
        public IEfStatus UploadFileToDatabase(string fileName, string delimiter)
        {
            FileResult fileResult = GetRecordsFromFile(fileName, delimiter);
            return ParseRecordsAndPersistToDatabase(fileResult, delimiter);
        }

        public IEfStatus UploadFileToDatabase(IStream fileStream, string delimiter)
        {
            FileResult fileResult = GetRecordsFromFileAsStream(fileStream, delimiter);
            return null;//ParseRecordsAndPersistToDatabase(fileResult);
        }

        /// <summary>
        /// Gets the records from file as stream.
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns></returns>
        private FileResult GetRecordsFromFileAsStream(IStream fileStream, string delimiter)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        private IEfStatus ParseRecordsAndPersistToDatabase(FileResult fileResult, string delimiter)
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
        private IEfStatus Parse(FileResult fileResult, string delimiter, out IEnumerable<object> objResult)
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
                catch (ArgumentException argEx)
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
        private Dictionary<string, object> ConvertStringCollectionToPropDictionary(IEnumerable<string> propertiesValues, FileResult fileResult)
        {
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
        private FileResult GetRecordsFromFile(string fileName, string delimiter)
        {
            IEfStatus status = FileExist(fileName);
            if (!status.IsValid)
            {
                return new FileResult { Status = status };
            }

            using (System.IO.StreamReader file = new System.IO.StreamReader(fileName, Encoding.GetEncoding("windows-1251")))
            {
                string line;
                int count = 0;

                string[] result;
                string assemblyName = string.Empty;
                string entityName = string.Empty;
                string dateTimeFormat = string.Empty;
                char[] charSeparator = { Convert.ToChar(delimiter) };
                Type originalType = null;
                List<PropertyDescription> properties = new List<PropertyDescription>();
                List<string> records = new List<string>();
                try
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        if (count == ENTITY_NAME_FILE_LINE)
                        {
                            result = line.Split(charSeparator, StringSplitOptions.RemoveEmptyEntries);
                            assemblyName = result[ASSEMBLY_NAME_POSITION];
                            entityName = result[ENTITY_NAME_POSITION];
                            dateTimeFormat = result[DATETIME_FORMAT_POSITION];
                            originalType = ExtractType(assemblyName, entityName);
                            if (originalType == null)
                            {
                                throw new ArgumentNullException("originalType");
                            }
                        }
                        if (count == PROPERTIES_DESCRIPTION_FILE_LINE)
                        {
                            result = line.Split(charSeparator, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < result.Length; i++)
                            {
                                properties.Add(new PropertyDescription { Position = i, Name = result[i] });
                            }
                        }
                        // content of records in the file
                        if (count >= FILE_CONTENT_STARTING_LINE)
                        {
                            records.Add(line);
                        }

                        count++;
                    }
                }
                catch (OutOfMemoryException oomException)
                {
                    // TODO:Add error logging after implementation of ILogger
                    status.SetErrors(
                        this.GetValidationResult(string.Format(Resources.FileProcessError, fileName))
                        );

                }
                catch (IOException ioException)
                {
                    // TODO:Add error logging after implementation of ILogger
                    status.SetErrors(
                        this.GetValidationResult(string.Format(Resources.FileProcessError, fileName))
                        );
                }
                catch (ArgumentNullException)
                {
                    status.SetErrors(
                        this.GetValidationResult(string.Format(Resources.InvalidRecordType, entityName))
                        );
                }

                return new FileResult()
                                {
                                    RecordOriginalType = originalType,
                                    DateTimeFormat = dateTimeFormat,
                                    Properties = properties,
                                    Records = records,
                                    Status = status
                                };
            }
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
        private IEfStatus SaveRecordsToDataBase(IEnumerable<object> objResultl, Type entityType)
        {
            List<UnitConfig> records = new List<UnitConfig>();
            foreach (var item in objResultl)
            {
                records.Add(item as UnitConfig);
            }
            var timer = new Stopwatch();
            timer.Start();
            
            data.Units.BulkInsert(records, CommonConstants.LoadingUser);
            var res = data.SaveChanges("Initial Loading");

            return res;
        }

        private static bool ValidateDllPath(ref string path, string RelativePath = "", string Extension = "")
        {
            // Check if it contains any Invalid Characters.
            if (path.IndexOfAny(Path.GetInvalidPathChars()) == -1)
            {
                try
                {
                    // If path is relative take %IGXLROOT% as the base directory
                    if (!Path.IsPathRooted(path))
                    {
                        if (string.IsNullOrEmpty(RelativePath))
                        {
                            // Exceptions handled by Path.GetFullPath
                            // ArgumentException path is a zero-length string, contains only white space, or contains one or more of the invalid characters defined in GetInvalidPathChars. -or- The system could not retrieve the absolute path.
                            // 
                            // SecurityException The caller does not have the required permissions.
                            // 
                            // ArgumentNullException path is null.
                            // 
                            // NotSupportedException path contains a colon (":") that is not part of a volume identifier (for example, "c:\"). 
                            // PathTooLongException The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.

                            // RelativePath is not passed so we would take the project path 
                            path = Path.GetFullPath(RelativePath);

                        }
                        else
                        {
                            // Make sure the path is relative to the RelativePath and not our project directory
                            path = Path.Combine(RelativePath, path);
                        }
                    }

                    // Exceptions from FileInfo Constructor:
                    //   System.ArgumentNullException:
                    //     fileName is null.
                    //
                    //   System.Security.SecurityException:
                    //     The caller does not have the required permission.
                    //
                    //   System.ArgumentException:
                    //     The file name is empty, contains only white spaces, or contains invalid characters.
                    //
                    //   System.IO.PathTooLongException:
                    //     The specified path, file name, or both exceed the system-defined maximum
                    //     length. For example, on Windows-based platforms, paths must be less than
                    //     248 characters, and file names must be less than 260 characters.
                    //
                    //   System.NotSupportedException:
                    //     fileName contains a colon (:) in the middle of the string.
                    FileInfo fileInfo = new FileInfo(path);

                    // Exceptions using FileInfo.Length:
                    //   System.IO.IOException:
                    //     System.IO.FileSystemInfo.Refresh() cannot update the state of the file or
                    //     directory.
                    //
                    //   System.IO.FileNotFoundException:
                    //     The file does not exist.-or- The Length property is called for a directory.
                    bool throwEx = fileInfo.Length == -1;

                    // Exceptions using FileInfo.IsReadOnly:
                    //   System.UnauthorizedAccessException:
                    //     Access to fileName is denied.
                    //     The file described by the current System.IO.FileInfo object is read-only.-or-
                    //     This operation is not supported on the current platform.-or- The caller does
                    //     not have the required permission.
                    throwEx = fileInfo.IsReadOnly;

                    if (!string.IsNullOrEmpty(Extension))
                    {
                        // Validate the Extension of the file.
                        if (Path.GetExtension(path).Equals(Extension, StringComparison.InvariantCultureIgnoreCase))
                        {
                            // Trim the Library Path
                            path = path.Trim();
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return true;

                    }
                }
                catch (ArgumentNullException)
                {
                    //   System.ArgumentNullException:
                    //     fileName is null.
                }
                catch (System.Security.SecurityException)
                {
                    //   System.Security.SecurityException:
                    //     The caller does not have the required permission.
                }
                catch (ArgumentException)
                {
                    //   System.ArgumentException:
                    //     The file name is empty, contains only white spaces, or contains invalid characters.
                }
                catch (UnauthorizedAccessException)
                {
                    //   System.UnauthorizedAccessException:
                    //     Access to fileName is denied.
                }
                catch (PathTooLongException)
                {
                    //   System.IO.PathTooLongException:
                    //     The specified path, file name, or both exceed the system-defined maximum
                    //     length. For example, on Windows-based platforms, paths must be less than
                    //     248 characters, and file names must be less than 260 characters.
                }
                catch (NotSupportedException)
                {
                    //   System.NotSupportedException:
                    //     fileName contains a colon (:) in the middle of the string.
                }
                catch (FileNotFoundException)
                {
                    // System.FileNotFoundException
                    //  The exception that is thrown when an attempt to access a file that does not
                    //  exist on disk fails.
                    throw new FileNotFoundException();
                    // Console.WriteLine(fnfe);
                }
                catch (IOException)
                {
                    //   System.IO.IOException:
                    //     An I/O error occurred while opening the file.
                }
                catch (Exception)
                {
                    // Unknown Exception. Might be due to wrong case or nulll checks.
                }
            }
            else
            {
                // Path contains invalid characters
            }
            return false;
        }

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

    internal class PropertyDescription
    {
        public int Position { get; set; }
        public string Name { get; set; }
    }

    internal class FileResult
    {
        public Type RecordOriginalType { get; set; }

        public string DateTimeFormat { get; set; }

        public IEnumerable<PropertyDescription> Properties { get; set; }

        public IEnumerable<string> Records { get; set; }

        public IEfStatus Status { get; set; }
    }
}
