using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [Header("Wheels colliders")]
    public WheelCollider frontRightWheelCollider;
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider backRightWheelCollider;
    public WheelCollider backLeftWheelCollider;

    [Header("Wheels Transforms")]
    public Transform frontRightWheelTransform;
    public Transform frontLeftWheelTransform;
    public Transform backRightWheelTransform;
    public Transform backLeftWheelTransform;
    public Transform vehicleDoor;

    [Header("Vehicle Engine")]
    public float accelerationForce = 100f;
    private float presentAcceleration = 0f;
    public float breakingForce = 200f;
    private float presentBreakForce = 0f;
    public GameObject Carcamera;

    [Header("Vehicle Steering")]
    public float wheelsTorque = 20f;
    private float presentTurnAngle = 0f;

    [Header("Vehicle Security")]
    public PlayerScript player;
    private float radius = 5f;
    private bool isOpened = false;

    [Header("Disable Things")]
    public GameObject AimCam;
    public GameObject crossHair;
    public GameObject ThirdpersonCam;
    public GameObject PlayerCharacter;

    [Header("Light Controls")]
    public GameObject TrailLights;
    public GameObject BreakLights;
    public GameObject FrontLights;
    public GameObject ReverseLights;
    public GameObject LeftIndicators;
    public GameObject RightIndicators;

    [Header("Weapon Scripts")]
    public Shotgun shotgunScript;
    public Handgun handgun1Script;
    public Handgun2 handgun2Script;
    public UZI uziScript;
    public UZI2 uzi2Script;
    public Bazooka bazookaScript;

    private void Update()
    {
        if(Vector3.Distance(transform.position, player.transform.position) < radius)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                //자동차 타기 시도,장전중에는 타는것 제한한다.
                if (handgun1Script.setReloading == false && handgun2Script.setReloading == false
                && bazookaScript.setReloading == false && shotgunScript.setReloading == false
                && uziScript.setReloading == false && uzi2Script.setReloading == false)
                {
                    Debug.Log("VehicleController|handgun1,2Script,uzi1,2Sccript,shotgunscript,bazookascript setReloading status 재장전하고있지 않던 상황에만 자동차탑승:" +
                 handgun1Script.setReloading + "," + handgun2Script.setReloading + "|" + uziScript.setReloading + "," + uzi2Script.setReloading + "|" + shotgunScript.setReloading + "|" + bazookaScript.setReloading);
                    isOpened = true;
                    radius = 5000f;
                    PlayerCharacter.SetActive(false);
                }
                else
                {
                    Debug.Log("VehicleController|handgun1,2Script,uzi1,2Sccript,shotgunscript,bazookascript setReloading status 어느하나라도 재장전하고있던상황이였다면 자동차탑승 명령무시:" +
                handgun1Script.setReloading + "," + handgun2Script.setReloading + "|" + uziScript.setReloading + "," + uzi2Script.setReloading + "|" + shotgunScript.setReloading + "|" + bazookaScript.setReloading);
                }
            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
                player.transform.position = vehicleDoor.transform.position;
                isOpened = false;
                radius = 5f;
                PlayerCharacter.SetActive(true);
            }
            //lightControls
            TrailLights.SetActive(true);
            FrontLights.SetActive(true);
            ReverseLights.SetActive(true);
            LeftIndicators.SetActive(true);
            RightIndicators.SetActive(true);
            BreakLights.SetActive(true);
        }
        else
        {
            //lightControls
            TrailLights.SetActive(false);
            FrontLights.SetActive(false);
            ReverseLights.SetActive(false);
            LeftIndicators.SetActive(false);
            RightIndicators.SetActive(false);
            BreakLights.SetActive(false);
        }

        if (isOpened == true)
        {
            ThirdpersonCam.SetActive(false);
            AimCam.SetActive(false);
            crossHair.SetActive(false);
            Carcamera.SetActive(true);

            MoveVehicle();
            VehicleSteering();
            ApplyBreaks();

        }
        else if(isOpened == false)
        {
            ThirdpersonCam.SetActive(true);
            AimCam.SetActive(true);
            crossHair.SetActive(true);
            Carcamera.SetActive(false);
        }
    }
    void MoveVehicle()
    {
        frontRightWheelCollider.motorTorque = presentAcceleration;
        frontLeftWheelCollider.motorTorque = presentAcceleration;
        backRightWheelCollider.motorTorque = presentAcceleration;
        backLeftWheelCollider.motorTorque = presentAcceleration;

        presentAcceleration = accelerationForce * Input.GetAxis("Vertical");
       // Debug.Log("VehicleController Now presentAcceleration:" + presentAcceleration);
    }

    void VehicleSteering()
    {
        presentTurnAngle = wheelsTorque * Input.GetAxis("Horizontal");
        frontRightWheelCollider.steerAngle = presentTurnAngle;
        frontLeftWheelCollider.steerAngle = presentTurnAngle;
       
        //animate wheels
        SteeringWheels(frontRightWheelCollider, frontRightWheelTransform);
        SteeringWheels(frontLeftWheelCollider, frontLeftWheelTransform);
        SteeringWheels(backRightWheelCollider, backRightWheelTransform);
        SteeringWheels(backLeftWheelCollider, backLeftWheelTransform);
    }

    void SteeringWheels(WheelCollider WC,Transform WT)
    {
        Vector3 position;
        Quaternion rotation;

        WC.GetWorldPose(out position, out rotation);

        Debug.Log("SteeringWheels wheelColliders -> transformPose |presentTurnAngle" + presentTurnAngle+">"+ WT.name + "|" + position + "," + rotation);
        WT.position = position;
        WT.rotation = rotation;
    }

    void ApplyBreaks()
    {
        if (Input.GetKey(KeyCode.Space))
            presentBreakForce = breakingForce;
        else
            presentBreakForce = 0f;

        frontRightWheelCollider.brakeTorque = presentBreakForce;
        frontLeftWheelCollider.brakeTorque = presentBreakForce;
        backRightWheelCollider.brakeTorque = presentBreakForce;
        backLeftWheelCollider.brakeTorque = presentBreakForce;
    }
}
