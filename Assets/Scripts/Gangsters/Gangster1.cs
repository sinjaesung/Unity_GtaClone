using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gangster1 : MonoBehaviour
{
    [Header("Character Info")]
    public float movingSpeed;
    public float runningSpeed;
    private float CurrentmovingSpeed;
    public float turningSpeed = 300f;
    public float stopSpeed = 1f;
    private float characterHealth = 200f;
    public float presentHealth;


    [Header("Destination Var")]
    public Vector3 destination;
    public bool destinationReached;
    public Animator animator;

    [Header("Gangster AI")]
    public GameObject playerBody;
    public LayerMask PlayerLayer;
    public float visionRadius;
    public float shootingRadius;
    public bool playerInvisionRadius;
    public bool playerInshootingRadius;

    [Header("Gangster Shooting Var")]
    public float giveDamageOf = 3f;
    public float shootingRange = 50f;
    public GameObject ShootingRaycastArea;
    public float timebtwShoot;
    bool previouslyShoot;
    public Player player;
    public GameObject bloodEffect;

    public AudioSource audiosource;
    private void Start()
    {
        audiosource = GetComponent<AudioSource>();

        presentHealth = characterHealth;
        playerBody = GameObject.Find("Player");
        CurrentmovingSpeed = movingSpeed;
        player = GameObject.FindObjectOfType<Player>();
        StartCoroutine(StartSetup());
    }
    private IEnumerator StartSetup()
    {
        yield return new WaitForSeconds(1f);

        playerBody = GameObject.Find("Player");
        player = GameObject.FindObjectOfType<Player>();
    }

    private void Update()
    {
        playerInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, PlayerLayer);
        playerInshootingRadius = Physics.CheckSphere(transform.position, shootingRadius, PlayerLayer);

        if (!playerInvisionRadius && !playerInshootingRadius)
        {
          //  Debug.Log("Ganster1 walk���� ����:");
            Walk();
        }
        if (playerInvisionRadius && !playerInshootingRadius)
        {
           // Debug.Log("Ganster1 ChasePlayer���� ����:");
            ChasePlayer();
        }
        if (playerInvisionRadius && playerInshootingRadius)
        {
           // Debug.Log("Ganster1 ShootPlayer���� ����:");
            ShootPlayer();
        }
    }

    public void Walk()
    {
        if (transform.position != destination)
        {
            //�Ϲݽù�,�ڵ����ʹ� �ٸ� ���·� �� ó���Ѵ�.�ϴ������� �ȵǰ� xz���⸸ �����ؼ� �Ѿƿ����ؾ���.(���ߴɷ¾��°��)
            Vector3 destinationDirection = destination - transform.position;
            destinationDirection.y = 0;//��ſ� ������ٵ���� ó���Ͽ� xz�θ� y�� ��������� slope�������� ��������� ���ư����ְԲ�ó��.
            float destinationDistance = destinationDirection.magnitude;

            if (destinationDistance >= stopSpeed)
            {
                if (!playerInshootingRadius)
                {
                    //Turning
                    destinationReached = false;
                    Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turningSpeed * Time.deltaTime);

                    //Move AI
                    transform.Translate(Vector3.forward * movingSpeed * Time.deltaTime);

                    animator.SetBool("Walk", true);
                    animator.SetBool("Shoot", false);
                    animator.SetBool("Run", false);
                }
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

    public void ChasePlayer()
    {
        //Vector3 PlayerToDirection = playerBody.transform.position - transform.position;
        //PlayerToDirection.y = 0;
        playerBody = GameObject.Find("Player");

        if (!playerInshootingRadius)
        {
            transform.position += transform.forward * CurrentmovingSpeed * Time.deltaTime;
            if (playerBody != null)
            {
                transform.LookAt(playerBody.transform);
            }

            animator.SetBool("Run", true);
            animator.SetBool("Walk", false);
            animator.SetBool("Shoot", false);

            CurrentmovingSpeed = runningSpeed;
        }
    }

    public void ShootPlayer()
    {
        CurrentmovingSpeed = 0f;

        //transform.position += transform.forward * CurrentmovingSpeed * Time.deltaTime;
        playerBody = GameObject.Find("Player");
        if (playerBody != null)
        {
            transform.LookAt(playerBody.transform);
        }

        animator.SetBool("Run", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Shoot", true);

        if (!previouslyShoot)
        {
            RaycastHit hit;
            if (Physics.Raycast(ShootingRaycastArea.transform.position, ShootingRaycastArea.transform.forward, out hit, shootingRange))
            {
               // Debug.Log("Shooting" + hit.transform.name);

                PlayerScript playerBody = hit.transform.GetComponent<PlayerScript>();

                if (playerBody != null)
                {
                    playerBody.playerHitDamage(giveDamageOf);
                    GameObject bloodEffectGo = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(bloodEffectGo, 1f);
                }
            }

            previouslyShoot = true;
            Invoke(nameof(ActiveShooting), timebtwShoot);
        }
    }

    private void ActiveShooting()
    {
        previouslyShoot = false;
    }

    public void characterHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;

        if (presentHealth <= 0)
        {
            animator.SetBool("Die", true);
            characterDie();
        }
    }

    private void characterDie()
    {
        audiosource.Play();
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        CurrentmovingSpeed = 0f;
        shootingRange = 0f;
        Object.Destroy(gameObject, 4.0f);
        player.currentkills += 1;
        player.playerMoney += 10;
    }
}
