using System;

namespace SupermarketReceipt.DiscountFor;

public abstract class ADiscountFor(Product product, double quantity, Func<double, string> doubleToPriceLabel = null)
{
    internal readonly Product product = product;
    internal readonly Func<double, string> doubleToPriceLabel = doubleToPriceLabel;
    internal readonly double quantity = quantity;

    protected int QuantityAsInt { get => (int)quantity; }

    protected virtual bool CanApplyDiscount() => true;

    protected abstract double GetDiscountValue();

    protected abstract string GetDiscountLabel();

    public Discount GetDiscount() => CanApplyDiscount() ? new(product, GetDiscountLabel(), -GetDiscountValue()) : null;
}
