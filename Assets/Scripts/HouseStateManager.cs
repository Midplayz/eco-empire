using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HouseStateManager : MonoBehaviour
{
    public static HouseStateManager Instance;

    public List<HouseManager> HouseManagerList;

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

        List<bool> unlockedHouses = new List<bool>();
        unlockedHouses = SavingLoadingManager.Instance.LoadHousesUnlocked();
        for(int i = 0; i <HouseManagerList.Count; i++)
        {
            HouseManagerList[i].isUnlocked = unlockedHouses[i];
        }
    }

    public List<HouseManager> GetAvailableHouses()
    {
        return HouseManagerList.Where(houseManager => houseManager.isUnlocked && houseManager.hasTrash && !houseManager.isBooked).ToList();
    }
}
