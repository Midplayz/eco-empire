using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TruckMovementScript : MonoBehaviour
{
    public PathDefiner pathDefiner;
    public Transform[] targetHouses;
    public GameObject trashIntruck;
    public int truckCapacity = 1;
    public float speed = 5f;
    public float rotationSpeed = 2f;
    public float stopDuration = 2f;

    public bool outForDuty = false;

    private int currentHouseIndex = 0;
    private bool isReturning = false;
    private int currentWaypointIndex = 0;
    private Transform targetWaypoint;
    private Vector3 startPosition;

    public void StartTrip()
    {
        if (targetHouses.Count() > truckCapacity)
        {
            Debug.LogError(transform.name + " Has more Targets than Capacity!");
            return;
        }
        if (pathDefiner == null || pathDefiner.waypoints.Length == 0)
        {
            Debug.LogError("PathDefiner is not assigned or has no waypoints.");
            return;
        }
        startPosition = pathDefiner.waypoints[0].position;
        transform.position = new Vector3(startPosition.x, transform.position.y, startPosition.z);
        trashIntruck.SetActive(false);
        StartCoroutine(MoveAlongPath());
    }

    private IEnumerator MoveAlongPath()
    {
        while (true)
        {
            if (currentWaypointIndex < pathDefiner.waypoints.Length)
            {
                targetWaypoint = pathDefiner.waypoints[currentWaypointIndex];
                yield return StartCoroutine(MoveToWaypoint(targetWaypoint));

                if (!isReturning && currentHouseIndex < targetHouses.Length && targetHouses[currentHouseIndex] != null)
                {
                    Transform targetHouse = targetHouses[currentHouseIndex];
                    int closestMainRoadWaypointIndex = FindClosestMainRoadWaypointIndex(targetHouse.position);

                    while (currentWaypointIndex < closestMainRoadWaypointIndex)
                    {
                        targetWaypoint = pathDefiner.waypoints[currentWaypointIndex];
                        yield return StartCoroutine(MoveToWaypoint(targetWaypoint));
                        currentWaypointIndex++;
                    }

                    yield return StartCoroutine(MoveToTargetHouse(targetHouse));
                    currentHouseIndex++;

                    currentWaypointIndex = FindNextWaypointIndex(targetHouse.position);
                }
                else if (isReturning && currentWaypointIndex >= pathDefiner.waypoints.Length - 1)
                {
                    outForDuty = false;

                    currentHouseIndex = 0;
                    currentWaypointIndex = 0;
                    isReturning = false;
                    trashIntruck.SetActive(false);
                    TrucksManager.Instance.OnTripComplete(this);
                    yield break;
                }

                currentWaypointIndex++;
            }
            else
            {
                isReturning = true;
                currentWaypointIndex = pathDefiner.waypoints.Length - 1;
                currentHouseIndex = 0; 
            }
        }
    }

    private IEnumerator MoveToWaypoint(Transform waypoint)
    {
        Vector3 targetPosition = new Vector3(waypoint.position.x, transform.position.y, waypoint.position.z);
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            Debug.Log("Direction: " + direction + " And Waypoint: " + waypoint);
            transform.position += direction * speed * Time.deltaTime;

            float smoothingFactor = 0.1f;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1 - Mathf.Pow(smoothingFactor, Time.deltaTime));

            yield return null;
        }
    }

    private IEnumerator MoveToTargetHouse(Transform house)
    {
        Vector3 targetPosition = new Vector3(house.position.x, transform.position.y, house.position.z);
        yield return StartCoroutine(MoveToWaypoint(house));

        if (transform.forward.z > 0.0f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        yield return new WaitForSeconds(stopDuration);

        house.GetComponent<WayPointHouseHolder>().targetHouse.OnTrashPickup();
        StatsManager.Instance.AdjustCurrency(house.GetComponent<WayPointHouseHolder>().targetHouse.costOfTrashCollection);
        trashIntruck.SetActive(true);
        int closestMainRoadWaypointIndex = FindClosestMainRoadWaypointIndex(transform.position);
        Transform closestMainRoadWaypoint = pathDefiner.waypoints[closestMainRoadWaypointIndex+1];
        yield return StartCoroutine(MoveToWaypoint(closestMainRoadWaypoint));
    }

    private int FindClosestMainRoadWaypointIndex(Vector3 targetPosition)
    {
        int closestIndex = -1;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < pathDefiner.waypoints.Length; i++)
        {
            float distance = Vector3.Distance(pathDefiner.waypoints[i].position, targetPosition);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    private int FindNextWaypointIndex(Vector3 housePosition)
    {
        int closestIndex = FindClosestMainRoadWaypointIndex(housePosition);
        for (int i = 0; i < pathDefiner.waypoints.Length; i++)
        {
            if (i == closestIndex)
            {
                return i;
            }
        }
        return 0;
    }
}
