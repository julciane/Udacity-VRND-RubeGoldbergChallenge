using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInputManager : MonoBehaviour
{
    public bool isLeftController;

    public float throwForce = 1.5f;
    private OVRInput.Controller thisController;

    //Teleport
    private LineRenderer laser;
    public GameObject teleportAimerObject;
    public Vector3 teleportLocation;
    public GameObject player;
    public LayerMask laserMask;
    
    // Use this for initialization
    void Start()
    {
        laser = GetComponentInChildren<LineRenderer>();
        if (isLeftController)
        {
            thisController = OVRInput.Controller.LTouch;
        }
        else
        {
            thisController = OVRInput.Controller.RTouch;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Teleport is mapped to Oculus's Controller X button
        Teleport();
    }

    private void Teleport()
    {
        if (isLeftController)
        {
            //X button is pressed
            if (OVRInput.Get(OVRInput.Button.Three))
            {
                
                //Showing laser
                laser.gameObject.SetActive(true);
                laser.SetPosition(0, gameObject.transform.position);

                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, 15f, laserMask))
                {
                    teleportAimerObject.SetActive(true);

                    teleportLocation = hit.point;
                    laser.SetPosition(1, teleportLocation);

                    //setting aimer position
                    teleportAimerObject.transform.position = new Vector3(teleportLocation.x, teleportLocation.y, teleportLocation.z);
                }
                else
                {
                    //showing the ray
                    laser.SetPosition(1, transform.forward * 15 + transform.position);
                }
            }

            if (OVRInput.GetUp(OVRInput.Button.Three))
            {
                //X button was released this frame, let's teleport our player and hide teleport aimer
                laser.gameObject.SetActive(false);
                teleportAimerObject.SetActive(false);

                teleportLocation.y = player.transform.position.y;
                player.transform.position = teleportLocation;

            }
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.CompareTag("Throwable"))
        {
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, thisController) < 0.1f)
            {
                ThrowObject(col);
            }
            else if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, thisController) > 0.1f)
            {
                GrabObj(col);
            }
        }
    }

    private void GrabObj(Collider col)
    {
        col.transform.SetParent(gameObject.transform);
        col.GetComponent<Rigidbody>().isKinematic = true;
        Debug.Log("You are touching down the trigger on an object");
    }

    private void ThrowObject(Collider col)
    {
        col.transform.SetParent(null);
        Rigidbody rigidBody = col.GetComponent<Rigidbody>();
        rigidBody.isKinematic = false;

        //rigidBody.velocity = device.velocity * throwForce;
        rigidBody.velocity = OVRInput.GetLocalControllerVelocity(thisController) * throwForce;

        //rigidBody.angularVelocity = device.angularVelocity;
        rigidBody.angularVelocity = OVRInput.GetLocalControllerAngularVelocity(thisController);

        Debug.Log("You have released the trigger");
    }
}
