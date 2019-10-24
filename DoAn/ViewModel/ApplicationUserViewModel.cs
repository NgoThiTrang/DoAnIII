
using DoAn.Data.Model;
using System;
using System.Collections.Generic;

namespace Web.Models
{
    public class ApplicationUserViewModel 
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        public string LoginEndIP { get; set; }
        public int LoginCount { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string FullName { get; set; }
       
        public IEnumerable<ActivityLog> ActivityLogs { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public List<ApplicationGroupViewModel>  Groups { get; set; }
    }
}
