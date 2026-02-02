namespace SupermarketReceipt.DiscountFor;

public class DiscountPercentDiscount(
    Product p,
    double quantity,
    double unitPrice,
    double argument) : ADiscountFor(p, quantity, null)
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