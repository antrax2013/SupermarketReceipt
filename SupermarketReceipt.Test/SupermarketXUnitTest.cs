using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SupermarketReceipt.Test;

public class SupermarketXUnitTest
{
    [Fact]
    public void TenPercentDiscountIsNotApplied()
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
        Assert.Equal(1.99, receiptItem.Price, 0.01);
        Assert.Equal(2.5 * 1.99, receiptItem.TotalPrice, 0.01);
        Assert.Equal(2.5, receiptItem.Quantity, 0.01);
    }

    [Fact]
    public void TenPercentDiscountAppliedOnToothbrush()
    {
        // ARRANGE
        SupermarketCatalog catalog = new Catalog();
        var toothbrush = new Product("toothbrush", ProductUnit.Each);
        catalog.AddProduct(toothbrush, 0.99);

        var cart = new ShoppingCart();
        cart.AddItemQuantity(toothbrush, 1);

        var teller = new Teller(catalog);
        teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, toothbrush, 10.0);

        // ACT
        var receipt = teller.ChecksOutArticlesFrom(cart);

        // ASSERT
        Assert.Single(receipt.GetItems());

        Discount expectedDiscount = new(toothbrush, "10% off", -0.099);
        Discount actualDiscount = receipt.GetDiscounts().First();
        Assert.Equal(actualDiscount, expectedDiscount);

        var receiptItem = receipt.GetItems()[0];
        Assert.Equal(toothbrush, receiptItem.Product);

        Assert.Equal(0.89, receipt.GetTotalPrice(), 0.01);
        Assert.Equal(0.99, receiptItem.Price, 0.01);
        Assert.Equal(0.99, receiptItem.TotalPrice, 0.01);
        Assert.Equal(1, receiptItem.Quantity, 0.01);
    }

    [Fact]
    public void TenPercentDiscountAppliedOn2Toothbrush()
    {
        // ARRANGE
        SupermarketCatalog catalog = new Catalog();
        var toothbrush = new Product("toothbrush", ProductUnit.Each);
        catalog.AddProduct(toothbrush, 0.99);

        var cart = new ShoppingCart();
        cart.AddItemQuantity(toothbrush, 2);

        var teller = new Teller(catalog);
        teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, toothbrush, 10.0);

        // ACT
        var receipt = teller.ChecksOutArticlesFrom(cart);

        // ASSERT
        Assert.Single(receipt.GetItems());

        Discount expectedDiscount = new(toothbrush, "10% off", -0.198);
        Discount actualDiscount = receipt.GetDiscounts().First();
        Assert.Equal(actualDiscount, expectedDiscount);

        var receiptItem = receipt.GetItems()[0];
        Assert.Equal(toothbrush, receiptItem.Product);

        Assert.Equal(1.78, receipt.GetTotalPrice(), 0.01);
        Assert.Equal(0.99, receiptItem.Price, 0.01);
        Assert.Equal(1.98, receiptItem.TotalPrice, 0.01);
        Assert.Equal(2, receiptItem.Quantity, 0.01);
    }

    [Fact]
    public void TenPercentDiscountAppliedOn1And1Toothbrush()
    {
        // ARRANGE
        SupermarketCatalog catalog = new Catalog();
        var toothbrush = new Product("toothbrush", ProductUnit.Each);
        catalog.AddProduct(toothbrush, 0.99);

        var cart = new ShoppingCart();
        cart.AddItemQuantity(toothbrush, 1);
        cart.AddItemQuantity(toothbrush, 1);

        var teller = new Teller(catalog);
        teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, toothbrush, 10.0);

        // ACT
        var receipt = teller.ChecksOutArticlesFrom(cart);

        // ASSERT
        Assert.Equal(2, receipt.GetItems().Count);
        Assert.Equal(1.78, receipt.GetTotalPrice(), 0.1);

        Discount expectedDiscount = new(toothbrush, "10% off", -0.198);
        Assert.Collection(receipt.GetDiscounts(),
            actualDiscount => Assert.Equal(expectedDiscount, actualDiscount)
        );


        Assert.Equal(1.78, receipt.GetTotalPrice(), 0.1);
        Assert.Collection(receipt.GetItems(),
            receiptItem =>
            {
                Assert.Equal(0.99, receiptItem.Price, 0.1);
                Assert.Equal(0.99, receiptItem.TotalPrice, 0.1);
                Assert.Equal(1, receiptItem.Quantity, 0.1);
            },
            receiptItem =>
            {
                Assert.Equal(0.99, receiptItem.Price, 0.1);
                Assert.Equal(0.99, receiptItem.TotalPrice, 0.1);
                Assert.Equal(1, receiptItem.Quantity, 0.1);
            }
        );
    }

    [Fact]
    public void TenPercentDiscountAppliedOn1And1Appels()
    {
        // ARRANGE
        SupermarketCatalog catalog = new Catalog();
        var apples = new Product("apples", ProductUnit.Kilo);
        catalog.AddProduct(apples, 0.99);

        var cart = new ShoppingCart();
        cart.AddItemQuantity(apples, 1);
        cart.AddItemQuantity(apples, 1);

        var teller = new Teller(catalog);
        teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, apples, 10.0);

        // ACT
        var receipt = teller.ChecksOutArticlesFrom(cart);

        // ASSERT
        Assert.Equal(2, receipt.GetItems().Count);
        Assert.Equal(1.78, receipt.GetTotalPrice(), 0.1);

        Discount expectedDiscount = new(apples, "10% off", -0.198);
        Assert.Collection(receipt.GetDiscounts(),
            actualDiscount => Assert.Equal(expectedDiscount, actualDiscount)
        );


        Assert.Equal(1.78, receipt.GetTotalPrice(), 0.1);
        Assert.Collection(receipt.GetItems(),
            receiptItem =>
            {
                Assert.Equal(0.99, receiptItem.Price, 0.1);
                Assert.Equal(0.99, receiptItem.TotalPrice, 0.1);
                Assert.Equal(1, receiptItem.Quantity, 0.1);
            },
            receiptItem =>
            {
                Assert.Equal(0.99, receiptItem.Price, 0.1);
                Assert.Equal(0.99, receiptItem.TotalPrice, 0.1);
                Assert.Equal(1, receiptItem.Quantity, 0.1);
            }
        );
    }

    [Fact]
    public void BundleDiscount()
    {
        // ARRANGE
        SupermarketCatalog catalog = new Catalog();
        var toothbrush = new Product("toothbrush", ProductUnit.Each);
        var toothPaste = new Product("toothPaste", ProductUnit.Each);
        catalog.AddProduct(toothbrush, 1.79);
        catalog.AddProduct(toothPaste, 0.99);


        var cart = new ShoppingCart();
        cart.AddItem(toothbrush);
        cart.AddItem(toothPaste);

        var teller = new Teller(catalog);
        BundleOffer bundleOffer = new([toothPaste, toothPaste]);
        teller.AddSpecialOffer(bundleOffer);

        // ACT
        var receipt = teller.ChecksOutArticlesFrom(cart);

        // ASSERT
        Assert.Equal(2.6, receipt.GetTotalPrice(), 0.1);

        Assert.Collection(receipt.GetDiscounts(),
            actualDiscount => Assert.Equal(new(toothbrush, "Bundle", 0.099), actualDiscount),
            actualDiscount => Assert.Equal(new(toothPaste, "Bundle", 0.179), actualDiscount)
        );

        Assert.Equal(1.78, receipt.GetTotalPrice(), 0.1);
        Assert.Collection(receipt.GetItems(),
            receiptItem =>
            {
                Assert.Equal(0.99, receiptItem.Price, 0.1);
                Assert.Equal(0.99, receiptItem.TotalPrice, 0.1);
                Assert.Equal(1, receiptItem.Quantity, 0.1);
            },
            receiptItem =>
            {
                Assert.Equal(1.79, receiptItem.Price, 0.1);
                Assert.Equal(1.79, receiptItem.TotalPrice, 0.1);
                Assert.Equal(1, receiptItem.Quantity, 0.1);
            }
        );
    }
}