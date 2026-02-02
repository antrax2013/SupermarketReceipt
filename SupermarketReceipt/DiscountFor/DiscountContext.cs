using SupermarketReceipt.Offer;

namespace SupermarketReceipt.DiscountFor;

public record DiscountContext(ProductQuantity ProductQuantity, double UnitPrice, OfferWithOneProduct Offer);
