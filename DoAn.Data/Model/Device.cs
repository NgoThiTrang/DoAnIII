using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn.Data.Model
{
    [Table("Device")]
    public class Device
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(30)]
        public string Imei { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(20)]
        public string WarningPhoneNumber { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(30)]
        public string WarningMail { get; set; }

        public DateTime? CreatedDate { get; set; }
        public int DistrictId { get; set; }
        public bool? isActive { get; set; }
        [ForeignKey("DistrictId")]
        public virtual District District { get; set; }
    }
}