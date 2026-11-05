using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AromaDom.Web.Models
{
    [Table("ProductTypes")]
    public class ProductType
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column("name")]
        public string Name { get; set; }

        [Column("coefficient", TypeName = "decimal")]
        public decimal Coefficient { get; set; }
    }
}