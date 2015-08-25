/// <summary>
/// Summary description for RoleFileViewModel
/// </summary>
namespace UserLoader
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Serializable]
    public class RoleFileViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 4)]
        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format("\n\t-{0}", this.Name);
        }
    }
}
