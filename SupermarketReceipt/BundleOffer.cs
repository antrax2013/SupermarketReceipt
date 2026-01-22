using System.Collections.Generic;
using System.Linq;

namespace SupermarketReceipt;

public class BundleOffer(List<Product> products)
{
    public Discount GetDiscountFor(ShoppingCart cart, Catalog catalog)
    {
        List<Product> cartProducts = [.. cart.GetItems().Select(x => x.Product)];

        var notRecogizedProduct = products.FirstOrDefault(product => !cartProducts.Contains(product));

        if (notRecogizedProduct != null)
        {
            return new(null, "aucun discount", 0);
        }

        //foreach (var item in products)
        //{
        //    if (!p.Contains(item))
        //    {
        //        return new(null, "aucun discount", 0); ;
        //    }

        //}

        //products.Where(p => cart.GetItems().Select(c => c.Product.Name).Contains(p.Name)).Sum(p => p.)

        /*foreach (var item in products)
        {
            total += catalog.
        }*/


        return new(products[0], "Bundle", 0.278);
    }
}