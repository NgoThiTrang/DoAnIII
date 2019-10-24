using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn.Data.Model
{
    [Table("WarningProfile")]
    public class WarningProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public double Up_Thres { get; set; }

        public double Low_Thres { get; set; }

        public int ProcessTimeOut { get; set; }
        public string UserId { get; set; }
        [Required]
        [MaxLength(200)]
        public string WarningContent { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(30)]
        public string PropertiesName { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

    }
}