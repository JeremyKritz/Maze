using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {
    // use the textboxes beign enabled / disabled as booleans
    // probably a switch statement?
    private Text IntroText;
  //  private GameObject SkipTutorial;
  //  private GameObject MoveToFlag;
  //  private GameObject PullTrigger;
  //  private GameObject EnterMaze;

    private GameObject headset;

    private float platformRange = 2.0f;
    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;

    // Use this for initialization
    void Start () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        IntroText = GameObject.Find("Canvas").GetComponent<Text>();
        //IntroText = GameObject.Find("IntroText");
        //  SkipTutorial = GameObject.Find("SkipTutorial");
        //  MoveToFlag = GameObject.Find("MoveToFlag");
          headset = GameObject.Find("Camera (eye)");
        //  SkipTutorial.SetActive(false);
        /*
        Debug.Log(IntroText.activeInHierarchy);
        IntroText.SetActive(false);
        Debug.Log(IntroText.activeInHierarchy);
        */
    }
	
	// Update is called once per frame
	void Update () {
        
        if (IntroText.enabled)
        {
      
            if (controller.GetPressDown(triggerButton))
            {
                Debug.Log(headset.transform.localPosition);
                Debug.Log("Trigger being pressed for IntroText");
                if( headset.transform.localPosition.x < platformRange && headset.transform.localPosition.x > -platformRange && headset.transform.localPosition.z < platformRange && headset.transform.localPosition.z > -platformRange)
                {
                    Debug.Log("this should work...");

                    // Trigger is being pressed in the right range. (May need to fiddle w range)                 
                    IntroText.enabled = false;
                }
            }
        }
		
	}
}
