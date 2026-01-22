using System.Collections.Generic;
using System.Linq;

namespace SupermarketReceipt;

public class BundleOffer(List<Product> products)
{
    public Discount GetDiscountFor(ShoppingCart cart, Catalog catalog)
    {
        List<Product> cartProducts = [.. cart.GetItems().Select(x => x.Product)];

        var notRecognizedProduct = products.FirstOrDefault(product => !cartProducts.Contains(product));

        if (notRecognizedProduct != null)
        {
            return new(null, "aucun discount", 0);
        }

        var total = (from p in products select catalog.GetUnitPrice(p)).Sum();

        return new(products[0], "Bundle", total * 0.10);
    }
}