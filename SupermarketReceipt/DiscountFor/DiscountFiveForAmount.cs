using System;

namespace SupermarketReceipt.DiscountFor;

public class DiscountFiveForAmount(
    Func<double, string> doubleToPriceLabel,
    Product p,
    double quantity,
    double unitPrice,
    double argument
) : ADiscountFor(p, doubleToPriceLabel, quantity)
{
    protected override bool CanApplyDiscount()
    {
        return QuantityAsInt >= 5;
    }

    protected override double GetDiscountValue()
    {
        var numberOfXs = QuantityAsInt / 5;
        return unitPrice * quantity - (argument * numberOfXs + QuantityAsInt % 5 * unitPrice);
    }

    protected override string GetDiscountLabel()
    {
        return "5 for " + doubleToPriceLabel(argument);
    }
}