using Xunit;
using Assert = Xunit.Assert;

namespace SupermarketReceipt.Test;

public class BundleOfferTests
{

    [Fact]
    public void BundleTest()
    {
        // Given
        Catalog catalog = new();
        var toothbrush = new Product("toothbrush", ProductUnit.Each);
        var toothPaste = new Product("toothPaste", ProductUnit.Each);
        catalog.AddProduct(toothPaste, 0.99);
        catalog.AddProduct(toothbrush, 1.79);

        ShoppingCart cart = new();
        Discount expectedDicount = new(toothPaste, "Bundle", 0.278);

        cart.AddItemQuantity(toothbrush, 1);
        cart.AddItemQuantity(toothPaste, 1);

        BundleOffer bundleOffer = new([toothPaste, toothbrush]);

        // When
        Discount discount = bundleOffer.GetDiscountFor(cart, catalog);

        // Then
        Assert.Equal(discount, expectedDicount);
    }

    [Fact]
    public void BundleTest2()
    {
        // Given
        Catalog catalog = new();
        var mirror = new Product("mirror", ProductUnit.Each);
        var toothPaste = new Product("toothPaste", ProductUnit.Each);
        catalog.AddProduct(toothPaste, 0.99);
        catalog.AddProduct(mirror, 2);

        ShoppingCart cart = new();
        Discount expectedDicount = new(toothPaste, "Bundle", 0.299);

        cart.AddItemQuantity(mirror, 1);
        cart.AddItemQuantity(toothPaste, 1);

        BundleOffer bundleOffer = new([toothPaste, mirror]);

        // When
        Discount discount = bundleOffer.GetDiscountFor(cart, catalog);

        // Then
        Assert.Equal(discount, expectedDicount);
    }
}
