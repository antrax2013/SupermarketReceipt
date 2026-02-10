using System.Collections.Generic;
using System.Linq;

namespace SupermarketReceipt;

public class BundleOffer(List<Product> products) : IOffer
{
    public List<Discount> GetDiscountsFor(ShoppingCart cart, SupermarketCatalog catalog)
    {
        List<Discount> discounts = [];

        if (AllProductsInCart(cart))
        {
            foreach (Product product in products)
            {
                var quantity = cart.GetItems().First(x => x.Product.Equals(product)).Quantity;
                var discountValue = catalog.GetUnitPrice(product) * quantity * 0.10;
                var discount = new Discount(product, "Bundle", discountValue);

                discounts.Add(discount);
            }
        }

        return discounts;
    }

    public bool AllProductsInCart(ShoppingCart cart)
    {
        List<Product> cartProducts = [.. cart.GetItems().Select(x => x.Product)];
        bool allProductsInCart = products.All(p => cartProducts.Contains(p));

        return allProductsInCart;
    }
}