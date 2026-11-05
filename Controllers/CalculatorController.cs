using System.Web.Mvc;
using AromaDom.Web.Models;

namespace AromaDom.Web.Controllers
{
    public class CalculatorController : Controller
    {
        private readonly AromaDomContext _context = new AromaDomContext();

        public ActionResult Index()
        {

            int result = ProductionCalculator.CalculateProductQuantity(
                _context,
                productTypeId: 1,
                materialTypeId: 1,
                rawMaterialQuantity: 10,
                productParam1: 0.18,
                productParam2: 1.0
            );

            ViewBag.Result = result;
            ViewBag.Title = "Калькулятор производства";

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}