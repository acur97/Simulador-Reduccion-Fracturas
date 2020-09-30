using UnityEngine;
using System.Collections;

public class TakeScreenShotWeb : MonoBehaviour {

	//public string screenShotURL= "http://www.my-server.com/cgi-bin/screenshot.pl";
	public string screenShotURL= "192.168.1.108:3000/uphoto";
	public GameObject GO_Leg;
	private float lastTime;
	private bool picSelection = false;

	// Use this for initialization
	void Start () {
		//StartCoroutine(UploadPNG());
	}

	void OnMouseDown() {

		if (picSelection == false) {
			GO_Leg.SetActive (true);
			StartCoroutine (UploadPNG1 ());
	
			print ("Leg");
		}

		if (picSelection == true) {
			GO_Leg.SetActive (false);
			StartCoroutine (UploadPNG2 ());
			//GO_Leg.SetActive (true);

			print ("Bones");
		}

		if (picSelection == false) {
			picSelection = true;
		} else {
			picSelection = false;
		}
	}
	
	IEnumerator UploadPNG1() {
		// We should only read the screen after all rendering is complete
		yield return new WaitForEndOfFrame();
		
		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		var tex = new Texture2D( width, height, TextureFormat.RGB24, false );
		
		// Read screen contents into the texture
		tex.ReadPixels( new Rect(0, 0, width, height), 0, 0 );
		tex.Apply();
		
		// Encode texture into PNG
		byte[] bytes = tex.EncodeToPNG();
		Destroy( tex );
		
		// Create a Web Form
		WWWForm form = new WWWForm();
		form.AddField("frameCount", Time.frameCount.ToString());
		form.AddBinaryData("fileUpload", bytes, "screenShot.png", "image/png");
		
		// Upload to a cgi script
		WWW w = new WWW(screenShotURL, form);
		yield return w;
		if (!string.IsNullOrEmpty(w.error)) {
			print(w.error);
		}
		else {
			print("Finished Uploading Screenshot");
		}
	}


	IEnumerator UploadPNG2() {
		// We should only read the screen after all rendering is complete
		yield return new WaitForEndOfFrame();
		
		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		var tex = new Texture2D( width, height, TextureFormat.RGB24, false );
		
		// Read screen contents into the texture
		tex.ReadPixels( new Rect(0, 0, width, height), 0, 0 );
		tex.Apply();
		
		// Encode texture into PNG
		byte[] bytes = tex.EncodeToPNG();
		Destroy( tex );
		
		// Create a Web Form
		WWWForm form = new WWWForm();
		form.AddField("frameCount", Time.frameCount.ToString());
		form.AddBinaryData("fileUpload", bytes, "screenShot.png", "image/png");
		
		// Upload to a cgi script
		WWW w = new WWW(screenShotURL, form);
		yield return w;
		if (!string.IsNullOrEmpty(w.error)) {
			print(w.error);
		}
		else {
			print("Finished Uploading Screenshot");
		}
	}
}
