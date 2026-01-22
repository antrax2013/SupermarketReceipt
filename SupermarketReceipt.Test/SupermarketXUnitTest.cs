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
        teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, toothbrush, 10.0);

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
    public void When_There_Are_Only_A_Toothbrush_And_No_Offert_In_Catalog_Then_Receipt_Total_Should_Be_0_99()
    {
        // ARRANGE
        SupermarketCatalog catalog = new Catalog();
        var toothbrush = new Product("toothbrush", ProductUnit.Each);
        catalog.AddProduct(toothbrush, 0.99);
        var cart = new ShoppingCart();
        cart.AddItemQuantity(toothbrush, 1);

        var teller = new Teller(catalog);

        // ACT
        var receipt = teller.ChecksOutArticlesFrom(cart);

        // ASSERT
        Assert.Equal(0.99, receipt.GetTotalPrice());
    }

    [Theory]
    //[InlineData(SpecialOfferType.TenPercentDiscount, 10, 9)]
    //[InlineData(SpecialOfferType.FiveForAmount, 10, 20)]
    //[InlineData(SpecialOfferType.ThreeForTwo, 10, 7)]
    [InlineData(SpecialOfferType.TwoForAmount, 10, 50)]
    public void Discount_Should_Have_Valid_Amount(SpecialOfferType offerType, double argument, double expectedValue)
    {
        // ARRANGE
        SupermarketCatalog catalog = new Catalog();
        var toothbrush = new Product("toothbrush", ProductUnit.Each);
        catalog.AddProduct(toothbrush, 1);

        var cart = new ShoppingCart();
        cart.AddItemQuantity(toothbrush, 10);

        var teller = new Teller(catalog);
        teller.AddSpecialOffer(offerType, toothbrush, argument);

        // ACT
        var receipt = teller.ChecksOutArticlesFrom(cart);

        // ASSERT
        Assert.Equal(expectedValue, receipt.GetTotalPrice());
    }
}