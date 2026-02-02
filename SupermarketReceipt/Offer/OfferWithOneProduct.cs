namespace SupermarketReceipt.Offer;

public enum SpecialOfferType
{
    ThreeForTwo,
    PercentDiscount,
    TwoForAmount,
    FiveForAmount
}

public record OfferWithOneProduct(SpecialOfferType OfferType, Product Product, double Argument) : IOffer;