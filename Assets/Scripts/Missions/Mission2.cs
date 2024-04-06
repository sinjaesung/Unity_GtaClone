using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission2 : MonoBehaviour
{
    public Player player;
    public Missions missions;

    private void OnTriggerEnter(Collider other)
    {
        if(missions.Mission1 == true && missions.Mission2 == false && missions.Mission3 == false && missions.Mission4 == false)
        {
            missions.Mission2 = true;
            player.playerMoney += 600;
        }
    }
}
