using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [Header("Item Info")]
    public int itemPrice;
    public int itemRadius;
    public string ItemTag;
    private GameObject ItemToPick;

    [Header("Player Info")]
    public Player player;
    public Inventory inventory;
    public Missions missions;

    private void Start()
    {
        ItemToPick = GameObject.FindWithTag(ItemTag);
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, player.transform.position) < itemRadius)
        {
            if (Input.GetKeyDown("f"))
            {
                if(itemPrice > player.playerMoney)
                {
                    Debug.Log("You are broke");
                    //Show UI
                }
                else
                {
                    if (missions.Mission1 == true && missions.Mission2 == true && missions.Mission3==false && missions.Mission4 == false)
                    {
                        missions.Mission3 = true;
                        player.playerMoney += 800;
                    }

                    if (ItemTag == "HandGunPickup")
                    {
                        player.playerMoney -= itemPrice;

                        inventory.Weapon1.SetActive(true);
                        inventory.isWeapon1Picked = true;
                        Debug.Log(ItemTag);
                    }
                    else if(ItemTag == "ShotGunPickup")
                    {
                        player.playerMoney -= itemPrice;

                        inventory.Weapon2.SetActive(true);
                        inventory.isWeapon2Picked = true;
                        Debug.Log(ItemTag);
                    }
                    else if(ItemTag == "UziPickup")
                    {
                        player.playerMoney -= itemPrice;

                        inventory.Weapon3.SetActive(true);
                        inventory.isWeapon3Picked = true;
                        Debug.Log(ItemTag);
                    }
                    else if(ItemTag == "BazookaPickup")
                    {
                        player.playerMoney -= itemPrice;

                        inventory.Weapon4.SetActive(true);
                        inventory.isWeapon4Picked = true;
                        Debug.Log(ItemTag);
                    }
                    ItemToPick.SetActive(false);
                }
            }
        }
    }
}
