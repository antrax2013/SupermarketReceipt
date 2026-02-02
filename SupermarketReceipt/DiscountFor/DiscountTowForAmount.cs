using System;

namespace SupermarketReceipt.DiscountFor;

public class DiscountTowForAmount(
    Product p,
    double quantity,
    double unitPrice,
    double argument,
    Func<double, string> doubleToPriceLabel) : ADiscountFor(p, quantity, doubleToPriceLabel)
{

    protected override double GetDiscountValue()
    {
        var quantityAsInt = (int)quantity;
        var total = argument * (quantityAsInt / 2) + quantityAsInt % 2 * unitPrice;
        return unitPrice * quantity - total;
    }

    protected override string GetDiscountLabel()
    {
        return "2 for " + doubleToPriceLabel(argument);
    }
}