using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn.Data.Model
{
    [Table("DataPackage")]
    public class DataPackage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int DeviceId { get; set; }

        public DateTime TimePackage { get; set; }

        public double PH { get; set; }

        public double Salt { get; set; }

        public double Oxy { get; set; }

        public double Temp { get; set; }

        public double H2S { get; set; }

        public double NH3 { get; set; }

        public double NH4Min { get; set; }

        public double NH4Max { get; set; }

        public double NO2Min { get; set; }

        public double NO2Max { get; set; }

        public double SulfideMin { get; set; }

        public double SulfideMax { get; set; }

        [ForeignKey("DeviceId")]
        public virtual Device Device { get; set; }
    }
}