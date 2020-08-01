public interface IPurchaseAble
{
    int PurchasePrice { get; }
    void UpdatePurchaseButton();
    PurchaseButton PurchaseButtonObject { get; set; }
    PurchaseCategoryEnum Category { get; }
    bool IsPurchaseAble { get; }
}