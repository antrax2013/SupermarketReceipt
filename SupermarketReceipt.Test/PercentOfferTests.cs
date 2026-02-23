using Xunit;

namespace SupermarketReceipt.Test;

public class PercentOfferTests
{
    [Theory]
    [InlineData("10% off", 10, -0.099, 0.89)]
    [InlineData("20% off", 20, -0.198, 0.792)]
    public void Should_Apply_PercentDiscount(string discountName, double discountPercent, double expectedDiscountedAmount, double expectedTotalPrice)
    {
        // ARRANGE
        SupermarketCatalog catalog = new Catalog();
        var apples = new Product("apples", ProductUnit.Kilo);
        catalog.AddProduct(apples, 0.99);

        var cart = new ShoppingCart();
        cart.AddItemQuantity(apples, 1);

        var teller = new Teller(catalog);
        PercentDiscountOffer offer = new(discountPercent, apples);
        teller.AddSpecialOffer(offer);

        // ACT
        var receipt = teller.ChecksOutArticlesFrom(cart);

        // ASSERT
        Assert.Single(receipt.GetItems());
        Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice(), 0.1);

        Discount expectedDiscount = new(apples, discountName, expectedDiscountedAmount);
        Assert.Collection(receipt.GetDiscounts(),
            actualDiscount => Assert.Equal(expectedDiscount, actualDiscount)
        );

        Assert.Collection(receipt.GetItems(),
            receiptItem =>
            {
                Assert.Equal(0.99, receiptItem.Price, 0.1);
                Assert.Equal(0.99, receiptItem.TotalPrice, 0.1);
                Assert.Equal(1, receiptItem.Quantity, 0.1);
            }
        );
    }

    [Fact]
    public void Should_Not_Apply_Discount()
    {
        // ARRANGE
        SupermarketCatalog catalog = new Catalog();
        var apples = new Product("apples", ProductUnit.Kilo);
        catalog.AddProduct(apples, 0.99);
        var toothBrush = new Product("toothBrush", ProductUnit.Each);
        catalog.AddProduct(toothBrush, 2);

        var cart = new ShoppingCart();
        cart.AddItemQuantity(toothBrush, 1);

        var teller = new Teller(catalog);
        PercentDiscountOffer offer1 = new(10, apples);
        teller.AddSpecialOffer(offer1);

        // ACT
        var receipt = teller.ChecksOutArticlesFrom(cart);

        // ASSERT
        Assert.Single(receipt.GetItems());
        Assert.Equal(2, receipt.GetTotalPrice(), 0.1);

        Assert.Empty(receipt.GetDiscounts());
    }

    [Fact]
    public void Should_Apply_PercentDiscount_For_2_Discounted_Products()
    {
        // ARRANGE
        SupermarketCatalog catalog = new Catalog();
        var apples = new Product("apples", ProductUnit.Kilo);
        catalog.AddProduct(apples, 0.99);
        var toothBrush = new Product("toothBrush", ProductUnit.Each);
        catalog.AddProduct(toothBrush, 2);

        var cart = new ShoppingCart();
        cart.AddItemQuantity(apples, 1);
        cart.AddItemQuantity(toothBrush, 1);

        var teller = new Teller(catalog);
        PercentDiscountOffer offer1 = new(10, apples);
        PercentDiscountOffer offer2 = new(20, toothBrush);
        teller.AddSpecialOffer(offer1);
        teller.AddSpecialOffer(offer2);

        // ACT
        var receipt = teller.ChecksOutArticlesFrom(cart);

        // ASSERT
        Assert.Equal(2, receipt.GetItems().Count);
        Assert.Equal(2.49, receipt.GetTotalPrice(), 0.1);

        Discount expectedDiscount1 = new(apples, "10% off", -0.099);
        Discount expectedDiscount2 = new(toothBrush, "20% off", -0.4);
        Assert.Collection(receipt.GetDiscounts(),
            actualDiscount => Assert.Equal(expectedDiscount1, actualDiscount),
            actualDiscount => Assert.Equal(expectedDiscount2, actualDiscount)
        );
    }

    [Fact]
    public void Should_Apply_PercentDiscount_For_2_Discounted_ToothBrush()
    {
        // ARRANGE
        SupermarketCatalog catalog = new Catalog();
        var toothBrush = new Product("toothBrush", ProductUnit.Each);
        catalog.AddProduct(toothBrush, 2);

        var cart = new ShoppingCart();
        cart.AddItemQuantity(toothBrush, 1);
        cart.AddItemQuantity(toothBrush, 2);


        var teller = new Teller(catalog);
        PercentDiscountOffer offer = new(20, toothBrush);
        teller.AddSpecialOffer(offer);

        // ACT
        var receipt = teller.ChecksOutArticlesFrom(cart);

        // ASSERT
        Assert.Equal(2, receipt.GetItems().Count);
        Assert.Equal(4.8, receipt.GetTotalPrice(), 0.1);

        Discount expectedDiscount1 = new(toothBrush, "20% off", -0.4);
        Discount expectedDiscount2 = new(toothBrush, "20% off", -0.8);
        Assert.Collection(receipt.GetDiscounts(),
            actualDiscount => Assert.Equal(expectedDiscount1, actualDiscount),
            actualDiscount => Assert.Equal(expectedDiscount2, actualDiscount)
        );
    }
}
