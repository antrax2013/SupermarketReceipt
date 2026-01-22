using SupermarketReceipt.DiscountFor;
using System.Collections.Generic;
using System.Globalization;

namespace SupermarketReceipt;

public class ShoppingCart
{
    private readonly List<ProductQuantity> _items = [];
    private readonly Dictionary<Product, double> _productQuantities = [];
    private static readonly CultureInfo Culture = CultureInfo.CreateSpecificCulture("en-GB");


    public List<ProductQuantity> GetItems()
    {
        return [.. _items];
    }

    public void AddItem(Product product)
    {
        AddItemQuantity(product, 1.0);
    }


    public void AddItemQuantity(Product product, double quantity)
    {
        _items.Add(new ProductQuantity(product, quantity));

        if (_productQuantities.TryGetValue(product, out double value))
        {
            var newAmount = value + quantity;
            _productQuantities[product] = newAmount;
        }
        else
        {
            _productQuantities.Add(product, quantity);
        }
    }

    public void HandleOffers(Receipt receipt, Dictionary<Product, Offer> offers, SupermarketCatalog catalog)
    {
        foreach (var p in _productQuantities.Keys)
        {
            var quantity = _productQuantities[p];


            Discount discount = null;

            if (offers.TryGetValue(p, out Offer offer))
            {
                var unitPrice = catalog.GetUnitPrice(p);
                discount = GetDiscount(offer.OfferType, p, quantity, unitPrice, offer.Argument);
            }

            if (discount != null)
                receipt.AddDiscount(discount);
        }
    }

    private Discount GetDiscount(SpecialOfferType offerType, Product p, double quantity, double unitPrice, double argument)
    {
        return offerType switch
        {
            SpecialOfferType.TwoForAmount => GetDiscountForTwoForAmount(p, quantity, unitPrice, argument),
            SpecialOfferType.ThreeForTwo => GetDiscountForThreeForTwo(p, quantity, unitPrice),
            SpecialOfferType.PercentDiscount => GetDiscountForTenPercentDiscount(p, quantity, unitPrice, argument),
            SpecialOfferType.FiveForAmount => GetDiscountForFiveForAmount(p, quantity, unitPrice, argument),
            _ => null,
        };
    }

    private Discount GetDiscountForFiveForAmount(Product p, double quantity, double unitPrice, double argument)
    {
        return (new DiscountFiveForAmount(PrintPrice, p, quantity, unitPrice, argument)).GetDiscount();
    }

    private Discount GetDiscountForTenPercentDiscount(Product p, double quantity, double unitPrice, double argument)
    {
        return (new DiscountPercentDiscount(PrintPrice, p, quantity, unitPrice, argument)).GetDiscount();
    }

    private Discount GetDiscountForThreeForTwo(Product p, double quantity, double unitPrice)
    {
        return (new DiscountThreeForTwo(PrintPrice, p, quantity, unitPrice)).GetDiscount();
    }

    private Discount GetDiscountForTwoForAmount(Product p, double quantity, double unitPrice, double argument)
    {
        return (new DiscountTowForAmount(PrintPrice, p, quantity, unitPrice, argument)).GetDiscount();
    }

    private string PrintPrice(double price)
    {
        return price.ToString("N2", Culture);
    }
}