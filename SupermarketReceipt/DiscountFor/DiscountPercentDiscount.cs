using System;

namespace SupermarketReceipt.DiscountFor;

public class DiscountPercentDiscount(
    Func<double, string> doubleToPriceLabel,
    Product p,
    double quantity,
    double unitPrice,
    double argument
) : ADiscountFor(p, doubleToPriceLabel, quantity)
{
    public static double GetDiscountValue(double quantity, double unitPrice, double argument)
    {
        return quantity * unitPrice * argument / 100.0;
    }

    protected override double GetDiscountValue()
    {
        return quantity * unitPrice * argument / 100.0;
    }

    protected override string GetDiscountLabel()
    {
        return argument + "% off";
    }
}