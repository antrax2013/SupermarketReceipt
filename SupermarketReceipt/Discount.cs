namespace SupermarketReceipt;

public class Discount
{
    public Discount(Product product, string description, double discountAmount)
    {
        Product = product;
        Description = description;
        DiscountAmount = discountAmount;
    }

    public string Description { get; }
    public double DiscountAmount { get; }
    public Product Product { get; }

    public override bool Equals(object obj)
    {
        var discount = obj as Discount;
        return discount != null &&
               Product == discount.Product &&
               Description == discount.Description &&
               DiscountAmount == discount.DiscountAmount
               ;
    }
}