using System;

namespace SupermarketReceipt.DiscountFor;

public class DiscountThreeForTwo(
    Func<double, string> doubleToPriceLabel,
    Product p,
    double quantity,
    double unitPrice
) : ADiscountFor(p, doubleToPriceLabel, quantity)
{

    protected override bool CanApplyDiscount()
    {
        return QuantityAsInt > 2;
    }

    protected override double GetDiscountValue()
    {
        var numberOfXs = QuantityAsInt / 3;
        return quantity * unitPrice - (numberOfXs * 2 * unitPrice + QuantityAsInt % 3 * unitPrice);
    }

    protected override string GetDiscountLabel()
    {
        return "3 for 2";
    }
}