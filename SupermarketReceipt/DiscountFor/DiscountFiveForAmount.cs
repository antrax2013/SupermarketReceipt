using System;

namespace SupermarketReceipt.DiscountFor;

public class DiscountFiveForAmount(
    Product p,
    double quantity,
    double unitPrice,
    double argument,
    Func<double, string> doubleToPriceLabel) : ADiscountFor(p, quantity, doubleToPriceLabel)
{
    private const int threshold = 5;

    protected override bool CanApplyDiscount()
    {
        return QuantityAsInt >= threshold;
    }

    protected override double GetDiscountValue()
    {
        var numberOfXs = QuantityAsInt / threshold;
        return unitPrice * quantity - (argument * numberOfXs + QuantityAsInt % threshold * unitPrice);
    }

    protected override string GetDiscountLabel()
    {
        return $"{threshold} for " + doubleToPriceLabel(argument);
    }
}