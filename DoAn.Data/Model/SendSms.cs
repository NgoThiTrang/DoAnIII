using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn.Data.Model
{
    [Table("SendSms")]
    public class SendSms
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int WarningNewId { get; set; }

        public DateTime TimeReceive { get; set; }

        public int DeviceId { get; set; }

        public string PhoneNumber { get; set; }

        public bool isSent { get; set; }

        public string Content { get; set; }

        public double Value { get; set; }

        [ForeignKey("WarningNewId")]
        public virtual WarningNew WarningNew { get; set; }

        [ForeignKey("DeviceId")]
        public virtual Device Device { get; set; }
    }
}