using System.ComponentModel.DataAnnotations;

namespace AromaDom.Web.Models
{
    public class ProductForMaterialViewModel
    {
        public int ProductId { get; set; }

        [Display(Name = "Артикул")]
        public string Article { get; set; }

        [Display(Name = "Наименование продукции")]
        public string ProductName { get; set; }

        [Display(Name = "Тип продукции")]
        public string ProductTypeName { get; set; }

        [Display(Name = "Необходимое количество материала")]
        public decimal RequiredMaterialQuantity { get; set; }

        [Display(Name = "Единица измерения материала")]
        public string MaterialUnit { get; set; }

        [Display(Name = "Минимальная цена для партнёра")]
        public decimal MinPartnerPrice { get; set; }
    }
}