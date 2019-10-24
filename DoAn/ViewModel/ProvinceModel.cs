using System;

namespace Web.Models
{
    public class ProvinceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public bool isActive { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}