using System;
using System.Collections.Generic;

namespace SteelDbModel.Models
{
    public partial class AspNetUserLogin
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public int UserId { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
