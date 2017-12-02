using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInputManager : MonoBehaviour
{
    public bool isLeftHand;
    
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
    }

    // Update is called once per frame
    void Update()
    {
        //Teleport is mapped to Oculus's Controller X button
        Teleport();
    }

    private void Teleport()
    {
        if (isLeftHand)
        {
            //X button is pressed
            if (OVRInput.Get(OVRInput.Button.One))
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

            if (OVRInput.GetUp(OVRInput.Button.One))
            {
                //X button was released this frame, let's teleport our player and hide teleport aimer
                laser.gameObject.SetActive(false);
                teleportAimerObject.SetActive(false);

                teleportLocation.y = player.transform.position.y;
                player.transform.position = teleportLocation;

            }
        }
    }
}
