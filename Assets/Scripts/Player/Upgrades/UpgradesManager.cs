using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
    public static UpgradesManager Instance;

    public int smallTrucksOwned = 1;
    public int largeTrucksOwned = 0;
    public List<bool> housesUnlocked = new List<bool>();

    public int smallTruckLevel = 0;
    public int largeTrucksLevel = 0;

    //[Header("UI Area")]
    //public

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadSavedUpgradeValues();
    }

    public void LoadSavedUpgradeValues()
    {
        int cashInHand = 0;
        SavingLoadingManager.Instance.LoadAll(out cashInHand, out smallTrucksOwned, out largeTrucksOwned, out housesUnlocked, out smallTruckLevel, out largeTrucksLevel);
    }
    public void SaveUpgradeValues()
    {
        SavingLoadingManager.Instance.SaveSmallTrucksOwned(smallTrucksOwned);
        SavingLoadingManager.Instance.SaveLargeTrucksOwned(largeTrucksOwned);
        SavingLoadingManager.Instance.SaveHousesUnlocked(housesUnlocked);
        SavingLoadingManager.Instance.SaveSmallTruckLevel(smallTruckLevel);
        SavingLoadingManager.Instance.SaveLargeTruckLevel(largeTrucksLevel);
    }
}
