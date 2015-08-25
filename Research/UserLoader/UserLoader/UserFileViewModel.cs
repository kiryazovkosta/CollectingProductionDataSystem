/// <summary>
/// Summary description for UserFileViewModel
/// </summary>
namespace UserLoader
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    [Serializable]
    public class UserFileViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 4)]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        [StringLength(50, MinimumLength = 2)]
        public string MiddleName { get; set; }

        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }

        [XmlIgnore]
        public bool IsChangePasswordRequired { get; set; }

        [StringLength(250, MinimumLength = 4)]
        public string Occupation { get; set; }

        [XmlArrayItem(typeof(RoleFileViewModel))]
        public List<RoleFileViewModel> Roles { get; set; }

        public override string ToString()
        {
           var userStringBuilder = new StringBuilder();
           userStringBuilder.Append(this.UserName + "\t");
           userStringBuilder.Append(this.Email + "\t");
           userStringBuilder.Append(this.FirstName + "\t");
           userStringBuilder.Append(this.LastName + "\t");
           userStringBuilder.Append(this.MiddleName + "\t");
           userStringBuilder.Append(this.LastName + "\t");
           userStringBuilder.Append(this.Occupation + "\t");
           foreach (var role in this.Roles)
           {
               userStringBuilder.Append(role.ToString());               
           }

           return userStringBuilder.ToString();
        }
    }
}
