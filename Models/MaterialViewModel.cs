using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AromaDom.Web.Models
{
    public class MaterialViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Тип материала")]
        [Required(ErrorMessage = "Тип материала обязателен для заполнения")]
        public int MaterialTypeId { get; set; }

        [Display(Name = "Наименование материала")]
        [Required(ErrorMessage = "Наименование обязательно для заполнения")]
        [StringLength(150, ErrorMessage = "Наименование не должно превышать 150 символов")]
        public string Name { get; set; }

        [Display(Name = "Цена единицы материала")]
        [Required(ErrorMessage = "Цена обязательна для заполнения")]
        [Range(0.01, 999999.99, ErrorMessage = "Цена должна быть положительным числом")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Цена должна быть с точностью до сотых (например, 450.00)")]
        public decimal Price { get; set; }

        [Display(Name = "Единица измерения")]
        [Required(ErrorMessage = "Единица измерения обязательна")]
        [StringLength(20, ErrorMessage = "Единица измерения не должна превышать 20 символов")]
        public string Unit { get; set; }

        [Display(Name = "Количество в упаковке")]
        [Required(ErrorMessage = "Количество в упаковке обязательно")]
        [Range(0.01, 999999, ErrorMessage = "Количество в упаковке должно быть положительным")]
        public decimal PackageQuantity { get; set; }

        [Display(Name = "Количество на складе")]
        [Required(ErrorMessage = "Количество на складе обязательно")]
        [Range(0, 999999, ErrorMessage = "Количество на складе не может быть отрицательным")]
        public decimal StockQuantity { get; set; }

        [Display(Name = "Минимальное количество")]
        [Required(ErrorMessage = "Минимальное количество обязательно")]
        [Range(0, 999999, ErrorMessage = "Минимальное количество не может быть отрицательным")]
        public decimal MinQuantity { get; set; }

        public string MaterialTypeName { get; set; }
        public decimal RequiredQuantity { get; set; }
    }
}