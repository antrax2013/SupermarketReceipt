using System;

namespace SupermarketReceipt;

public enum SpecialOfferType
{
    ThreeForTwo,
    TenPercentDiscount,
    TwoForAmount,
    FiveForAmount,
    Bundle,
}

public class Offer
{
    private Product _product;
    private Func<ShoppingCart, bool>? _offerApply = null;

    public bool OfferApply(ShoppingCart cart)
    {
        if (_offerApply == null)
            return true;
        return _offerApply(cart);
    }

    public Product GetProduct()
    {
        return _product;
    }

    public Offer(SpecialOfferType offerType, Product product, double argument, Func<ShoppingCart, bool>? OfferApply = null)
    {
        OfferType = offerType;
        Argument = argument;
        _product = product;
        _offerApply = OfferApply;
    }

    public SpecialOfferType OfferType { get; }
    public double Argument { get; }
}