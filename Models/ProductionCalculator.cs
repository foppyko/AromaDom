using System;
using System.Linq;

namespace AromaDom.Web.Models
{
    public static class ProductionCalculator
    {
        public static int CalculateProductQuantity(
            AromaDomContext context,
            int productTypeId,
            int materialTypeId,
            int rawMaterialQuantity,
            double productParam1,
            double productParam2)
        {
            if (rawMaterialQuantity <= 0)
            {
                return -1;
            }

            if (productParam1 <= 0 || productParam2 <= 0)
            {
                return -1;
            }

            var productType = context.ProductTypes.Find(productTypeId);
            if (productType == null)
            {
                return -1;
            }

            var materialType = context.MaterialTypes.Find(materialTypeId);
            if (materialType == null)
            {
                return -1;
            }

            double coefficient = (double)productType.Coefficient;

            double lossPercent = (double)materialType.LossPercent;

            double rawMaterialPerUnit = productParam1 * productParam2 * coefficient;

            if (rawMaterialPerUnit <= 0)
            {
                return -1;
            }
            double effectiveRawMaterial = rawMaterialQuantity * (1.0 - lossPercent);

            int productQuantity = (int)Math.Floor(effectiveRawMaterial / rawMaterialPerUnit);

            return productQuantity;
        }
    }
}