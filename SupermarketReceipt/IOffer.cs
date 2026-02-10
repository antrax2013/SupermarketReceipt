using System.Collections.Generic;

namespace SupermarketReceipt;

public interface IOffer
{
    List<Discount> GetDiscountsFor(ShoppingCart cart, SupermarketCatalog catalog);
}