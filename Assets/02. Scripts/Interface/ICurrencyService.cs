
using System.Collections.Generic;

public interface ICurrencyService 
{
    public int[] CurrencyList { get; }

    public void IncreaseCurrency(ECurrencyType type, int amount);
    public int GetCurrency(int type);
    public int GetCurrency(ECurrencyType type);
}
