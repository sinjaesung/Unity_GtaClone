using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Boss : MonoBehaviour
{
    float bossHealth = 120f;
    public Animator animator;
    public Player player;
    public Missions missions;

    private void Update()
    {
        if(bossHealth < 120)
        {
            //animation
            animator.SetBool("Shooting", true);
        }
        if(bossHealth <= 0)
        {
            //pass mission
            if (missions.Mission1 == true && missions.Mission2 == true && missions.Mission3 == true && missions.Mission4==false)
            {
                missions.Mission4 = true;
                player.playerMoney += 2000;
            }

            Object.Destroy(gameObject, 4.0f);
            //animation
            animator.SetBool("Died", true);
            animator.SetBool("Shooting", false);
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }
    }

    public void characterHitDamage(float takeDamage)
    {
        bossHealth -= takeDamage;
    }
}
