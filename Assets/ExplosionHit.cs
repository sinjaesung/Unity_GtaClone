using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;

public class ExplosionHit : MonoBehaviour
{
    [SerializeField] public float damage=150f;
    private void Start()
    {
        Debug.Log("폭발오브젝트 생성 스플레시대미지 줘야함");
    }

    private void OnTriggerStay(Collider other)
    {
        Object obj = other.transform.GetComponent<Object>();
        PoliceOfficer policeOfficer = other.transform.GetComponent<PoliceOfficer>();
        CharacterNavigatorScript characterNavigatorScript = other.transform.GetComponent<CharacterNavigatorScript>();
        PoliceOfficer2 policeOfficer2 = other.transform.GetComponent<PoliceOfficer2>();
        FBIOfficer fbiofficer = other.transform.GetComponent<FBIOfficer>();
        Gangster1 gangster1 = other.transform.GetComponent<Gangster1>();
        Gangster2 gangster2 = other.transform.GetComponent<Gangster2>();
        Boss bossScript = other.transform.GetComponent<Boss>();

        Debug.Log("ExplosionHit hit Collider other target:" + other.transform.name);
        if (obj != null)
        {
            obj.objectHitDamage(damage/3);
        }
        else if (policeOfficer != null)
        {
            policeOfficer.characterHitDamage(damage / 3);
        }
        else if (characterNavigatorScript != null)
        {
            characterNavigatorScript.characterHitDamage(damage / 3);
        }
        else if (policeOfficer2 != null)
        {
            policeOfficer2.characterHitDamage(damage / 3);
        }
        else if (fbiofficer != null)
        {
            fbiofficer.characterHitDamage(damage / 3);
        }
        else if (gangster1 != null)
        {
            gangster1.characterHitDamage(damage / 3);
        }
        else if (gangster2 != null)
        {
            gangster2.characterHitDamage(damage / 3);
        }
        else if (bossScript != null)
        {
            bossScript.characterHitDamage(damage / 3);
        }
    }
}
