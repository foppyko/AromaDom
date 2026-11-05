using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AromaDom.Web.Models
{
    [Table("Products")]
    public class Product
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("product_type_id")]
        public int ProductTypeId { get; set; }

        [ForeignKey("ProductTypeId")]
        public virtual ProductType ProductType { get; set; }

        [Required]
        [StringLength(20)]
        [Column("article")]
        public string Article { get; set; }

        [Required]
        [StringLength(150)]
        [Column("name")]
        public string Name { get; set; }

        [Column("min_partner_price", TypeName = "decimal")]
        public decimal MinPartnerPrice { get; set; }
    }
}