using UnityEngine;
using System.Collections;

public class TakeScreenShot : MonoBehaviour {

	public GameObject GO_Leg;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//print ("Passing for here");
	}

	void OnMouseDown() {
		ScreenCapture.CaptureScreenshot("XRayPicLeg.png");
		print ("Storing XRayPicLeg");
		GO_Leg.SetActive (false);
		ScreenCapture.CaptureScreenshot("XRayPicBones.png");
		print ("Storing XRayPicBones");
	}
}
