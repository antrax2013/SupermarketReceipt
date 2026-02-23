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
        var toothpaste = new Product("toothpaste", ProductUnit.Each);
        catalog.AddProduct(toothbrush, 1.79);
        catalog.AddProduct(toothpaste, 0.99);

        ShoppingCart cart = new();
        cart.AddItemQuantity(toothbrush, 1);
        cart.AddItemQuantity(toothpaste, 1);

        BundleOffer bundleOffer = new([
            new ProductQuantity(toothbrush, 1),
            new ProductQuantity(toothpaste, 1),
        ]);

        // When
        List<Discount> actualDiscounts = bundleOffer.GetDiscountsFor(cart, catalog);

        // Then
        Assert.Contains(new(toothbrush, "Bundle", -0.179), actualDiscounts);
        Assert.Contains(new(toothpaste, "Bundle", -0.099), actualDiscounts);
    }

    [Fact]
    public void AppliesOneBundleDiscount()
    {
        // Given
        SupermarketCatalog catalog = new Catalog();
        var toothbrush = new Product("toothbrush", ProductUnit.Each);
        var toothpaste = new Product("toothpaste", ProductUnit.Each);
        catalog.AddProduct(toothbrush, 1.79);
        catalog.AddProduct(toothpaste, 0.99);

        ShoppingCart cart = new();
        cart.AddItemQuantity(toothbrush, 1);
        cart.AddItemQuantity(toothpaste, 1);
        cart.AddItemQuantity(toothpaste, 1);

        BundleOffer bundleOffer = new([
            new ProductQuantity(toothbrush, 1),
            new ProductQuantity(toothpaste, 1),
        ]);

        // When
        List<Discount> actualDiscounts = bundleOffer.GetDiscountsFor(cart, catalog);

        // Then
        Assert.Equal(2, actualDiscounts.Count);
        Assert.Contains(new(toothbrush, "Bundle", -0.179), actualDiscounts);
        Assert.Contains(new(toothpaste, "Bundle", -0.099), actualDiscounts);
    }

    [Fact]
    public void AppliesTwoBundlesDiscounts()
    {
        // Given
        SupermarketCatalog catalog = new Catalog();
        var toothbrush = new Product("toothbrush", ProductUnit.Each);
        var toothpaste = new Product("toothpaste", ProductUnit.Each);
        catalog.AddProduct(toothbrush, 1.79);
        catalog.AddProduct(toothpaste, 0.99);

        ShoppingCart cart = new();
        cart.AddItemQuantity(toothbrush, 1);
        cart.AddItemQuantity(toothpaste, 1);
        cart.AddItemQuantity(toothpaste, 1);
        cart.AddItemQuantity(toothbrush, 1);

        BundleOffer bundleOffer = new([
            new ProductQuantity(toothbrush, 1),
            new ProductQuantity(toothpaste, 1),
        ]);

        // When
        List<Discount> actualDiscounts = bundleOffer.GetDiscountsFor(cart, catalog);

        // Then
        Assert.Equal(2, actualDiscounts.Count);
        Assert.Contains(new(toothbrush, "Bundle", -0.358), actualDiscounts);
        Assert.Contains(new(toothpaste, "Bundle", -0.198), actualDiscounts);
    }

    [Fact]
    public void NoAppliesBundlesDiscounts()
    {
        // Given
        SupermarketCatalog catalog = new Catalog();
        var toothbrush = new Product("toothbrush", ProductUnit.Each);
        var toothpaste = new Product("toothpaste", ProductUnit.Each);
        catalog.AddProduct(toothbrush, 1.79);
        catalog.AddProduct(toothpaste, 0.99);

        ShoppingCart cart = new();
        cart.AddItemQuantity(toothbrush, 1);
        cart.AddItemQuantity(toothpaste, 1);

        BundleOffer bundleOffer = new([
            new ProductQuantity(toothbrush, 2),
            new ProductQuantity(toothpaste, 2),
        ]);

        // When
        List<Discount> actualDiscounts = bundleOffer.GetDiscountsFor(cart, catalog);

        // Then
        Assert.Empty(actualDiscounts);
    }
}
