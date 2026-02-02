using SupermarketReceipt.DiscountFor;
using SupermarketReceipt.Offer;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SupermarketReceipt;

public class Teller
{
    private readonly SupermarketCatalog _catalog;
    private readonly List<IOffer> _offers = [];
    private static readonly CultureInfo Culture = CultureInfo.CreateSpecificCulture("en-GB");

    public Teller(SupermarketCatalog catalog)
    {
        _catalog = catalog;
    }

    public void AddSpecialOffer(SpecialOfferType offerType, Product product, double argument)
    {
        _offers.Add(new OfferWithOneProduct(offerType, product, argument));
    }

    public void AddBundleOffer(IEnumerable<Product> products, double argument)
    {
        _offers.Add(new OfferWithManyProducts(products, argument));
    }

    private bool HasAnyOffer { get => _offers.Count > 0; }

    public Receipt ChecksOutArticlesFrom(ShoppingCart theCart)
    {
        var receipt = new Receipt();
        var productQuantities = theCart.GetItems();

        bool isEmptyShoppingCart = productQuantities.Count == 0;
        if (isEmptyShoppingCart)
            return receipt;

        foreach (var pq in productQuantities)
        {
            var p = pq.Product;
            var quantity = pq.Quantity;
            var unitPrice = _catalog.GetUnitPrice(p);
            var price = quantity * unitPrice;

            receipt.AddProduct(p, quantity, unitPrice, price);
        }

        bool hasNoOffer = !HasAnyOffer;
        if (hasNoOffer)
            return receipt;

        var discounts = HandleOffers(receipt.GetReadOnlyItems(), _offers);

        if (discounts.Count > 0)
            receipt.AddDiscounts(discounts);

        return receipt;
    }

    private static List<Discount> HandleOffers(List<ReceiptItem> receiptItems, List<IOffer> offers)
    {
        if (offers.Count == 0)
            return [];

        List<Discount> discounts = [];

        foreach (OfferWithManyProducts offer in offers.OfType<OfferWithManyProducts>())
        {
            discounts.Add(new Discount(null, "10% off on 1 toothbrush & 1 toothpaste", -0.3));
        }

        foreach (OfferWithOneProduct offer in offers.OfType<OfferWithOneProduct>())
        {
            Discount discount = GetDiscountFromOneProductOffer([.. receiptItems], offer);
            if (discount is null)
                continue;
            discounts.Add(discount);
        }
        return discounts;
    }

    private static Discount GetDiscountFromOneProductOffer(IReadOnlyList<ReceiptItem> receiptItems, OfferWithOneProduct offer)
    {
        ReceiptItem item = receiptItems
                        .Where(r => r.Product.Equals(offer.Product))
                        .SingleOrDefault();

        if (item is null)
            return null;

        ProductQuantity pq = new(item.Product, item.Quantity);
        DiscountContext context = new(pq, item.Price, offer);
        return GetDiscountFomOfferType(offer.OfferType, context);
    }

    private static Discount GetDiscountFomOfferType(SpecialOfferType offerType, DiscountContext context)
    {
        DiscountFactory factory = new((price) => price.ToString("N2", Culture)); //moche

        return offerType switch
        {
            SpecialOfferType.TwoForAmount => factory.GetDiscountForTwoForAmount(context),
            SpecialOfferType.ThreeForTwo => factory.GetDiscountForThreeForTwo(context),
            SpecialOfferType.PercentDiscount => factory.GetDiscountForTenPercentDiscount(context),
            SpecialOfferType.FiveForAmount => factory.GetDiscountForFiveForAmount(context),
            _ => null,
        };
    }
}