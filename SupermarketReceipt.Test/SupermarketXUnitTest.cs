using System.Collections.Generic;
using Xunit;

namespace SupermarketReceipt.Test;

public class SupermarketXUnitTest
{
    [Fact]
    public void TenPercentDiscount()
    {
        // ARRANGE
        SupermarketCatalog catalog = new Catalog();
        var toothbrush = new Product("toothbrush", ProductUnit.Each);
        catalog.AddProduct(toothbrush, 0.99);
        var apples = new Product("apples", ProductUnit.Kilo);
        catalog.AddProduct(apples, 1.99);

        var cart = new ShoppingCart();
        cart.AddItemQuantity(apples, 2.5);

        var teller = new Teller(catalog);
        teller.AddSpecialOffer(SpecialOfferType.PercentDiscount, toothbrush, 10.0);

        // ACT
        var receipt = teller.ChecksOutArticlesFrom(cart);

        // ASSERT
        Assert.Equal(4.975, receipt.GetTotalPrice());
        Assert.Equal(new List<Discount>(), receipt.GetDiscounts());
        Assert.Single(receipt.GetItems());
        var receiptItem = receipt.GetItems()[0];
        Assert.Equal(apples, receiptItem.Product);
        Assert.Equal(1.99, receiptItem.Price);
        Assert.Equal(2.5 * 1.99, receiptItem.TotalPrice);
        Assert.Equal(2.5, receiptItem.Quantity);
    }

    [Fact]
    public void When_The_Shoping_Cart_Is_Empty_Receipt_Then_Total_Should_Be_Equals_To_0()
    {
        // ARRANGE
        SupermarketCatalog catalog = new Catalog();
        var toothbrush = new Product("toothbrush", ProductUnit.Each);
        catalog.AddProduct(toothbrush, 0.99);
        var apples = new Product("apples", ProductUnit.Kilo);
        catalog.AddProduct(apples, 1.99);

        var cart = new ShoppingCart();

        var teller = new Teller(catalog);

        // ACT
        var receipt = teller.ChecksOutArticlesFrom(cart);

        // ASSERT
        Assert.Equal(0, receipt.GetTotalPrice());
    }

    [Fact]
    public void When_The_Catalog_Is_Empty_Then_Receipt_Total_Should_Be_Equals_To_0()
    {
        // ARRANGE
        SupermarketCatalog catalog = new Catalog();
        var cart = new ShoppingCart();

        var teller = new Teller(catalog);

        // ACT
        var receipt = teller.ChecksOutArticlesFrom(cart);

        // ASSERT
        Assert.Equal(0, receipt.GetTotalPrice());
    }

    [Fact]
    public void When_There_Are_Only_2_Toothbrushs_And_No_Offert_In_Catalog_Then_Receipt_Total_Should_Be_1_98()
    {
        // ARRANGE
        SupermarketCatalog catalog = new Catalog();
        var toothbrush = new Product("toothbrush", ProductUnit.Each);
        catalog.AddProduct(toothbrush, 0.99);
        var cart = new ShoppingCart();
        cart.AddItemQuantity(toothbrush, 1);
        cart.AddItemQuantity(toothbrush, 1);

        var teller = new Teller(catalog);

        // ACT
        var receipt = teller.ChecksOutArticlesFrom(cart);

        // ASSERT
        Assert.Equal(1.98, receipt.GetTotalPrice());
    }

    [Theory]
    [InlineData(SpecialOfferType.PercentDiscount, 10, 10, 9, "10% off")]
    [InlineData(SpecialOfferType.PercentDiscount, 10, 20, 8, "20% off")]
    [InlineData(SpecialOfferType.FiveForAmount, 10, 4, 8, "5 for 4.00")]
    [InlineData(SpecialOfferType.FiveForAmount, 6, 1, 2, "5 for 1.00")]
    [InlineData(SpecialOfferType.ThreeForTwo, 10, 1, 7, "3 for 2")]
    [InlineData(SpecialOfferType.TwoForAmount, 10, 1, 5, "2 for 1.00")]
    public void Discount_Should_Have_Expected_Total_And_DiscountLabel_From_Theories(SpecialOfferType offerType, double quantity, double argument, double expectedTotal, string expectedDiscountLabel)
    {
        // ARRANGE
        SupermarketCatalog catalog = new Catalog();
        var toothbrush = new Product("toothbrush", ProductUnit.Each);
        catalog.AddProduct(toothbrush, 1);

        var cart = new ShoppingCart();
        cart.AddItemQuantity(toothbrush, quantity);

        var teller = new Teller(catalog);
        teller.AddSpecialOffer(offerType, toothbrush, argument);

        // ACT
        var receipt = teller.ChecksOutArticlesFrom(cart);
        var discounts = receipt.GetDiscounts();

        // ASSERT
        Assert.Equal(expectedTotal, receipt.GetTotalPrice());
        Assert.Single(discounts);
        Assert.Equal(expectedDiscountLabel, discounts[0].Description);
    }
}