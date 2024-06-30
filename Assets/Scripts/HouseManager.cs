using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    public GameObject dumpster;
    public GameObject trash;
    public bool isUnlocked = true;
    public bool isBooked = false;
    public bool hasTrash = false;
    public Transform WayPointForHouse;

    public int costOfTrashCollection;

    void Start()
    {
        if (isUnlocked)
        {
            dumpster.SetActive(true);
            trash.SetActive(true);
            hasTrash = true;
        }
        else
        {
            dumpster.SetActive(false); 
            trash.SetActive(false);
            hasTrash = false;
        }
    }

    public void OnUnlock()
    {
        isUnlocked = true;
        dumpster.SetActive(true);
        trash.SetActive(true);
        hasTrash = true;
    }

    public void OnTrashPickup()
    {
        trash.SetActive(false);
        hasTrash = false;
        isBooked = false;
        StartCoroutine(ResetTrash());
    }

    public void OnTrashRestored()
    {
        trash.SetActive(true);
        hasTrash = true;
        isBooked = false;
    }

    public IEnumerator ResetTrash()
    {
        int timeForReset = Random.Range(5, 10);
        yield return new WaitForSeconds(timeForReset);
        OnTrashRestored();
    }
}
