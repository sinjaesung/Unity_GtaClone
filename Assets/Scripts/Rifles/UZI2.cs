using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class UZI2 : MonoBehaviour
{
    [Header("Rifle Things")]
    public Camera cam;
    public float giveDamage = 30f;
    public float shootingRange = 70f;
    public float fireCharge = 10f;
    private float nextTimeToShoot = 0f;
    public string gunid;
    public Transform hand;
    public bool isMoving;

    [Header("Rifle Animation and reloading")]
    private int maximumAmmunition = 25;
    public int mag = 10;
    private int presentAmmunition;
    public float reloadingTime = 4.3f;
    public bool setReloading = false;

    [Header("Rifle Effects")]
    public ParticleSystem muzzleSpark;
    public GameObject metalEffect;
    public GameObject bloodEffect;

    [Header("Sounds & UI")]
    public GameObject AmmoOutUI;

    public LayerMask shootLayer;

    private void Awake()
    {
        transform.SetParent(hand);
        Cursor.lockState = CursorLockMode.Locked;
        presentAmmunition = maximumAmmunition;
    }

    private void Update()
    {
        if (setReloading)
            return;

        if (presentAmmunition <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (isMoving == false)
        {
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToShoot)
            {
                nextTimeToShoot = Time.time + 1f / fireCharge;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        if (mag == 0)
        {
            //show ammo out text
            StartCoroutine(ShowAmmoOut());
            return;
        }
        presentAmmunition--;

        if (presentAmmunition == 0)
        {
            mag--;
        }

        muzzleSpark.Play();
        RaycastHit hitInfo;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, shootingRange,shootLayer))
        {
            Debug.Log(gunid + "|hitTransformName:" + hitInfo.transform.name);

            Object obj = hitInfo.transform.GetComponent<Object>();
            PoliceOfficer policeOfficer = hitInfo.transform.GetComponent<PoliceOfficer>();
            CharacterNavigatorScript characterNavigatorScript = hitInfo.transform.GetComponent<CharacterNavigatorScript>();
            PoliceOfficer2 policeOfficer2 = hitInfo.transform.GetComponent<PoliceOfficer2>();
            FBIOfficer fbiofficer = hitInfo.transform.GetComponent<FBIOfficer>();
            Gangster1 gangster1 = hitInfo.transform.GetComponent<Gangster1>();
            Gangster2 gangster2 = hitInfo.transform.GetComponent<Gangster2>();
            Boss bossScript = hitInfo.transform.GetComponent<Boss>();

            if (obj != null)
            {
                obj.objectHitDamage(giveDamage);
                GameObject metalEffectGo = Instantiate(metalEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(metalEffectGo, 1f);
            }
            else if (policeOfficer != null)
            {
                policeOfficer.characterHitDamage(giveDamage);
                GameObject bloodEffectGo = Instantiate(bloodEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(bloodEffectGo, 1f);
            }
            else if (characterNavigatorScript != null)
            {
                characterNavigatorScript.characterHitDamage(giveDamage);
                GameObject bloodEffectGo = Instantiate(bloodEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(bloodEffectGo, 1f);
            }
            else if (policeOfficer2 != null)
            {
                policeOfficer2.characterHitDamage(giveDamage);
                GameObject bloodEffectGo = Instantiate(bloodEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(bloodEffectGo, 1f);
            }
            else if (fbiofficer != null)
            {
                fbiofficer.characterHitDamage(giveDamage);
                GameObject bloodEffectGo = Instantiate(bloodEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(bloodEffectGo, 1f);
            }
            else if (gangster1 != null)
            {
                gangster1.characterHitDamage(giveDamage);
                GameObject bloodEffectGo = Instantiate(bloodEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(bloodEffectGo, 1f);
            }
            else if (gangster2 != null)
            {
                gangster2.characterHitDamage(giveDamage);
                GameObject bloodEffectGo = Instantiate(bloodEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(bloodEffectGo, 1f);
            }
            else if (bossScript != null)
            {
                bossScript.characterHitDamage(giveDamage);
                GameObject bloodEffectGo = Instantiate(bloodEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(bloodEffectGo, 1f);
            }
        }
    }

    IEnumerator Reload()
    {
        setReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadingTime);
        Debug.Log("Done Reloading...");
        presentAmmunition = maximumAmmunition;
        setReloading = false;
    }

    IEnumerator ShowAmmoOut()
    {
        AmmoOutUI.SetActive(true);
        yield return new WaitForSeconds(5f);
        AmmoOutUI.SetActive(false);
    }
}
