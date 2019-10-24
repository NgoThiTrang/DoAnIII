using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models;

namespace DoAn.ViewModel
{
    public class ActivityLogViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string UserId { get; set; }   
        public  ApplicationUserViewModel User { get; set; }
    }
}