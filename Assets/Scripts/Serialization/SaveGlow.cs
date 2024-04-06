using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGlow : MonoBehaviour
{
    public Player player;
    public Missions missions;

    public GameObject SaveUIgameObject;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            player.SavePlayer();
            //UI
            StartCoroutine(SaveUI());
        }
        if(missions.Mission1==false && missions.Mission2 == false && missions.Mission3 == false && missions.Mission4 == false)
        {
            missions.Mission1 = true;
            player.playerMoney += 400;
        }
    }

    IEnumerator SaveUI()
    {
        SaveUIgameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        SaveUIgameObject.SetActive(false);
    }
}
