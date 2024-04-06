using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarWaypointNavigator : MonoBehaviour
{
    [Header(" Car AI")]
    public CarNavigatorScript car;
    public Waypoint currentWaypoint;

    private void Awake()
    {
        car = GetComponent<CarNavigatorScript>();
    }

    private void Start()
    {
        car.LocateDestination(currentWaypoint.GetPosition());
    }

    private void Update()
    {
        if (car.destinationReached)
        {
            currentWaypoint = currentWaypoint.nextWaypoint;
            car.LocateDestination(currentWaypoint.GetPosition());
        }
    }
}
