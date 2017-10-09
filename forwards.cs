using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/**
 * Basic implementation of how to use a Vive controller as an input device.

 */
public class forwards : MonoBehaviour
{
    bool disabled = false;
    GameObject cylinder;
    Rigidbody rbCyl;
    GameObject capsule;
    GameObject headset;
    private float speed = 30;
    private float upfloat = 80; // ASSUMING 10 IS TOO HIGH, BUT WE'LL SEE
    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;

    // Use this for initialization
    void Start()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        cylinder = GameObject.Find("CirclePlatform");
        headset = GameObject.Find("Camera (eye)");
        capsule = GameObject.Find("TrackerCapsule");
        rbCyl = cylinder.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
       // Vector3 upV = new Vector3(0.0f, speed * 30, 0.0f);
        Vector3 oldPos = cylinder.transform.position;
        rbCyl.freezeRotation = true;
        
        
        
        if (oldPos.y > 11)
        {
            disabled = true;
            
        }
        else
        {
            disabled = false;
            
        }
       // Debug.Log("aaaah");
        if (controller == null)
        {
            Debug.Log("Controller not initialized");
            return;
        }

        if (controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad) && !disabled) // can only move if not in the air
        {
            Debug.Log("Touchpad works");
            //Debug.Log(capsule.transform.forward);
            float rotation = headset.transform.rotation.eulerAngles.y;
            Debug.Log(rotation);
            float playerX = 0;
            float playerZ = 0;
            if (rotation < 90)
            {
                playerX = Mathf.Sin(ToRad(rotation));
                playerZ = Mathf.Cos(ToRad(rotation));
            }
            if(rotation >=90 && rotation <= 180)
            {
                rotation = rotation - 90;
                playerZ = -Mathf.Sin(ToRad(rotation));
                playerX = Mathf.Cos(ToRad(rotation));
            }
            if(rotation>180 && rotation <= 270)
            {
                rotation = rotation - 180;
                playerX = - Mathf.Sin(ToRad(rotation));
                playerZ = - Mathf.Cos(ToRad(rotation));
            }
            if(rotation > 270 && rotation <= 360)
            {
                rotation = rotation - 270;
                playerZ = Mathf.Sin(ToRad(rotation));
                playerX =  - Mathf.Cos(ToRad(rotation));
            }
            
            Debug.Log(rotation + " " + playerX + " " + playerZ);
            Vector3 playerDirection = new Vector3(playerX, 0, playerZ);
            rbCyl.AddForce(playerDirection * speed);
           // Vector3 capDir = capsule.transform.forward;
            //Vector3 playerDirection = new Vector3(capDir.x, 0, capDir.z);
           // rbCyl.AddForce(playerDirection * speed);
        }

            if (controller.GetPressDown(triggerButton))
        {
            // Find the closest item to the hand in case there are multiple and interact with it
            Debug.Log("ok");
            if (!(disabled)){
                Debug.Log("should go up");
                Vector3 up = new Vector3(0,upfloat,0); // change the number
                rbCyl.AddForce(up);
                cylinder.transform.Translate(up);
                // THIS CURRENTLY DOESNT FALL, BUT IT BASICALLY WORKS

            }
        }
        if (controller.GetPressDown(gripButton))
        {
           // SceneManager.LoadScene("MazeScene");
        }
    }
    float ToDegrees(float rad)
    {
        return (rad * 180.0f) / Mathf.PI;
    }
    float ToRad(float degrees)
    {
        return (degrees * Mathf.PI) / 180;
    }
}

















