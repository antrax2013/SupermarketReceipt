using System.Collections.Generic;

namespace SupermarketReceipt;

public class Catalog : SupermarketCatalog
{
    private readonly IDictionary<string, double> _prices = new Dictionary<string, double>();

    public void AddProduct(Product product, double price)
    {
        _prices.Add(product.Name, price);
    }

    public double GetUnitPrice(Product p)
    {
        return _prices[p.Name];
    }
}