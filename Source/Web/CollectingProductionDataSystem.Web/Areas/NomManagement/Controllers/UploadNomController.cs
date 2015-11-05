using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CollectingProductionDataSystem.Application.Contracts;
using CollectingProductionDataSystem.Data.Contracts;

namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Controllers
{
    public class UploadNomController : AreaBaseController
    {
        private readonly IFileUploadService fileService;

        public UploadNomController(IProductionData dataParam, IFileUploadService fileServiceParam)
            : base(dataParam) 
        {
            this.fileService = fileServiceParam;
        }

        // GET: NomManagement/UploadNom
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(IEnumerable<HttpPostedFileBase> upload)
        {
            if (upload.Count() == 0)
            {
                return Content("Error");
            }

            var inputFile = upload.FirstOrDefault();
            if (inputFile == null)
            {
                return Content("Error");
            }
            var fileNameSplit = inputFile.FileName.Split('.');
            var fileType = fileNameSplit[fileNameSplit.Length - 1];
            if (fileType != "csv" && fileType!="txt")
            {
                return Content("Error");
            }

            return Json(this.fileService.UploadFileToDatabase(inputFile.InputStream, ";", inputFile.FileName));

            // Return an empty string to signify success
            return Content("");
        }
    }
}