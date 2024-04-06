using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNavigatorScript : MonoBehaviour
{
    [Header("Character Info")]
    public float movingSpeed;
    public float turningSpeed = 300f;
    public float stopSpeed = 1f;
    private float characterHealth = 100f;
    public float presentHealth;

    [Header("Destination Var")]
    public Vector3 destination;
    public bool destinationReached;
    public Animator animator;
    public Player player;

    public AudioSource audiosource;
    private void Update()
    {
        Walk();
        player = GameObject.FindObjectOfType<Player>();
    }
    private void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }

    public void Walk()
    {
        if(transform.position != destination)
        {
            Vector3 destinationDirection = destination - transform.position;
            //destinationDirection.y = 0;
            float destinationDistance = destinationDirection.magnitude;

           // Debug.Log("CharacterWalk DestinationDirection:" + destinationDirection);
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

    public void characterHitDamage(float takeDamage) {
        presentHealth -= takeDamage;

        if(presentHealth <= 0)
        {
            animator.SetBool("Die", true);
            characterDie();
        }
    }

    private void characterDie()
    {
        audiosource.Play();
        movingSpeed = 0f;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        Object.Destroy(gameObject, 4.0f);
        player.currentkills += 1;
        player.playerMoney += 5;
    }
}
