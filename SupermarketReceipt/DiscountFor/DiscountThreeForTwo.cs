namespace SupermarketReceipt.DiscountFor;

public class DiscountThreeForTwo(
    Product p,
    double quantity,
    double unitPrice
) : ADiscountFor(p, quantity, null)
{
    private const int threshold = 3;

    protected override bool CanApplyDiscount()
    {
        return QuantityAsInt > (threshold - 1);
    }

    protected override double GetDiscountValue()
    {
        var numberOfXs = QuantityAsInt / threshold;
        return quantity * unitPrice - (numberOfXs * (threshold - 1) * unitPrice + QuantityAsInt % threshold * unitPrice);
    }

    protected override string GetDiscountLabel()
    {
        return "3 for 2";
    }
}