using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInputManager : MonoBehaviour
{
     SteamVR_TrackedObject trackedObj;
    public SteamVR_Controller.Device device;

    public enum Rig
    {
        Oculus,
        Vive
    }

    public Rig RigInUse;
    
    public bool isLeftHand;
    
    //Teleport
    private LineRenderer laser;
    public GameObject teleportAimerObject;
    public Vector3 teleportLocation;
    public GameObject player;
    public LayerMask laserMask;

    private float yNudgeAmount = 0.3f; //especifico da altura do teleportAimerObject

    // Use this for initialization
    void Start()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        laser = GetComponentInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Teleport();
    }

    private void Teleport()
    {
        if (isLeftHand)
        {
            if (OVRInput.Get(OVRInput.Button.One))
            {
                //O trigger button está pressionado
                laser.gameObject.SetActive(true);
                //Showing laser
                laser.SetPosition(0, gameObject.transform.position);

                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, 15f, laserMask))
                {
                   
                    teleportAimerObject.SetActive(true);

                    
                    teleportLocation = hit.point;
                    laser.SetPosition(1, teleportLocation);

                    //aimer position
                    teleportAimerObject.transform.position = new Vector3(teleportLocation.x, teleportLocation.y, teleportLocation.z);
                }
                else
                {
                    //teleportLocation = transform.forward * 15 + transform.position;
                    //RaycastHit groundRay;

                    /*if (Physics.Raycast(teleportLocation, -Vector3.up, out groundRay, 17f, laserMask))
                    {
                        teleportLocation = groundRay.point;// new Vector3(transform.position.x + transform.forward.x * 15, groundRay.point.y, transform.position.z + transform.forward.z * 15);
                    }*/
                    laser.SetPosition(1, transform.forward * 15 + transform.position);
                    //aimer position
                    //teleportAimerObject.transform.position = teleportLocation + new Vector3(0, yNudgeAmount, 0);

                }

            }

            if (OVRInput.GetUp(OVRInput.Button.One))
            {
                //o trrigger button foi solto nesse frame
                laser.gameObject.SetActive(false);
                teleportAimerObject.SetActive(false);

                teleportLocation.y = player.transform.position.y;

                player.transform.position = teleportLocation;

            }
        }
    }

    private bool buttonPressUp()
    {
        if(RigInUse == Rig.Vive)
        {
            device = SteamVR_Controller.Input((int)trackedObj.index);
            return device.GetPressUp(SteamVR_Controller.ButtonMask.Grip);
        }
        else
        {
            return OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger); 
        }
        return false;
    }

    private bool buttonPressed()
    {
        if (RigInUse == Rig.Vive)
        {
            device = SteamVR_Controller.Input((int)trackedObj.index);
            return device.GetPress(SteamVR_Controller.ButtonMask.Grip);
        }
        else
        {
            //return OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger);
        }
        return false;
    }
}
