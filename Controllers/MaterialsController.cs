using System.Linq;
using System.Web.Mvc;
using AromaDom.Web.Models;

namespace AromaDom.Web.Controllers
{
    public class MaterialsController : Controller
    {
        private readonly AromaDomContext _context = new AromaDomContext();

        public ActionResult Index()
        {
            var materials = _context.Materials
                .Include("MaterialType")
                .OrderBy(m => m.Name)
                .ToList();

            var model = materials.Select(m => new MaterialViewModel
            {
                Id = m.Id,
                MaterialTypeName = m.MaterialType.Name,
                Name = m.Name,
                MinQuantity = m.MinQuantity,
                StockQuantity = m.StockQuantity,
                Price = m.Price,
                Unit = m.Unit,
                RequiredQuantity = CalculateRequiredQuantity(m.Id)
            }).ToList();

            return View(model);
        }

        public ActionResult Create()
        {
            ViewBag.Title = "Добавление материала";
            PopulateMaterialTypes();
            return View(new MaterialViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MaterialViewModel model)
        {
            ViewBag.Title = "Добавление материала";

            if (model.Price < 0)
            {
                ModelState.AddModelError("Price", "Цена не может быть отрицательной");
            }
            if (model.MinQuantity < 0)
            {
                ModelState.AddModelError("MinQuantity", "Минимальное количество не может быть отрицательным");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var material = new Material
                    {
                        MaterialTypeId = model.MaterialTypeId,
                        Name = model.Name,
                        Price = model.Price,
                        Unit = model.Unit,
                        PackageQuantity = model.PackageQuantity,
                        StockQuantity = model.StockQuantity,
                        MinQuantity = model.MinQuantity
                    };

                    _context.Materials.Add(material);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = $"Материал \"{model.Name}\" успешно добавлен.";
                    return RedirectToAction("Index");
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError("", $"Ошибка при сохранении: {ex.Message}");
                }
            }

            PopulateMaterialTypes();
            return View(model);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var material = _context.Materials.Find(id.Value);
            if (material == null)
            {
                return HttpNotFound();
            }

            var model = new MaterialViewModel
            {
                Id = material.Id,
                MaterialTypeId = material.MaterialTypeId,
                Name = material.Name,
                Price = material.Price,
                Unit = material.Unit,
                PackageQuantity = material.PackageQuantity,
                StockQuantity = material.StockQuantity,
                MinQuantity = material.MinQuantity
            };

            ViewBag.Title = "Редактирование материала";
            PopulateMaterialTypes();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MaterialViewModel model)
        {
            ViewBag.Title = "Редактирование материала";

            if (model.Price < 0)
            {
                ModelState.AddModelError("Price", "Цена не может быть отрицательной");
            }
            if (model.MinQuantity < 0)
            {
                ModelState.AddModelError("MinQuantity", "Минимальное количество не может быть отрицательным");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var material = _context.Materials.Find(model.Id);
                    if (material == null)
                    {
                        return HttpNotFound();
                    }

                    material.MaterialTypeId = model.MaterialTypeId;
                    material.Name = model.Name;
                    material.Price = model.Price;
                    material.Unit = model.Unit;
                    material.PackageQuantity = model.PackageQuantity;
                    material.StockQuantity = model.StockQuantity;
                    material.MinQuantity = model.MinQuantity;

                    _context.SaveChanges();

                    TempData["SuccessMessage"] = $"Материал \"{model.Name}\" успешно обновлён.";
                    return RedirectToAction("Index");
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError("", $"Ошибка при сохранении: {ex.Message}");
                }
            }

            PopulateMaterialTypes();
            return View(model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var material = _context.Materials.Find(id.Value);
            if (material == null)
            {
                return HttpNotFound();
            }

            ViewBag.Title = "Удаление материала";
            return View(material);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var material = _context.Materials.Find(id);
                if (material != null)
                {
                    _context.Materials.Remove(material);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = $"Материал \"{material.Name}\" удалён.";
                }
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = $"Ошибка при удалении: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
        public ActionResult ProductsByMaterial(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var material = _context.Materials
                .Include("MaterialType")
                .FirstOrDefault(m => m.Id == id.Value);

            if (material == null)
            {
                return HttpNotFound();
            }

            var products = (from mp in _context.MaterialProducts
                            join p in _context.Products on mp.ProductId equals p.Id
                            join pt in _context.ProductTypes on p.ProductTypeId equals pt.Id
                            where mp.MaterialId == material.Id
                            select new ProductForMaterialViewModel
                            {
                                ProductId = p.Id,
                                Article = p.Article,
                                ProductName = p.Name,
                                ProductTypeName = pt.Name,
                                RequiredMaterialQuantity = mp.QuantityPerUnit,
                                MaterialUnit = material.Unit,
                                MinPartnerPrice = p.MinPartnerPrice
                            })
                            .OrderBy(p => p.ProductName)
                            .ToList();

            ViewBag.MaterialName = material.Name;
            ViewBag.MaterialTypeName = material.MaterialType.Name;
            ViewBag.MaterialId = material.Id;

            return View(products);
        }
        private decimal CalculateRequiredQuantity(int materialId)
        {
            return _context.MaterialProducts
                .Where(mp => mp.MaterialId == materialId)
                .Sum(mp => (decimal?)mp.QuantityPerUnit) ?? 0;
        }
        private void PopulateMaterialTypes()
        {
            var types = _context.MaterialTypes.OrderBy(t => t.Name).ToList();
            ViewBag.MaterialTypes = new SelectList(types, "Id", "Name");
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