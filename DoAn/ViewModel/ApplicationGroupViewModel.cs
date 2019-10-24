
using DoAn.Data.Model;
using System.Collections.Generic;

namespace Web.Models
{
    public class ApplicationGroupViewModel 
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public List<ApplicationRoleViewModel> Roles { get; set; }
    }
}