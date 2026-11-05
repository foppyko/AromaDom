using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AromaDom.Web.Models
{
    [Table("MaterialTypes")]
    public class MaterialType
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column("name")]
        public string Name { get; set; }

        [Column("loss_percent", TypeName = "decimal")]
        public decimal LossPercent { get; set; }
    }
}