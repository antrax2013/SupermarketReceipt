using System.Collections.Generic;
using Xunit;
using Assert = Xunit.Assert;

namespace SupermarketReceipt.Test;

public class BundleOfferTests
{

    [Fact]
    public void BundleAppliesDiscount()
    {
        // Given
        SupermarketCatalog catalog = new Catalog();
        var toothbrush = new Product("toothbrush", ProductUnit.Each);
        var toothPaste = new Product("toothPaste", ProductUnit.Each);
        catalog.AddProduct(toothbrush, 1.79);
        catalog.AddProduct(toothPaste, 0.99);

        ShoppingCart cart = new();

        cart.AddItemQuantity(toothbrush, 1);
        cart.AddItemQuantity(toothPaste, 1);

        BundleOffer bundleOffer = new([toothPaste, toothbrush]);

        // When
        List<Discount> actualDiscounts = bundleOffer.GetDiscountsFor(cart, catalog);

        // Then
        Assert.Contains(new(toothbrush, "Bundle", 0.179), actualDiscounts);
        Assert.Contains(new(toothPaste, "Bundle", 0.099), actualDiscounts);
    }

    [Fact]
    public void ProductQuantities()
    {
        // Given
        SupermarketCatalog catalog = new Catalog();
        var toothbrush = new Product("toothbrush", ProductUnit.Each);
        var toothPaste = new Product("toothPaste", ProductUnit.Each);
        catalog.AddProduct(toothbrush, 1.79);
        catalog.AddProduct(toothPaste, 0.99);

        ShoppingCart cart = new();

        cart.AddItemQuantity(toothbrush, 1);
        cart.AddItemQuantity(toothPaste, 1);
        cart.AddItemQuantity(toothPaste, 1);

        BundleOffer bundleOffer = new([toothPaste, toothbrush]);

        // When
        List<Discount> actualDiscounts = bundleOffer.GetDiscountsFor(cart, catalog);

        // Then
        Assert.Equal(2, actualDiscounts.Count);
        Assert.Contains(new(toothbrush, "Bundle", 0.179), actualDiscounts);
        Assert.Contains(new(toothPaste, "Bundle", 0.198), actualDiscounts);
    }
}
