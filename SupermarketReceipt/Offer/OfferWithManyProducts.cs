using System.Collections.Generic;

namespace SupermarketReceipt.Offer;

public record OfferWithManyProducts(IEnumerable<Product> Products, double Argument) : IOffer;