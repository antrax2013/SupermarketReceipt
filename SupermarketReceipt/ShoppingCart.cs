using System.Collections.Generic;

namespace SupermarketReceipt;

public class ShoppingCart
{
    private readonly List<ProductQuantity> _items = [];
    private readonly Dictionary<Product, double> _productQuantities = [];

    public List<ProductQuantity> GetItems()
    {
        return [.. _items];
    }

    public void AddItemQuantity(Product product, double quantity)
    {
        _items.Add(new ProductQuantity(product, quantity));

        if (_productQuantities.TryGetValue(product, out double value))
        {
            var newAmount = value + quantity;
            _productQuantities[product] = newAmount;
        }
        else
        {
            _productQuantities.Add(product, quantity);
        }
    }
}