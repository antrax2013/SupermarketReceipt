using SupermarketReceipt.DiscountFor;
using System;

namespace SupermarketReceipt;

public class DiscountFactory(Func<double, string> printPrice)
{
    public Discount GetDiscountForFiveForAmount(DiscountContext context)
    {
        return (new DiscountFiveForAmount(context.ProductQuantity.Product, context.ProductQuantity.Quantity, context.UnitPrice, context.Offer.Argument, printPrice)).GetDiscount();
    }

    public Discount GetDiscountForTenPercentDiscount(DiscountContext context)
    {
        return (new DiscountPercentDiscount(context.ProductQuantity.Product, context.ProductQuantity.Quantity, context.UnitPrice, context.Offer.Argument)).GetDiscount();
    }

    public Discount GetDiscountForThreeForTwo(DiscountContext context)
    {
        return (new DiscountThreeForTwo(context.ProductQuantity.Product, context.ProductQuantity.Quantity, context.UnitPrice)).GetDiscount();
    }

    public Discount GetDiscountForTwoForAmount(DiscountContext context)
    {
        return (new DiscountTowForAmount(context.ProductQuantity.Product, context.ProductQuantity.Quantity, context.UnitPrice, context.Offer.Argument, printPrice)).GetDiscount();
    }
}