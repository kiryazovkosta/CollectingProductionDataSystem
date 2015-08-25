using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CollectingProductionDataSystem.Infrastructure.Mapping;
using CollectingProductionDataSystem.Models.Nomenclatures;

namespace CollectingProductionDataSystem.Web.InputModels
{
    public class EditReasonInputModel:IMapFrom<EditReason>
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}