using UnityEngine;
using System.Collections;
using SimpleJSON;

public class CameraMovement : MonoBehaviour {

	public GameObject GO_CArmDeltaX;
	public GameObject GO_CArmAlphaDegree;
	public GameObject GO_CArmBetaDegree;

	private float speed;
	private float smooth;

	private float fNivel;
	private float fAngle;
	private float fDistance;

	//public string url = "10.0.76.155:3000/jsonData";
	public string url = "10.91.1.143:2000/jsonData";
	public WWW www;
	public string answer = "";
	float lastTime;
	float delay;
	private Vector3 initialPos;

	// Use this for initialization
	void Start () {
		speed = 2.0f;
		fNivel = 0.0f;
		fAngle = 0.0f;
		fDistance = 0.0f;
		smooth = 20.0f;

		lastTime = Time.time;
		delay = 0.0f;

		www = new WWW (url);

		initialPos = GO_CArmDeltaX.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		MovingFromKeyboard ();

		//MovingFromCArm ();

		AplyingTransformation ();
	}

	void MovingFromKeyboard()
	{
		//Modifying fDistance 
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			fDistance += 3.0f;
		}

		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			fDistance = -3.0f;
		}

		//Modifying fNivel
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			fNivel -= 5.0f;
		}

		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			fNivel += 5.0f;
		}

		//Modifying xDistance
		if (Input.GetKeyDown (KeyCode.Q)) {
			fAngle -= 5.0f;
		}
		
		if (Input.GetKeyDown (KeyCode.A)) {
			fAngle += 5.0f;
		}
	}

	void MovingFromCArm()
	{
		if (www.isDone == true && lastTime + delay < Time.time) {
			answer = www.text;
			//print (answer);
			
			var N = JSON.Parse (answer);
			
			lastTime = Time.time;
			
			www = new WWW(url);
			
			//fNivel = N["N"].AsFloat;
			//fAngle = -(N["A"].AsFloat - 90.0f);
			fAngle = N["A"].AsFloat + 180.0f;
			//fDistance = N["D"].AsInt;
			
			//print("CArmData: " + fNivel.ToString() + " " + fAngle.ToString() + " " + fDistance.ToString());
		}
	}

	void AplyingTransformation()
	{
		//GO_CArmDeltaX.transform.position = initialPos + new Vector3(fDistance, 0.0f, 0.0f);
		//GO_CArmBetaDegree.transform.rotation = Quaternion.Slerp (GO_CArmBetaDegree.transform.rotation, Quaternion.Euler(fNivel, 0.0f, 0.0f), Time.deltaTime*smooth);
		GO_CArmAlphaDegree.transform.rotation = Quaternion.Slerp (GO_CArmAlphaDegree.transform.rotation, Quaternion.Euler(270.0f, 93.0f+fAngle, 0.0f), Time.deltaTime*smooth);

		fDistance = 0.0f;
	}
}
