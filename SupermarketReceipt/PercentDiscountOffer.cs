using System.Collections.Generic;
using System.Linq;

namespace SupermarketReceipt;

public class PercentDiscountOffer(double percent, Product product, string label = "") : IOffer
{
    private double Percent { get; } = percent;
    private Product Product { get; } = product;

    private readonly string Label = label;

    private List<ProductQuantity> discountsProducts;

    private string DiscountLabel
        => string.IsNullOrWhiteSpace(Label) ?
            $"{Percent}% off" : Label;


    public double GetDiscountValueFor(double unitPrice, double quantity) => -quantity * unitPrice * Percent / 100.0;

    public List<ProductQuantity> GetDiscountsProducts() => discountsProducts;

    public List<Discount> GetDiscountsFor(ShoppingCart cart, SupermarketCatalog catalog)
    {
        discountsProducts = [.. cart.GetItems().Where(i => i.Product.Equals(Product))];
        List<Discount> discounts = [];

        if (discountsProducts.Count == 0)
            return [];

        foreach (var discountProduct in discountsProducts)
        {
            var (product, quantity) = discountProduct;

            var unitPrice = catalog.GetUnitPrice(product);
            var discountValue = GetDiscountValueFor(unitPrice, quantity);

            discounts.Add(new Discount(product, DiscountLabel, discountValue));
        }

        return discounts;
    }
}
