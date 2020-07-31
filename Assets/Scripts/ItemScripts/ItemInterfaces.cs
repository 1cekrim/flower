using System.Collections;
using System.Collections.Generic;

public interface ITradeAble
{
    int PurchasePrice { get; }
    int SalePrice { get; }
    bool Purchase(int amount = 1);
    bool Sale(int amount = 1);
}
