using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn.Data.Model
{
    [Table("WarningNew")]
    public class WarningNew
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime TimeReceive { get; set; }

        public DateTime TimeSent { get; set; }

        public int WarningProfileId { get; set; }

        public int DeviceId { get; set; }

        public string WarningContent { get; set; }

        public double Value { get; set; }

        [ForeignKey("WarningProfileId")]
        public virtual WarningProfile WarningProfile { get; set; }
    }
}