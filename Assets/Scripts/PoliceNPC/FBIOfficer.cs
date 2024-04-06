using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBIOfficer : MonoBehaviour
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

    [Header("FBI AI")]
    public GameObject playerBody;
    public LayerMask PlayerLayer;
    public float visionRadius;
    public float shootingRadius;
    public bool playerInvisionRadius;
    public bool playerInshootingRadius;

    [Header("FBI Shooting Var")]
    public float giveDamageOf = 5f;
    public float shootingRange = 30f;
    public GameObject ShootingRaycastArea;
    public float timebtwShoot;
    bool previouslyShoot;
    public WantedLevel wantedlevelScript;
    public Player player;
    public GameObject bloodEffect;

    public AudioSource audiosource;
    private void Start()
    {
        audiosource = GetComponent<AudioSource>();
        presentHealth = characterHealth;
        playerBody = GameObject.Find("Player");
        wantedlevelScript = GameObject.FindObjectOfType<WantedLevel>();
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
        playerBody = GameObject.Find("Player");
        player = GameObject.FindObjectOfType<Player>();

        playerInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, PlayerLayer);
        playerInshootingRadius = Physics.CheckSphere(transform.position, shootingRadius, PlayerLayer);

        if (!playerInvisionRadius && !playerInshootingRadius && wantedlevelScript.level1 == false || wantedlevelScript.level2 == false ||
            wantedlevelScript.level3 == false || wantedlevelScript.level4 == false || wantedlevelScript.level5 == false)
        {
           // Debug.Log("FBIOfficer walk조건 충족:");
            Walk();
        }
        if (playerInvisionRadius && !playerInshootingRadius &&
             wantedlevelScript.level5 == true)
        {
            //Debug.Log("FBIOfficer ChasePlayer조건 충족:");
            ChasePlayer();         
        }
        if(playerInvisionRadius && playerInshootingRadius &&
            wantedlevelScript.level5 == true)
        {
           // Debug.Log("PoliceOFficer ShootPlayer조건 충족:");
            ShootPlayer();
        }
    }

    public void Walk()
    {
        if (transform.position != destination)
        {
            //일반시민,자동차와는 다른 형태로 좀 처리한다.하늘추적은 안되게 xz방향만 참고해서 쫓아오게해야함.(공중능력없는경우)
            Vector3 destinationDirection = destination - transform.position;
            destinationDirection.y = 0;//대신에 리지드바디까지 처리하여 xz로만 y축 언덕형태의 slope지형또한 결과적으로 나아갈수있게끔처리.
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
            if(playerBody != null)
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

        if(playerBody != null)
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
                Debug.Log("Shooting" + hit.transform.name);

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
