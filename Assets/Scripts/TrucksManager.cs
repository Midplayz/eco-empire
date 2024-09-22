using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using System;

public class TrucksManager : MonoBehaviour
{
    public static TrucksManager Instance;

    public PathDefiner mainRoadPath;

    public GameObject smallTruckPrefab;
    public GameObject largeTruckPrefab;

    public List<TruckMovementScript> totalTrucks;
    public List<TruckMovementScript> trucksOutForDuty;
    public List<TruckMovementScript> idleTrucks;

    public int timeBetweenTrucks = 2;

    public bool startSendingTrucks = true;
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
        int smallTrucksUnlocked = SavingLoadingManager.Instance.LoadSmallTrucksOwned();
        int largeTrucksUnlocked = SavingLoadingManager.Instance.LoadLargeTrucksOwned();

        for(int i = 0; i < smallTrucksUnlocked; i++)
        {
            OnSmallTruckPurchase();
        }

        if(largeTrucksUnlocked > 0)
        {
            for (int i = 0; i < largeTrucksUnlocked; i++)
            {
                OnLargeTruckPurchase();
            }
        }
    }

    private void Start()
    {
        StartCoroutine(StartSendingTrucks());
    }
    public void OnSmallTruckPurchase()
    {
        GameObject newGameObject = Instantiate(smallTruckPrefab, this.gameObject.transform);
        newGameObject.SetActive(false);
        TruckMovementScript tr = newGameObject.GetComponent<TruckMovementScript>();
        tr.pathDefiner = mainRoadPath;
        tr.stopDuration -= 0.2f * SavingLoadingManager.Instance.LoadSmallTruckLevel();
        totalTrucks.Add(tr);
        idleTrucks.Add(tr);
    }

    public void OnLargeTruckPurchase()
    {
        GameObject newGameObject = Instantiate(largeTruckPrefab, this.gameObject.transform);
        newGameObject.SetActive(false);
        TruckMovementScript tr = newGameObject.GetComponent<TruckMovementScript>();
        tr.stopDuration -= 0.2f * SavingLoadingManager.Instance.LoadLargeTruckLevel();
        tr.pathDefiner = mainRoadPath;
        totalTrucks.Add(tr);
        idleTrucks.Add(tr);
    }

    public IEnumerator StartSendingTrucks()
    {
        while (startSendingTrucks)
        {
            if (idleTrucks.Count>0)
            {
                SendTruckForDuty(idleTrucks[0]);
            }
            yield return new WaitForSeconds(timeBetweenTrucks);
        }
    }

    public void SendTruckForDuty(TruckMovementScript tr)
    {
        if(tr.outForDuty == true)
        {
            return;
        }
        else
        {
            tr.outForDuty = true;
        }
        List<HouseManager> availableHouses = HouseStateManager.Instance.GetAvailableHouses();
        if (availableHouses.Count > 0)
        {
            int targetHouseCount = Math.Min(availableHouses.Count, tr.truckCapacity);
            tr.targetHouses = new Transform[targetHouseCount];

            for (int i = 0; i < targetHouseCount; i++)
            {
                if (availableHouses[i] != null)
                {
                    tr.targetHouses[i] = availableHouses[i].WayPointForHouse;
                    availableHouses[i].isBooked = true;
                }
            }
        }
        else
        {
            tr.targetHouses = new Transform[0];
        }
        tr.gameObject.SetActive(true);
        tr.StartTrip();
        tr.outForDuty = true;
        idleTrucks.Remove(tr);
        trucksOutForDuty.Add(tr);
    }

    public void OnTripComplete(TruckMovementScript tr)
    {
        idleTrucks.Add(tr);
        trucksOutForDuty.Remove(tr);
        tr.gameObject.SetActive(false);
    }

    public void AssignAvailableHousesToTrucks()
    {
        List<HouseManager> availableHouses = HouseStateManager.Instance.GetAvailableHouses();

        foreach (var truck in trucksOutForDuty)
        {
            if (!truck.isFullyAssigned && availableHouses.Count > 0)
            {
                for (int i = 0; i < availableHouses.Count; i++)
                {
                    if (availableHouses[i] != null && !truck.HasCrossedWaypoint(availableHouses[i].WayPointForHouse))
                    {
                        Transform newHouseWaypoint = availableHouses[i].WayPointForHouse;
                        truck.AssignHouse(newHouseWaypoint, availableHouses[i]);
                        availableHouses[i].isBooked = true;
                        availableHouses.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }
}
