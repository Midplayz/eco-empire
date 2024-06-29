using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;

    public int cashInHand = 0;
    public int smallTrucksOwned = 1;
    public int largeTrucksOwned = 0;
    public List<bool> housesUnlocked = new List<bool>();

    public int smallTruckLevel = 0;
    public int largeTrucksLevel = 0;
    public event System.Action OnCurrencyAdjusted;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        cashInHand = SavingLoadingManager.Instance.LoadCashInHand();
    }

    public void AdjustCurrency(int amount)
    {
        cashInHand += amount;
        OnCurrencyAdjusted?.Invoke();
        SavingLoadingManager.Instance.SaveCashInHand(cashInHand);
    }

    private void OnApplicationQuit()
    {
        SavingLoadingManager.Instance.SaveCashInHand(cashInHand);
    }
}
