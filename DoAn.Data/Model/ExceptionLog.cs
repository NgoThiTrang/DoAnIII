using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn.Data.Model
{
    [Table("ExceptionLog")]
    public class ExceptionLog
    {
        [Key]
        public int Id { set; get; }

        public string Message { set; get; }

        public string ControllerName { get; set; }

        public string StackTrace { set; get; }

        public DateTime CreatedDate { set; get; }
    }
}