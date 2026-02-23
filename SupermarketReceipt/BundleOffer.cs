using System.Collections.Generic;
using System.Linq;

namespace SupermarketReceipt;

public class BundleOffer : IOffer
{
    private readonly List<ProductQuantity> discountsProducts = [];
    private readonly List<ProductQuantity> products;
    //private readonly Dictionary<Product, IOffer> offers = [];
    private readonly string label = "Bundle";

    public BundleOffer(List<ProductQuantity> products)
    {
        this.products = products;
        //foreach (var p in products)
        //{
        //    offers[p.Product] = new PercentDiscountOffer(10, p.Product, label);
        //}
    }

    public double GetDiscountValueFor(double unitPrice, double quantity) => -quantity * unitPrice * 10 / 100.0;

    public List<ProductQuantity> GetDiscountsProducts() => discountsProducts;

    public List<Discount> GetDiscountsFor(ShoppingCart cart, SupermarketCatalog catalog)
    {
        List<Discount> discounts = [];

        if (AllProductsInCart(cart))
        {
            int nbBundles = CountNumberOfBundlesInCart(cart);

            foreach (ProductQuantity pq in products)
            {
                var item = cart.GetItems().First(x => x.Product.Equals(pq.Product));
                var unitPrice = catalog.GetUnitPrice(pq.Product);
                var discountValue = GetDiscountValueFor(unitPrice, nbBundles);

                var discount = new Discount(pq.Product, label, discountValue);
                discounts.Add(discount);

                discountsProducts.Add(new ProductQuantity(pq.Product, nbBundles));
            }
        }

        return discounts;
    }

    public int CountNumberOfBundlesInCart(ShoppingCart cart)
    {
        return
            cart.GetItems()
            .GroupBy(x => x.Product)
            .Min(g =>
            {
                var bundleProductQuantity = products.Find(p => p.Product == g.Key).Quantity;
                return (int)(g.Sum(x => x.Quantity) / bundleProductQuantity);
            });
    }

    private bool AllProductsInCart(ShoppingCart cart)
    {
        List<ProductQuantity> cartProducts = [..
            cart.GetItems()
            .GroupBy(x => x.Product)
            .Select(g => new ProductQuantity(g.Key, g.Sum(x => x.Quantity)))
        ];

        bool allProductsInCart = products.All(
            p => cartProducts.Any(cp => cp.Product == p.Product && cp.Quantity >= p.Quantity)
        );

        return allProductsInCart;
    }


}