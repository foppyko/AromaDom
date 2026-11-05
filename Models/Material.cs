using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AromaDom.Web.Models
{
    [Table("Materials")]
    public class Material
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("material_type_id")]
        public int MaterialTypeId { get; set; }

        [ForeignKey("MaterialTypeId")]
        public virtual MaterialType MaterialType { get; set; }

        [Required]
        [StringLength(150)]
        [Column("name")]
        public string Name { get; set; }

        [Column("price", TypeName = "decimal")]
        public decimal Price { get; set; }

        [Column("stock_quantity", TypeName = "decimal")]
        public decimal StockQuantity { get; set; }

        [Column("min_quantity", TypeName = "decimal")]
        public decimal MinQuantity { get; set; }

        [Column("package_quantity", TypeName = "decimal")]
        public decimal PackageQuantity { get; set; }

        [Required]
        [StringLength(20)]
        [Column("unit")]
        public string Unit { get; set; }
    }
}