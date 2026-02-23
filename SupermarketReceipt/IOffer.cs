using System.Collections.Generic;

namespace SupermarketReceipt;

public interface IOffer
{
    List<Discount> GetDiscountsFor(ShoppingCart cart, SupermarketCatalog catalog);

    List<ProductQuantity> GetDiscountsProducts();

    double GetDiscountValueFor(double unitPrice, double quantity);
}