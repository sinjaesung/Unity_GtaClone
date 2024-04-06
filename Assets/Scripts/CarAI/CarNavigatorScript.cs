using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarNavigatorScript : MonoBehaviour
{
    [Header("Car Info")]
    public float movingSpeed;
    public float turningSpeed = 300f;
    public float stopSpeed = 1f;
    public GameObject sensor;
    float detectionRange = 10f;

    [Header("Destination Var")]
    public Vector3 destination;
    public bool destinationReached;
    public Player player;

    private void Update()
    {
        RaycastHit hitInfo;
        if(Physics.Raycast(sensor.transform.position,sensor.transform.forward,out hitInfo, detectionRange))
        {
            //Debug.Log(hitInfo.transform.name);

            CharacterNavigatorScript CharacterNPC = hitInfo.transform.GetComponent<CharacterNavigatorScript>();
            Player playerBody = hitInfo.transform.GetComponent<Player>();

            if(CharacterNPC != null)
            {
                movingSpeed = 0f;
                return;
            }
            else if(playerBody != null)
            {
                movingSpeed = 0f;
                return;
            }
        }

        Drive();
    }

    public void Drive()
    {
        movingSpeed = 10f;
        if (transform.position != destination)
        {
            Vector3 destinationDirection = destination - transform.position;
            //destinationDirection.y = 0;
            float destinationDistance = destinationDirection.magnitude;

            //Debug.Log("CarDrive DestinationDirection:" + destinationDirection);
            if (destinationDistance >= stopSpeed)
            {
                //Turning
                destinationReached = false;
                Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turningSpeed * Time.deltaTime);

                //Move AI
                transform.Translate(Vector3.forward * movingSpeed * Time.deltaTime);
            }
            else
            {
                destinationReached = true;
            }
        }
    }

    public void LocateDestination(Vector3 destination)
    {
        this.destination = destination;
        destinationReached = false;
    }
}
